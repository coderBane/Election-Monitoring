using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Election2023.ServerApp.Areas.Identity;

using Election2023.DataServer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region npgsql_datasoure_configuration
var dataSourceBuilder = new Npgsql.NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("PostgresConnection"));
ElectionDbContext.NpgMappings(ref dataSourceBuilder);
var dataSource = dataSourceBuilder.Build();
#endregion

var provider = builder.Configuration.GetValue("Provider", "Sqlite");

builder.Services.AddDbContextFactory<ElectionDbContext>(options => _ = provider switch
{
    "Sqlite" => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"), opt =>
    {
        opt.MigrationsAssembly("Election2023.MigrationsSqlite");
    }),

    "Mssql" => options.UseSqlServer(builder.Configuration.GetConnectionString("MssqlConnection"), opt => 
    {
        opt.EnableRetryOnFailure();
        opt.MigrationsAssembly("Election2023.MigrationsMssql");
    }),

    "Npgsql" => options.UseNpgsql(dataSource, opt =>
    {
        opt.EnableRetryOnFailure();
        opt.SetPostgresVersion(14, 1);
        opt.MigrationsAssembly("Election2023.MigrationsNpgsql");
    }).UseSnakeCaseNamingConvention(),

    _ => throw new Exception($"Unsupported provider: {provider}")
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ElectionDbContext>();
    
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
//builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
