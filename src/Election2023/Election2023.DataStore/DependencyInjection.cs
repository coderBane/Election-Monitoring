using Election2023.DataStore.Database;
using Election2023.DataStore.Repositories;
using Election2023.Application.Interfaces.Repositories;

using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Election2023.Application.Interfaces.Services;

namespace Election2023.DataStore
{
    public static class DependencyInjection
	{
        private static NpgsqlDataSource NpgHelper(string? conn)
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(conn);
            dataSourceBuilder.MapEnum<Party>("app.party");
            dataSourceBuilder.MapEnum<Gender>("app.gender");
            dataSourceBuilder.MapEnum<StateName>("app.state_name");
            dataSourceBuilder.MapEnum<ElectionType>("app.election_type");

            return dataSourceBuilder.Build();
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = configuration.GetValue("Provider", "Sqlite")
                                        .ToLower().Select((x, i) => i == 0 ? char.ToUpper(x) : x);

            provider = string.Join("", provider);

            services.AddDbContextFactory<ElectionDbContext>(options => _ = provider switch
            {
                "Sqlite" => options.UseSqlite(configuration.GetConnectionString("DefaultConnection"), opt =>
                {
                    opt.MigrationsAssembly("Election2023.MigrationsSqlite");
                }),

                "Mssql" => options.UseSqlServer(configuration.GetConnectionString("MssqlConnection"), opt =>
                {
                    opt.EnableRetryOnFailure();
                    opt.MigrationsAssembly("Election2023.MigrationsMssql");
                }),

                "Npgsql" =>  options.UseNpgsql(NpgHelper(configuration.GetConnectionString("PostgresConnection")), opt =>
                {
                    opt.EnableRetryOnFailure();
                    opt.SetPostgresVersion(14, 1);
                    opt.MigrationsAssembly("Election2023.MigrationsNpgsql");
                }).UseSnakeCaseNamingConvention(),

                _ => throw new Exception($"Unsupported provider: {provider}")
            });

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ElectionDbContext>();

            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}

