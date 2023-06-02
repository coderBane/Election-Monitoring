using Election2023.Application.Interfaces.Repositories;

namespace Election2023.Application.Features.Base;

public abstract class QueryHandler
{
    protected readonly IUnitOfWork _unitOfWork;

    public QueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}

public abstract class CommandHandler : QueryHandler
{
    protected readonly IMapper _mapper;

    public CommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
    {
        _mapper = mapper;
    }
}
