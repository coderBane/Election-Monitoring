namespace Election2023.Application.Exceptions.Handlers;

public class ServerExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TException : ServerException
{
    private readonly ILogger _logger;

    public ServerExceptionHandler(ILogger<ServerExceptionHandler<TRequest, TResponse, TException>> logger)
    {
        _logger = logger;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        var response = Activator.CreateInstance<TResponse>();
        if (response is Result result)
        {
            result.Success = false;
            result.Messages = new string[] { exception.Message };
            state.SetHandled(response);
        }

        return Task.CompletedTask;
    }
}