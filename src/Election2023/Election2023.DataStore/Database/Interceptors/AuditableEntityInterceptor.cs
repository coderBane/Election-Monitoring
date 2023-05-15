using Election2023.Domain.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Election2023.DataStore.Database.Interceptors;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTimeService;

    public AuditableEntityInterceptor(ICurrentUserService currentUserService, IDateTimeService dateTimeService)
    {
        _currentUserService = currentUserService;
        _dateTimeService = dateTimeService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        Update(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        Update(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
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
}

