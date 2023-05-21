using Election2023.Application.Interfaces.Repositories;
using Election2023.Application.ViewModels.Outgoing;

namespace Election2023.Application.Features.Candidates.Queries;

public class GetCandidatesByIdQuery : IRequest<Result<CandidateProfileVM>>
{
    public string Id { get; set; } = string.Empty;
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
        var candidate = await _unitOfWork.Repository<Candidate>().GetAsync(request.Id);

        if (candidate == null)
            return Result<CandidateProfileVM>.Fail();

        string? logo = _unitOfWork.Repository<PoliticalParty>().TableNoTracking.FirstOrDefault(x => x.Abbrv == candidate.PartyAbbrv)?.Logo;
        
        var mappped = new CandidateProfileVM(candidate.DisplayName, candidate.PartyAbbrv.ToString(), candidate.Age,
            candidate.OneToWatch, candidate.Incumbent, candidate.Image, candidate.Brief, 
            candidate.Category.ToString(), candidate.Education, candidate.ManifestoSnippets, logo);

        return Result<CandidateProfileVM>.Pass(mappped);
    }
}
