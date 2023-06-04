using Election2023.Application.Specifications.Candidacy;
using Election2023.Application.Interfaces.Repositories;
using Election2023.Application.ViewModels.Outgoing;
using Election2023.Application.Features.Base;
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

public class GetAllCandidatesQueryHandler : QueryHandler, IRequestHandler<GetAllCandidatesQuery, Result<List<CandidateSummaryVM>>>
{
    public GetAllCandidatesQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Result<List<CandidateSummaryVM>>> Handle(GetAllCandidatesQuery request, CancellationToken cancellationToken)
    {
        var specification = new CandidateFilter(request.SearchTerm, false, request.Category, null);
        var candidates = await _unitOfWork.Repository<Candidate>().Table
            .Specify(specification)
            .ProjectToType<CandidateSummaryVM>()
            .ToListAsync(cancellationToken);

        return candidates switch
        {
            null => Result<List<CandidateSummaryVM>>.Fail(),
            _ => Result<List<CandidateSummaryVM>>.Pass(candidates)
        };
    }
}