using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Election2023.DataStore.Database.Interceptors;

namespace Election2023.DataStore.Database
{
    public class ElectionDbContext : IdentityDbContext
	{
        private readonly string _identitySchema = "identity";
        private readonly string _databaseDefaultSchema = "app";

        private readonly AuditableEntityInterceptor _auditableEntityInterceptor;

        public ElectionDbContext(DbContextOptions<ElectionDbContext> options, 
            AuditableEntityInterceptor auditableEntityInterceptor) : base(options)
		{
            _auditableEntityInterceptor = auditableEntityInterceptor;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
            => optionsBuilder.AddInterceptors(_auditableEntityInterceptor);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (!Database.IsNpgsql())
                foreach(var property in builder.Model.GetEntityTypes()
                    .SelectMany(t => t.GetProperties())
                    .Where(p => p.ClrType.IsEnum))
                {
                    var type = typeof(EnumToStringConverter<>).MakeGenericType(property.ClrType);
                    var converter = Activator.CreateInstance(type, new ConverterMappingHints()) as ValueConverter;
                    property.SetValueConverter(converter);
                }

            base.OnModelCreating(builder);

            if (!Database.IsSqlite())
            {
                builder.HasDefaultSchema(_identitySchema);
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
                 .HasPrincipalKey(x => x.Abbrv)
                 .HasForeignKey(p => p.PartyAbbrv)
                 .IsRequired();

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

