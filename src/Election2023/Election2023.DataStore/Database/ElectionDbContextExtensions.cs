using System.Text.Json;
using Election2023.DataStore.Converters.JsonConverters;

namespace Election2023.DataStore.Database;

public static class ElectionDbContextExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new(){ PropertyNameCaseInsensitive = true };

    // private static List<State> States = new();
    // private static readonly List<LGA> LGAs = new();
    // private static readonly List<District> Districts = new();
    // private static readonly List<FederalConstituency> FCs = new();
    // private static readonly List<Election> Elections = new();
    private static List<Candidate> Candidates = new();
    private static List<PoliticalParty> PoliticalParties = new();

    public static void Initialize(this ElectionDbContext context, string path)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        context.SeedParties(path);
        context.SeedCandidates(path);
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

    private static void SeedParties(this ElectionDbContext context, string path)
    {
        if (context.PoliticalParties.Any()) return;

        var json = GetJson(path, "parties.json");
        if (json is null) return;

        PoliticalParties = JsonSerializer.Deserialize<List<PoliticalParty>>(json) ?? PoliticalParties;

        if (PoliticalParties.Any())
            context.PoliticalParties.AddRange(PoliticalParties);

        context.PoliticalParties.AddRange(PoliticalParties);
    }

    private static void SeedCandidates(this ElectionDbContext context, string path)
    {
        var json = GetJson(path, "candidates.json");
        if (json == null) return;

        var options = new JsonSerializerOptions();
        options.Converters.Add(new StringEnumJsonConverter<Party>());
        options.Converters.Add(new StringEnumJsonConverter<Gender>());
        options.Converters.Add(new StringEnumJsonConverter<ElectionType>());

        if(!context.Candidates.Any(x => x.Category == ElectionType.Presidential))
        {
            Candidates = JsonSerializer.Deserialize<List<Candidate>>(json, options) ?? Candidates;
            if (Candidates.Any())
            {
                context.Candidates.AddRange(Candidates);
                Candidates.Clear();
            }
        }
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

    private static string? GetJson(string contentPath, string file)
    {
        int index = contentPath.IndexOf("src");
        string assetsPath = Path.Combine(contentPath[..index], "static", "assets", file);
        if (string.IsNullOrEmpty(assetsPath) | !File.Exists(assetsPath))
            return null;

        string json = File.ReadAllText(assetsPath);
        if (string.IsNullOrEmpty(json))
            return null;

        return json;
    }
}

