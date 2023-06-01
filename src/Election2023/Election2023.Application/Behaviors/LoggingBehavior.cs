using Election2023.Application.Interfaces.Services;

namespace Election2023.Application.Behaviors;

public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehavior(ILogger<TRequest> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }

    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var name = nameof(request);
        var userId = _currentUserService.UserId;
        _logger.LogTrace("Request: {Name} with {@Request} by {UserId}", name, request, userId);
        
        return Task.CompletedTask;
    }
}