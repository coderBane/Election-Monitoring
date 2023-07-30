using Election2023.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Election2023.DataStore.Database.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;
    private List<AuditEntry> _tempAuditEntries = new();

    public AuditableEntityInterceptor(ICurrentUserService currentUserService, IDateTimeService dateTimeService)
    {
        _currentUserService = currentUserService;
        _dateTimeService = dateTimeService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Update(eventData.Context);
        TryInsertAuditLog(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        Update(eventData.Context);
        TryInsertAuditLog(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        TryUpdateTempPropsForAuditLog(eventData.Context);
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        TryUpdateTempPropsForAuditLog(eventData.Context);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private void Update(DbContext? context)
    {
        if (context == null) return;

        string userId = _currentUserService.UserId;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedOn = _dateTimeService.UtcNow;
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedOn = _dateTimeService.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userId;
                    entry.Entity.LastModifiedOn = _dateTimeService.UtcNow;
                    break;
            }
        }
    }

    private void TryInsertAuditLog(DbContext? context)
    {
        if (context is null) return;

        string userId = _currentUserService.UserId;
        List<AuditEntry> auditEntries = new();

        context.ChangeTracker.DetectChanges();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                UserId = userId,
                TableName = entry.Metadata.ClrType.Name
            };

            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey() && property.CurrentValue is not null)
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch(entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = AuditType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue!;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = AuditType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue!;
                        break;
                    
                    case EntityState.Modified when property.IsModified:
                        if (property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = AuditType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue!;
                        }
                        break;
                }
            }
        }

        foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryproperties))
            context.Set<Audit>().Add(auditEntry.ToAudit());

        _tempAuditEntries = auditEntries.Where(a => a.HasTemporaryproperties).ToList();
    }

    private void TryUpdateTempPropsForAuditLog(DbContext? context) //FIXME: id's not correct
    {
        if (context is null) return;

        if (_tempAuditEntries.Any())
        {
            foreach (var auditEntry in _tempAuditEntries)
            {
                foreach (var property in auditEntry.TemporaryProperties)
                {
                    if (property.Metadata.IsPrimaryKey() && property.CurrentValue is not null)
                        auditEntry.KeyValues[property.Metadata.Name] = property.CurrentValue;
                    else
                        auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue!;
                }
                context.Set<Audit>().Add(auditEntry.ToAudit());
            }
        }

        _tempAuditEntries.Clear();
    }
}

