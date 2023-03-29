namespace Election2023.Entities.Models.Candidacy;

public sealed class Candidate : Entity
{
    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Firstname { get; set; } = null!;

    [StringLength(50, MinimumLength = 2)]
    public string? Middlename { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Surname { get; set; } = null!;

    [Display(Name = "Name")]
    public string DisplayName => string.Format("{} {}{} {}",
        Title, Firstname, string.IsNullOrEmpty(Middlename) ? "" : " " + Middlename, Surname);

    [Required]
    public Gender Gender { get; set; } = Gender.Unspecified;

    [Required]
    public int Age { get; set; }

    [FileExtensions]
    public string? Image { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Brief { get; set; }

    public int ElectionId { get; set; }
    public Election Election { get; set; } = null!;

    public int PoliticalPartyId { get; set; }
    public PoliticalParty Party { get; set; } = null!;
}

