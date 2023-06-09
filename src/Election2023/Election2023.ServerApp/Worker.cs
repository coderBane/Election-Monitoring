using Election2023.DataStore.Database;
using Microsoft.EntityFrameworkCore;

namespace Election2023.ServerApp;

internal class Worker : IHostedService, IDisposable
{
    private Timer? _timer;

    private readonly string _contentPath;

    private readonly ILogger<Worker> _logger;

    private readonly IServiceProvider _services;

    private readonly IConfiguration _configuration;

    public Worker(IServiceProvider services, IWebHostEnvironment env, IConfiguration configuration, ILogger<Worker> logger)
        => (_services, _logger, _contentPath, _configuration) = (services, logger, env.ContentRootPath, configuration);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Starting worker service...");
        _timer = new(state => Execute(), null, TimeSpan.Zero, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private void Execute()
    {
        if (_configuration.GetValue<bool>("Seed"))
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ElectionDbContext>();

            _logger.LogDebug("Create clean slate for database");
            dbContext.DbCleanUp();

            var executionStrategy = dbContext.Database.CreateExecutionStrategy();

            executionStrategy.Execute(() =>
            {
                using var tx = dbContext.Database.BeginTransaction();
                _logger.LogDebug("Started an explicit database transaction id: {transId}", tx.TransactionId);

                _logger.LogDebug("Seeding database...");
                dbContext.Initialize(_contentPath);
                _logger.LogDebug("Database seeded successfully!");

                _logger.LogDebug("Committing transaction...");
                tx.Commit();
                _logger.LogDebug("Database transaction '{transId}' completed successfully", tx.TransactionId);
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogDebug("Stopping worker service...");
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
