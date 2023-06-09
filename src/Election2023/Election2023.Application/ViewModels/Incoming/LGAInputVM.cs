﻿namespace Election2023.Application.ViewModels.Incoming;

public record LGAInputVM(string State, string Name, string District, int RegisteredVoters, int EligibleVoters,
    int NoOfWards, int NoOfPollingUnits, IEnumerable<string> FederalConstituencies) { }

