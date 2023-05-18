using Election2023.Application.Interfaces.Repositories;
using Election2023.Application.Interfaces.Services;
using Election2023.Application.VeiwModels.Outgoing;

namespace Election2023.Application.Features.Candidates.Queries;

public class GetAllCandidatesQuery : IRequest<Result<List<CandidateSummaryVM>>>
{
    public GetAllCandidatesQuery(int searchTerm)
    {
        SearchTerm = searchTerm;
    }

    public int SearchTerm { get; set; }
}

public class GetAllCandidatesQueryHandler : IRequestHandler<GetAllCandidatesQuery, Result<List<CandidateSummaryVM>>>
{
    private readonly IUnitOfWork _unitOfWork;

    private readonly ICurrentUserService _currentUserService;

    public GetAllCandidatesQueryHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public Task<Result<List<CandidateSummaryVM>>> Handle(GetAllCandidatesQuery request, CancellationToken cancellationToken)
    {
        var candidates = _unitOfWork.Repository<Candidate>().TableNoTracking
            .Where(e => (int)e.Category == request.SearchTerm)
            .Select(c => new CandidateSummaryVM
            (c.Id,
            c.DisplayName, 
            c.PartyAbbrv.ToString(),
            c.Age, 
            c.OneToWatch, 
            c.Incumbent, 
            c.Image)).ToList();

        return Task.FromResult(Result<List<CandidateSummaryVM>>.Pass(candidates));
    }
}