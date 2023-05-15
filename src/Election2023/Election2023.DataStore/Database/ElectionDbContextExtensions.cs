using System.Text.Json;
using Election2023.Application.VeiwModels.Incoming;

namespace Election2023.DataStore.Database;

public static class ElectionDbContextExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new(){ PropertyNameCaseInsensitive = true };

    private static List<State> States = new();
    private static readonly List<LGA> LGAs = new();
    private static readonly List<District> Districts = new();
    private static readonly List<FederalConstituency> FCs = new();
    private static readonly List<Election> Elections = new();
    private static PoliticalParty[] PoliticalParties = Array.Empty<PoliticalParty>();

    public static void Initialize(this ElectionDbContext context, string path)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        context.SeedParties();

        // context.SeedStates(path);
        // context.SeedDistrict(path);
        // context.SeedConstituencies(path);
        // context.SeedLgas(path);
        context.SaveChanges();
    }

    public static void DbCleanUp(this ElectionDbContext context)
    {
        // context.States.ExecuteDelete();
        context.PoliticalParties.ExecuteDelete();
    }

    public static void SeedParties(this ElectionDbContext context)
    {
        if (context.PoliticalParties.Any()) return;

        Array.Resize(ref PoliticalParties, 10);

        PoliticalParties[0] = new() { Abbrv = Party.A, Name = "Accord", Colour = "SlateGray" };
        PoliticalParties[1] = new() { Abbrv = Party.LP, Name = "Labour Party", Colour = "LimeGreen" };
        PoliticalParties[2] = new() { Abbrv = Party.APC, Name = "All Progressive Congress", Colour = "DeepSkyBlue" };
        PoliticalParties[3] = new() { Abbrv = Party.PDP, Name = "People's Democratic Party", Colour = "Red" };
        PoliticalParties[4] = new() { Abbrv = Party.SDP, Name = "Social Democratic Party", Colour = "Orange" };
        PoliticalParties[5] = new() { Abbrv = Party.NNPP, Name = "New Nigeria People's Party", Colour = "MediumBlue" };
        PoliticalParties[6] = new() { Abbrv = Party.APGA, Name = "All Progressive Grand Allaiance", Colour = "Yellow" };
        PoliticalParties[7] = new() { Abbrv = Party.ADC, Name = "African Democratic Congress", Colour = "DarkGreen" };
        PoliticalParties[8] = new() { Abbrv = Party.ZLP, Name = "Zenith Labour Party", Colour = "Black" };
        PoliticalParties[9] = new() { Abbrv = Party.NRM, Name = "National Rescue Movement", Colour = "Gold" };

        context.PoliticalParties.AddRange(PoliticalParties);
    }

    // private static void SeedStates(this ElectionDbContext context, string path)
    // {
        // if (context.States.Any()) return;

        // var json = GetJson(path, "states.json");
        // if (json is null)
            // return;

        // var options = new JsonSerializerOptions();
        // options.Converters.Add(new StateNameConverter());
        // States = JsonSerializer.Deserialize<List<State>>(json, options)!;

        // if (States.Any())
        // {
            // context.States.AddRange(States);
        // }
    // }

    // private static void SeedDistrict(this ElectionDbContext context, string path)
    // {
        // if (context.Districts.Any()) return;

        // var json = GetJson(path, "districts.json");
        // if (json is null) return;

        // var ser = JsonSerializer.Deserialize<List<LegislativeInputVM>>(json, _jsonOptions);

        // if (ser is null) return;

        // foreach (var ele in ser)
        // {
            // var state = States.FirstOrDefault(x => x.Name == Enum.Parse<StateName>(ele.State));
            // Districts.Add(new()
            // {
                // Name = ele.Name,
                // Code = ele.Code,
                // State = state!,
                // RegisteredVoters = ele.RegisteredVoters,
                // EligibleVoters = ele.EligibleVoters,
                // NoOfLGAs = ele.NoOfLGAs,
                // NoOfPollingUnits = ele.NoOfPollingUnits,
                // NoOfWards = ele.NoOfWards
            // });
        // }

        // if (ser.Count == Districts.Count)
        //     context.Districts.AddRange(Districts);
    // }

    // private static void SeedConstituencies(this ElectionDbContext context, string path)
    // {
        // if (context.FederalConstituencies.Any()) return;

        // var json = GetJson(path, "federal_consts.json");
        // if (json is null) return;

        // var ser = JsonSerializer.Deserialize<List<LegislativeInputVM>>(json, _jsonOptions);

        // if (ser is null) return;

        // foreach (var ele in ser)
        // {
            // var state = ParseState(ele.State);
            // FCs.Add(new()
            // {
                // Name = ele.Name,
                // Code = ele.Code,
                // State = state!,
                // RegisteredVoters = ele.RegisteredVoters,
                // EligibleVoters = ele.EligibleVoters,
                // NoOfLGAs = ele.NoOfLGAs,
                // NoOfPollingUnits = ele.NoOfPollingUnits,
                // NoOfWards = ele.NoOfWards
            // });
        // }

        // if (ser.Count == FCs.Count)
        //     context.FederalConstituencies.AddRange(FCs);
    // }

    // private static void SeedLgas(this ElectionDbContext context, string path)
    // {
        // if (context.LGAs.Any()) return;

        // var json = GetJson(path, "lgas.json");
        // if (json is null) return;

        // var ser = JsonSerializer.Deserialize<List<LGAInputVM>>(json, _jsonOptions);

        // if (ser is null) return;

        // foreach (var ele in ser)
        // {
            // var state = ParseState(ele.State);
            // var district = Districts.FirstOrDefault(x => x.Name == ele.District);
            // LGAs.Add(new()
            // {
                // Name = ele.Name,
                // State = state!,
                // District = district!,
                // RegisteredVoters = ele.RegisteredVoters,
                // EligibleVoters = ele.EligibleVoters,
                // NoOfPollingUnits = ele.NoOfPollingUnits,
                // NoOfWards = ele.NoOfWards,
                // FederalConstituencies = FCs.Where(x => ele.FederalConstituencies.Any(y => y == x.Name)).ToList()
            // }
            // );
        // }

        // if (ser.Count == LGAs.Count)
        //     context.LGAs.AddRange(LGAs);
    // }

    // public static void SeedElection(this ElectionDbContext context)
    // {
        // if (context.Elections.Any()) return;

        // Elections.Add(new() { Type = ElectionType.HouseOfRepresentatives, Date = new(2023, 2, 25) });
        // Elections.Add(new() { Type = ElectionType.Senatorial, Date = new(2023, 2, 25) });
        // Elections.Add(new() { Type = ElectionType.Presidential, Date = new(2023, 2, 25) });
        // Elections.Add(new() { Type = ElectionType.Gubernatorial, Date = new(2023, 3, 18) });

        // context.Elections.AddRange(Elections);
    // }

    // private static State? ParseState(string state)
        // => States.FirstOrDefault(x => x.Name == Enum.Parse<StateName>(state));

    // private static string? GetJson(string contentPath, string file)
    // {
        // int index = contentPath.IndexOf("src");
        // string assetsPath = Path.Combine(contentPath[..index], "static", "assets", file);
        // if (string.IsNullOrEmpty(assetsPath) | !File.Exists(assetsPath))
            // return null;

        // string json = File.ReadAllText(assetsPath);
        // if (string.IsNullOrEmpty(json))
            // return null;

        // return json;
    // }
}
