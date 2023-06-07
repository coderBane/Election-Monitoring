using System.Diagnostics;
using Election2023.Application.Interfaces.Services;

namespace Election2023.Application.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly Stopwatch _timer;
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public PerformanceBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _timer = new Stopwatch();
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();

        var response = await next();

        _timer.Stop();

        var ellapsedMilliseconds = _timer.ElapsedMilliseconds;

        if (ellapsedMilliseconds > 500)
        {
            var name = typeof(TRequest).Name;
            var userId = _currentUserService.UserId;
            _logger.LogWarning("Long running request: {Name} took {ElapsedMilliseconds} ms with {@Request} by {UserId}", 
                name, ellapsedMilliseconds, request, userId);
        }

        return response;
    }
}