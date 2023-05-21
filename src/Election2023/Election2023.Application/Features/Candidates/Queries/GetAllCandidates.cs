using Election2023.Application.Specifications.Candidacy;
using Election2023.Application.Interfaces.Repositories;
using Election2023.Application.Interfaces.Services;
using Election2023.Application.ViewModels.Outgoing;
using Election2023.Application.Extensions;

namespace Election2023.Application.Features.Candidates.Queries;

public class GetAllCandidatesQuery : IRequest<Result<List<CandidateSummaryVM>>>
{
    public GetAllCandidatesQuery(string? searchTerm = null, int category = -1)
    {
        SearchTerm = searchTerm;
        Category = category;
    }

    public string? SearchTerm { get; set; }
    public int Category { get; set; }
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
        var specification = new CandidateFilter(request.SearchTerm, false, request.Category, null);
        var candidates = _unitOfWork.Repository<Candidate>().TableNoTracking
            .Specify(specification)
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