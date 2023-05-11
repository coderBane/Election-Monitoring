using Election2023.Domain.Common;
using Election2023.Application.Interfaces.Services;

using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Election2023.DataStore.Database
{
    public class ElectionDbContext : IdentityDbContext
	{
        private readonly string _databaseDefaultSchema = "app";
        // private readonly ICurrentUserService _currentUserService;
        private readonly DateTime _currentDateTime;

        public ElectionDbContext(DbContextOptions<ElectionDbContext> options) : base(options)
		{
            _currentDateTime = DateTime.UtcNow;
            // _currentUserService = currentUserService;
		}

        // public DbSet<LGA> LGAs => Set<LGA>();
        // public DbSet<Ward> Wards => Set<Ward>();
        // public DbSet<State> States => Set<State>();
        // public DbSet<District> Districts => Set<District>();
        // public DbSet<PollingUnit> PollingUnits => Set<PollingUnit>();
        // public DbSet<StateConstituency> StatesConstituencies => Set<StateConstituency>();
        // public DbSet<FederalConstituency> FederalConstituencies => Set<FederalConstituency>();

        // public DbSet<Election> Elections => Set<Election>();
        // public DbSet<ElectionResult> ElectionResults => Set<ElectionResult>();

        public DbSet<Candidate> Candidates => Set<Candidate>();
        public DbSet<PoliticalParty> PoliticalParties => Set<PoliticalParty>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            string userId = "Initiator";

            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.CreatedOn = _currentDateTime;
                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModifiedOn = _currentDateTime;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = userId;
                        entry.Entity.LastModifiedOn = _currentDateTime;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            if (!Database.IsSqlite())
            {
                builder.HasDefaultSchema("identity");
                builder.HasDefaultSchema(_databaseDefaultSchema);

                if (Database.IsNpgsql())
                {
                    NpgsqlModelBuilderExtensions.UseHiLo(builder, "db_seq_hilo", schema: _databaseDefaultSchema);

                    builder.HasPostgresEnum<Party>(_databaseDefaultSchema);
                    builder.HasPostgresEnum<StateName>(_databaseDefaultSchema);
                    builder.HasPostgresEnum<Gender>(_databaseDefaultSchema);
                    builder.HasPostgresEnum<ElectionType>(_databaseDefaultSchema);
                }

                if (Database.IsSqlServer())
                {
                    SqlServerModelBuilderExtensions.UseHiLo(builder, "DBSeqHiLo", schema: _databaseDefaultSchema);
                }
            }

            builder.Entity<Candidate>(b =>
            {
                b.HasKey(i => i.Id);

                b.HasOne(p => p.Party)
                 .WithMany()
                 .HasForeignKey(p => p.PoliticalPartyAbbrv)
                 .HasPrincipalKey(c => c.Abbrv);

                b.Property(c => c.Id)
                 .ValueGeneratedNever();

                b.HasIndex(n => new {n.Firstname, n.Surname});

                if (!Database.IsNpgsql())
                    b.Property(c => c.ManifestoSnippets)
                    .HasConversion(
                        v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => JsonSerializer.Deserialize<string[]>(v, (JsonSerializerOptions?)null) ?? Array.Empty<string>(),
                        new ValueComparer<string[]>(
                            (l, r) => l != null && r != null && l.SequenceEqual(r),
                            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                            c => c
                        )
                    );
            });

            // builder.Entity<State>()
            //     .Property(s => s.Code)
            //     .ValueGeneratedOnAdd();

            // builder.Entity<Legislative>(b =>
            // {
            //     b.Property(l => l.Code)
            //      .HasMaxLength(9)
            //      .IsFixedLength(true)
            //      .IsRequired();

            //     b.HasIndex(l => l.Code).IsUnique();
            //     b.HasIndex(l => l.Name);

            //     b.HasDiscriminator();
            // });

            // builder.Entity<PollingUnit>(b =>
            // {
            //     b.HasIndex(pu => pu.TerritoryCode).IsUnique();

            //     b.Property(pu => pu.TerritoryCode)
            //      .HasMaxLength(12)
            //      .IsFixedLength(true)
            //      .IsRequired();
            // });

            // builder.Entity<Ward>(b =>
            // {
            //     b.HasIndex(w => w.Name);
            // });

            // builder.Entity<LGA>(b =>
            // {
            //     b.HasIndex(lg => lg.Name);
            // });
        }
    }
}

