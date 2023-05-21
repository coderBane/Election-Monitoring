namespace Election2023.Application.ViewModels.Outgoing;

public record CandidateIdVM(string Id);

public record CandidateSummaryVM(string Id, string DisplayName, string Party, int Age,
    bool OneToWatch, bool Incumbent, string? Image);

public record CandidateProfileVM(string DisplayName, string Party, int Age, 
    bool OneToWatch, bool Incumbent, string? Image, string? Brief, 
    string Category, string Education, string[] Manifesto, string? PartyLogo, string PartyName); 
