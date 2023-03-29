#region global_usings
global using Election2023.Entities.Models;
global using Election2023.Entities.Models.Voting;
global using Election2023.Entities.Models.Candidacy;
global using Microsoft.EntityFrameworkCore;
#endregion

using Npgsql;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Election2023.DataServer.Data
{
    public class ElectionDbContext : IdentityDbContext
    {
        public ElectionDbContext(DbContextOptions<ElectionDbContext> options)
            : base(options) { }

        public DbSet<Ward> Wards => Set<Ward>();
        public DbSet<State> States => Set<State>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<Constituency> Constituencies => Set<Constituency>();
        public DbSet<Legislative> Legislatives => Set<Legislative>();
        public DbSet<PollingUnit> PollingUnits => Set<PollingUnit>();
        public DbSet<LocalGovernment> LocalGovernmentsAreas => Set<LocalGovernment>();

        public static void NpgMappings(ref NpgsqlDataSourceBuilder builder)
        {
            //builder.MapEnum<Party>();
            //builder.MapEnum<Gender>();
            builder.MapEnum<StateName>("app.state_name");
            //builder.MapEnum<ElectionType>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            if (!Database.IsSqlite())
            {
                modelBuilder.HasDefaultSchema("identity");
                modelBuilder.HasDefaultSchema("app");

                if (Database.IsNpgsql())
                {
                    NpgsqlModelBuilderExtensions.UseHiLo(modelBuilder, "db_seq_hilo", schema: "app");

                    //modelBuilder.HasPostgresEnum<Party>();
                    modelBuilder.HasPostgresEnum<StateName>("app");
                    //modelBuilder.HasPostgresEnum<Gender>();
                    //modelBuilder.HasPostgresEnum<ElectionType>();
                }

                if (Database.IsSqlServer())
                {
                    SqlServerModelBuilderExtensions.UseHiLo(modelBuilder, "DBSeqHiLo", schema: "app");
                }
            }

            modelBuilder.Entity<Legislative>(b => 
            {
                b.HasIndex(l => l.Name)
                 .HasDatabaseName("DCNameIndex");

                b.HasDiscriminator();
            });

            modelBuilder.Entity<PollingUnit>(b =>
            {
                b.HasIndex(pu => pu.TerritoryCode)
                 .HasDatabaseName("PuCodeIndex")
                 .IsUnique();

                b.Property(pu => pu.TerritoryCode)
                 .HasMaxLength(12)
                 .IsFixedLength(true)
                 .IsRequired();
            });

            modelBuilder.Entity<Ward>(b => 
            {
                b.HasIndex(w => w.Name)
                 .HasDatabaseName("WardNameIndex")
                 .IsUnique();
            });

            modelBuilder.Entity<LocalGovernment>(b => 
            {
                b.HasIndex(lg => lg.Name)
                 .HasDatabaseName("LgaNameIndex")
                 .IsUnique();
            });

            modelBuilder.Entity<LocalGovernmentConstituency>()
                .HasKey(lgc => new { lgc.LocalGovernmentId, lgc.ConstituencyId});
        }
    }
}
