namespace Election2023.Entities.Models.Voting;

public class LocalGovernment : Entity
{
    public LocalGovernment()
    {
        Wards = new HashSet<Ward>();
        PollingUnits = new HashSet<PollingUnit>();
        LocalGovernmentConstituencies = new HashSet<LocalGovernmentConstituency>();
    }

    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public int RegisteredVoters { get; init; }

    [Required]
    public int EligibleVoters { get; init; }

    [Required]
    public int NoOfPollingUnits { get; init; }

    [Required]
    public int NoOfWards { get; init; }

    [Required]
    [Column(Order = 1)]
    public int StateId { get; set; }
    public virtual State State { get; set; } = null!;

    [Required]
    public int DistrictId { get; set; }
    public virtual District District { get; set; } = null!;

    public virtual ICollection<LocalGovernmentConstituency>  LocalGovernmentConstituencies { get; }

    public virtual ICollection<Ward> Wards { get; }

    public virtual ICollection<PollingUnit> PollingUnits{ get; }
}

public sealed class LocalGovernmentConstituency
{
    public int LocalGovernmentId { get; set; }
    public LocalGovernment LocalGovernment { get; set; } = null!;

    public int ConstituencyId { get; set; }
    public Constituency Constituency { get; set; } = null!;
}

