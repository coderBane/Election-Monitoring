using Election2023.Application.Specifications.Candidacy;
using Election2023.Application.Interfaces.Repositories;
using Election2023.Application.ViewModels.Outgoing;
using Election2023.Application.Extensions;

namespace Election2023.Application.Features.Candidates.Queries;

public class GetCandidatesByIdQuery : IRequest<Result<CandidateProfileVM>>
{
    public string Id { get; set; } = string.Empty;

    public bool Include { get; set; }
}

internal class GetCandidatesByIdQueryHandler : IRequestHandler<GetCandidatesByIdQuery, Result<CandidateProfileVM>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCandidatesByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CandidateProfileVM>> Handle(GetCandidatesByIdQuery request, CancellationToken cancellationToken)
    {
        var specification = new CandidateFilter(null, request.Include, -1, request.Id);
        var candidate = await _unitOfWork.Repository<Candidate>().TableNoTracking
            .Specify(specification)
            .Select(x => new CandidateProfileVM(x.DisplayName, x.PartyAbbrv.ToString(), x.Age,
                x.OneToWatch, x.Incumbent, x.Image, x.Brief, x.Category.ToString(), x.Education, 
                x.ManifestoSnippets, x.Party.Logo, x.Party.Name)
            ).FirstOrDefaultAsync();

        return candidate switch
        {
            null => Result<CandidateProfileVM>.Fail(),
            _ => Result<CandidateProfileVM>.Pass(candidate)
        };
    }
}
