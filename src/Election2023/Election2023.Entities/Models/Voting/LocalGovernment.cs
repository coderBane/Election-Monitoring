namespace Election2023.Entities.Models.Voting;

public class LGA : Entity
{
    public LGA()
    {
        Wards = new HashSet<Ward>();
        PollingUnits = new HashSet<PollingUnit>();
        StateConstituencies = new HashSet<StateConstituency>();
        FederalConstituencies = new HashSet<FederalConstituency>();
    }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public int RegisteredVoters { get; set; }

    [Required]
    public int EligibleVoters { get; set; }

    [Required]
    public int NoOfPollingUnits { get; set; }

    [Required]
    public int NoOfWards { get; set; }

    [Required]
    [Column(Order = 1)]
    public int StateId { get; set; }
    public virtual State State { get; set; } = null!;

    [Required]
    public int DistrictId { get; set; }
    public virtual District District { get; set; } = null!;

    public virtual ICollection<FederalConstituency> FederalConstituencies { get; set; }

    public virtual ICollection<StateConstituency> StateConstituencies { get; set; }

    public virtual ICollection<Ward> Wards { get; }

    public virtual ICollection<PollingUnit> PollingUnits{ get; }
}

