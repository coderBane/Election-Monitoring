using System.Text.Json;
using System.Reflection;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Election2023.DataStore.Database.Interceptors;

namespace Election2023.DataStore.Database
{
    public class ElectionDbContext : IdentityDbContext
	{
        private const string _identitySchema = "identity";
        private const string _databaseDefaultSchema = "app";

        private readonly AuditableEntityInterceptor _auditableEntityInterceptor;

        public ElectionDbContext(DbContextOptions<ElectionDbContext> options, 
            AuditableEntityInterceptor auditableEntityInterceptor) : base(options)
		{
            _auditableEntityInterceptor = auditableEntityInterceptor;
		}

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

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

