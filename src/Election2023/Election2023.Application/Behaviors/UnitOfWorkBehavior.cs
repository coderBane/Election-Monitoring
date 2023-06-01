using System.Transactions;
using System.Windows.Input;
using Election2023.Application.Interfaces.Repositories;

namespace Election2023.Application.Behaviors;

public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICommand
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var transaction = new TransactionScope(
            TransactionScopeOption.Required, 
            new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted }, 
            TransactionScopeAsyncFlowOption.Enabled
        );

        var response = await next();

        await _unitOfWork.CommitAsync();

        transaction.Complete();

        return response;
    }
}