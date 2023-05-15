using Election2023.Domain.Enums;
using Election2023.Domain.Common;

namespace Election2023.Domain.Models.Candidacy;

public sealed class Candidate : AuditableEntity<string>
{
    public Candidate() => Id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0,8);

    [Required]
    public ElectionType Category {get; set;}

    public bool OneToWatch { get; set; }

    public bool Incumbent { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Firstname { get; set; } = null!;

    [StringLength(50, MinimumLength = 2)]
    public string? Middlename { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Surname { get; set; } = null!;

    [Required]
    public Gender Gender { get; set; } = Gender.Unspecified;

    [Required]
    [Range(30, 85)]
    public int Age { get; set; }

    [Required]
    public string Education { get; set; } = null!;

    [FileExtensions]
    public string? Image { get; set; }

    [DataType(DataType.MultilineText)]
    public string? Brief { get; set; }

    public string? Constituency { get; set; }

    public string? State { get; set; }

    [Required]
    [Column(Order = 2)]
    public Party PartyAbbrv { get; set; }
    public PoliticalParty Party { get; set; } = null!;

    public string[] ManifestoSnippets { get; set; } = Array.Empty<string>();

    public string DisplayName => string.Format("{2} {0} {1}",
        Firstname, string.IsNullOrEmpty(Middlename) ? "" : Middlename, Surname);

    public override bool Equals(object? obj) 
        => object.ReferenceEquals(this, obj) ||
            obj is Candidate other && 
            (other.PartyAbbrv == this.PartyAbbrv && other.Category == this.Category &&
            other.State == this.State && other.Constituency == this.Constituency);

    public override int GetHashCode() => HashCode.Combine(PartyAbbrv, Category, State, Constituency);
}

