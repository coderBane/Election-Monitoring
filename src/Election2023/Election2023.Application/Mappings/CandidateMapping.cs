using Election2023.Application.ViewModels.Outgoing;

namespace Election2023.Application.Mappings;

public class CandidateMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Candidate, CandidateSummaryVM>()
            .Map(dest => dest.Party, src => src.PartyAbbrv);

        config.NewConfig<Candidate, CandidateProfileVM>()
            .Map(dest => dest.Party, src => src.PartyAbbrv)
            .Map(dest => dest.Manifesto, src => src.ManifestoSnippets);
    }
}