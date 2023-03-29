namespace Election2023.Entities.Models.Voting;

public class State : Entity
{
    public State()
    {
        Districts = new HashSet<District>();
        Constituencies = new HashSet<Constituency>();
        LGAs = new HashSet<LocalGovernment>();
        Wards = new HashSet<Ward>();
        PollingUnits = new HashSet<PollingUnit>();
    }

    [Required]
    public StateName Name { get; init; }

    [Required]
    public int RegisteredVoters { get; init; }

    [Required]
    public int EligibleVoters { get; init; }

    [Required]
    public int NoOfPollingUnits { get; init; }

    [Required]
    public int NoOfWards { get; init; }

    [Required]
    public int NoOfLgas { get; init; }

    [Required]
    public int NoOfFedConstituencies { get; set; }

    [Required]
    public int NoOfStateConstituencies { get; set; }

    [Required]
    [Range(1, 3)]
    public int NoOfDistricts { get; init; }

    public virtual ICollection<District> Districts { get; }

    public virtual ICollection<Constituency> Constituencies { get; }

    public virtual ICollection<LocalGovernment> LGAs { get; }

    public virtual ICollection<Ward> Wards { get; }

    public virtual ICollection<PollingUnit> PollingUnits { get; }
}

