using Election2023.Entities.Models.Voting;

namespace Election2023.Entities.Models.Candidacy;

public sealed class Election : Entity
{
    public ElectionType Category { get; set; }

    public DateOnly Date { get; set; }

    public ICollection<Candidate> Candidates { get; set; } = new HashSet<Candidate>();
}

// public sealed record ElectionResult
// {
//    [Column(Order = 0)]
//    public int ElectionId { get; init; }
//    public Election Election { get; init; } = null!;

//    [Column(Order = 1)]
//    public int PoliticalPartyId { get; init; }
//    public PoliticalParty PoliticalParty { get; init; } = null!;

//    public int count { get; init; }
// }