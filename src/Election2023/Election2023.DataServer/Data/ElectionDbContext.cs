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

        public DbSet<LGA> LGAs => Set<LGA>();
        public DbSet<Ward> Wards => Set<Ward>();
        public DbSet<State> States => Set<State>();
        public DbSet<District> Districts => Set<District>();
        public DbSet<PollingUnit> PollingUnits => Set<PollingUnit>();
        // public DbSet<Constituency> Constituencies => Set<Constituency>();
        public DbSet<StateConstituency> StatesConstituencies => Set<StateConstituency>();
        public DbSet<FederalConstituency> FederalConstituencies => Set<FederalConstituency>();

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
                b.Property(l => l.Code)
                 .HasMaxLength(9)
                 .IsFixedLength(true)
                 .IsRequired();

                b.HasDiscriminator();
            });

            modelBuilder.Entity<District>(b => 
            {
                b.HasIndex(d => d.Name).IsUnique();
            });

            modelBuilder.Entity<FederalConstituency>(b => 
            {
                b.HasIndex(fc => fc.Name).IsUnique();
            });

            modelBuilder.Entity<StateConstituency>(b => 
            {
                b.HasIndex(sc => sc.Name);
            });

            modelBuilder.Entity<PollingUnit>(b =>
            {
                b.HasIndex(pu => pu.TerritoryCode).IsUnique();

                b.Property(pu => pu.TerritoryCode)
                 .HasMaxLength(12)
                 .IsFixedLength(true)
                 .IsRequired();
            });

            modelBuilder.Entity<Ward>(b => 
            {
                b.HasIndex(w => w.Name);
            });

            modelBuilder.Entity<LGA>(b => 
            {
                b.HasIndex(lg => lg.Name);
            });
        }
    }
}

