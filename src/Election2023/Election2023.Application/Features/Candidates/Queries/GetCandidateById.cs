using Election2023.Application.Specifications.Candidacy;
using Election2023.Application.Interfaces.Repositories;
using Election2023.Application.ViewModels.Outgoing;
using Election2023.Application.Extensions;
using Election2023.Application.Features.Base;

namespace Election2023.Application.Features.Candidates.Queries;

public class GetCandidatesByIdQuery : IRequest<Result<CandidateProfileVM>>
{
    public string Id { get; set; } = string.Empty;

    public bool Include { get; set; }
}

internal class GetCandidatesByIdQueryHandler : QueryHandler, IRequestHandler<GetCandidatesByIdQuery, Result<CandidateProfileVM>>
{
    public GetCandidatesByIdQueryHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<Result<CandidateProfileVM>> Handle(GetCandidatesByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new CandidateFilter(null, request.Include, -1, request.Id);
        var candidate = await _unitOfWork.Repository<Candidate>().Table
            .Specify(specification)
            .ProjectToType<CandidateProfileVM>()
            .FirstOrDefaultAsync(cancellationToken);

        return candidate switch
        {
            null => Result<CandidateProfileVM>.Fail(),
            _ => Result<CandidateProfileVM>.Pass(candidate)
        };
    }
}
