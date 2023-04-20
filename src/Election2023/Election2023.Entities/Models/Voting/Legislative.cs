namespace Election2023.Entities.Models.Voting;

public abstract class Legislative : Entity
{
    [Required]
    public string Name { get; set; } = null!;

    [RegularExpression(@"^\w{2}/\d{3}/\w{2}$", ErrorMessage = "The format should be XX-XXX-XX.")]
    public string Code { get; set; } = null!;

    [Required]
    [Column(Order = 1)]
    public int StateId { get; set; }
    public virtual State State { get; set; } = null!;

    public int RegisteredVoters { get; set; }

    public int EligibleVoters { get; set;}

    public int NoOfPollingUnits { get; set; }

    public int NoOfWards { get; set; }

    public int NoOfLGAs { get; set; }

    public ICollection<LGA> LGAs { get; } = new HashSet<LGA>();

    public virtual ICollection<Ward> Wards { get; } = new HashSet<Ward>();

    public virtual ICollection<PollingUnit> PollingUnits { get; } = new HashSet<PollingUnit>();
}

public sealed class District : Legislative
{

}

public abstract class Constituency : Legislative
{
    public bool IsFed { get; set; } 
}

public sealed class FederalConstituency : Constituency
{
   public FederalConstituency() => IsFed = true;
}

public sealed class StateConstituency : Constituency
{

}

