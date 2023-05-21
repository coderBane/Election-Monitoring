namespace Election2023.Application.ViewModels.Incoming;

public record LegislativeInputVM(string State, string Name, string Code, int RegisteredVoters,
    int EligibleVoters, int NoOfWards, int NoOfPollingUnits, int NoOfLGAs) { }

