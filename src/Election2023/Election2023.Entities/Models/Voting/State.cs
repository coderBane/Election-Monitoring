namespace Election2023.Entities.Models.Voting;

public class State : Entity
{
    public State()
    {
        Districts = new HashSet<District>();
        FederalConstituencies = new HashSet<FederalConstituency>();
        StateConstituencies = new HashSet<StateConstituency>();
        LGAs = new HashSet<LGA>();
        Wards = new HashSet<Ward>();
        PollingUnits = new HashSet<PollingUnit>();
    }

    [Required]
    public StateName Name { get; set; }

    [Required]
    public int RegisteredVoters { get; set; }

    [Required]
    public int EligibleVoters { get; set; }

    [Required]
    public int NoOfPollingUnits { get; set; }

    [Required]
    public int NoOfWards { get; set; }

    [Required]
    public int NoOfLGAs { get; set; }

    [Required]
    public int NoOfFederalConstituencies { get; set; }

    [Required]
    public int NoOfStateConstituencies { get; set; }

    [Required]
    [Range(1, 3)]
    public int NoOfDistricts { get; set; }

    public virtual ICollection<District> Districts { get; }

    public virtual ICollection<FederalConstituency> FederalConstituencies { get; }

    public virtual ICollection<StateConstituency> StateConstituencies { get; set; }

    public virtual ICollection<LGA> LGAs { get; }

    public virtual ICollection<Ward> Wards { get; }

    public virtual ICollection<PollingUnit> PollingUnits { get; }
}

