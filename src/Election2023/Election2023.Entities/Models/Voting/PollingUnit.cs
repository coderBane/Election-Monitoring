using Election2023.Entities.Models.Candidacy;

namespace Election2023.Entities.Models.Voting;

public class PollingUnit : Entity
{
    [Required]
    public string Address { get; set; } = null!;

    [RegularExpression(@"^\d{2}-\d{2}-\d{2}-\d{3}$", ErrorMessage = "The format should be XX-XX-XX-XXX.")]
    public string TerritoryCode { get; set; } = null!;

    [Required]
    public int RegisteredVoters { get; init; }

    [Required]
    public int EligibleVoters { get; init; }

    [Required]
    [Column(Order = 1)]
    public int StateId { get; set; }
    public State State { get; set; } = null!;

    [Required]
    [Column(Order = 2)]
    public int LGAId { get; set; }
    public LGA LGA { get; set; } = null!;

    [Required]
    [Column(Order = 3)]
    public int WardId { get; set; }
    public Ward Ward { get; set; } = null!;

    [Required]
    public int DistrictId { get; set; }
    public virtual District District { get; set; } = null!;

    public int FederalConstituencyId { get; set; }
    public virtual FederalConstituency FederalConstituency { get; set; } = null!;

    public int StateConstituencyId { get; set; }
    public virtual FederalConstituency StateConstituency { get; set; } = null!;
}
