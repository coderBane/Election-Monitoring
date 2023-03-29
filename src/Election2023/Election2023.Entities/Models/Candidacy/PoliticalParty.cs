namespace Election2023.Entities.Models.Candidacy;

public sealed class PoliticalParty : Entity
{
    [MaxLength(128)]
    public string Name { get; init; } = null!;

    [Required]
    public Party Abbrv { get; init; }

    public string? Logo { get; init; }

    [Required]
    public string Colour { get; set; } = null!;

    public ICollection<Candidate> Candidates { get; set; } = new HashSet<Candidate>();
}

