namespace Election2023.Entities.Models.Voting;

public abstract class Legislative : Entity
{
    [Required]
    public string Name { get; set; } = null!;

    [Required]
    [Column(Order = 1)]
    public int StateId { get; set; }
    public virtual State State { get; set; } = null!;

    public int RegisteredVoters => PollingUnits.Sum(pu => pu.RegisteredVoters);

    public int EligibleVoters => PollingUnits.Sum(pu => pu.EligibleVoters);

    public virtual ICollection<Ward> Wards { get; } = new HashSet<Ward>();

    public virtual ICollection<PollingUnit> PollingUnits { get; } = new HashSet<PollingUnit>();
}

public sealed class District : Legislative
{
   public ICollection<LocalGovernment> LGAs { get; } = new HashSet<LocalGovernment>();
}

public sealed class Constituency : Legislative
{
    public bool IsFed {get; set;} = true;

    public ICollection<LocalGovernmentConstituency> LocalGovernmentConstituencies { get; } = new HashSet<LocalGovernmentConstituency>();
}