using Election2023.Domain.Common;
using Election2023.Domain.Enums;

namespace Election2023.Domain.Models.Candidacy;

public sealed class PoliticalParty : Entity<int>
{
    [MaxLength(128)]
    public string Name { get; init; } = null!;

    [Required]
    public Party Abbrv { get; init; }

    public string? Logo { get; set; }

    [Required]
    public string Colour { get; set; } = null!;

    public ICollection<Candidate> Candidates { get; } = new HashSet<Candidate>();

    public override bool Equals(object? obj) 
        => ReferenceEquals(this, obj) || 
            obj is PoliticalParty other && (other.Abbrv == Abbrv || other.Name == Name);

    public override int GetHashCode() => HashCode.Combine(Abbrv, Name);
}

