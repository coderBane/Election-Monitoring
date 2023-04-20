namespace Election2023.Entities.Models.Voting;

public class Ward : Entity
{
    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public int RegisteredVoters { get; init; }

    [Required]
    public int EligibleVoters { get; init; }

    [Required]
    public int NoOfPollingUnits { get; set; }

    [Required]
    [Column(Order = 1)]
    public int StateId { get; set; }
    public virtual State State { get; set; } = null!;

    [Required]
    [Column(Order = 2)]
    public int LGAId { get; set; }
    public virtual LGA LGA { get; set; } = null!;

    [Required]
    public int DistrictId { get; set; }
    public virtual District District { get; set; } = null!;

    public int FederalConstituencyId { get; set; }
    public virtual FederalConstituency FederalConstituency { get; set; } = null!;

    public int StateConstituencyId { get; set; }
    public virtual StateConstituency StateConstituency { get; set; } = null!;

    public virtual ICollection<PollingUnit> PollingUnits { get; } = new HashSet<PollingUnit>();
}