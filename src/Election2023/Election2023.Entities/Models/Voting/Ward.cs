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
    public int NoPollingUnits { get; set; }

    [Required]
    [Column(Order = 1)]
    public int StateId { get; set; }
    public virtual State State { get; set; } = null!;

    [Required]
    [Column("LGA", Order = 2)]
    public int LocalGovernmentId { get; set; }
    public virtual LocalGovernment LocalGovernment { get; set; } = null!;

    [Required]
    public int DistrictId { get; set; }
    public virtual District District { get; set; } = null!;

    [Required]
    public int ConstituencyId { get; set; }
    public virtual Constituency Constituency { get; set; } = null!;

    public virtual ICollection<PollingUnit> PollingUnits { get; } = new HashSet<PollingUnit>();
}