// using Election2023.DataStore.Database;

namespace Election2023.ServerApp;

internal class Worker : IHostedService, IDisposable
{
    private Timer? _timer;

    private readonly IServiceProvider _services;

    public Worker(IServiceProvider services) => _services = services;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new(state => Execute(), null, TimeSpan.Zero, TimeSpan.FromDays(1));

        return Task.CompletedTask;
    }

    private void Execute()
    {
        // using var scope = _services.CreateScope();
        // var dbContext = scope.ServiceProvider.GetRequiredService<ElectionDbContext>();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
