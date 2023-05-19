using System.Text.Json;
using Election2023.DataStore.Converters.JsonConverters;
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
    private static List<Candidate> Candidates = new();
    private static PoliticalParty[] PoliticalParties = Array.Empty<PoliticalParty>();

    public static void Initialize(this ElectionDbContext context, string path)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        context.SeedParties();
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

    private static void SeedParties(this ElectionDbContext context)
    {
        if (context.PoliticalParties.Any()) return;

        Array.Resize(ref PoliticalParties, 18);

        PoliticalParties[0] = new() { Abbrv = Party.A, Name = "Accord", Colour = "SlateGray", Logo = "https://pbs.twimg.com/media/FrMlXGeaEAUdi_B.jpg" };
        PoliticalParties[1] = new() { Abbrv = Party.AA, Name = "Action Alliance", Colour = "CornflowerBlue", Logo = "https://inecnigeria.org/wp-content/uploads/2018/10/AA.jpg" };
        PoliticalParties[2] = new() { Abbrv = Party.AAC, Name = "African Action Congress", Colour = "Cornsilk", Logo = "https://pbs.twimg.com/profile_images/1085647150619406341/-fR2lwGA_400x400.jpg" };
        PoliticalParties[3] = new() { Abbrv = Party.ADC, Name = "African Democratic Congress", Colour = "DarkGreen", Logo = "https://leadership.ng/wp-content/uploads/2022/08/adc-1.jpg" };
        PoliticalParties[4] = new() { Abbrv = Party.ADP, Name = "Action Democratic Party", Colour = "Navy", Logo = "https://media.premiumtimesng.com/wp-content/files/2017/10/Action-Democratic-Party-logo.jpg" };
        PoliticalParties[5] = new() { Abbrv = Party.APC, Name = "All Progressive Congress", Colour = "DeepSkyBlue", Logo = "https://pbs.twimg.com/profile_images/983732871713214466/BzzWsk_i_400x400.jpg" };
        PoliticalParties[6] = new() { Abbrv = Party.APGA, Name = "All Progressive Grand Allaiance", Colour = "Yellow", Logo = "https://upload.wikimedia.org/wikipedia/en/b/b3/APGA_Nigeria_Logo.png" };
        PoliticalParties[7] = new() { Abbrv = Party.APM, Name = "Allied Peoples Movement", Colour = "MediumPurple", Logo = "https://media.premiumtimesng.com/wp-content/files/2022/04/Capture-3.png" };
        PoliticalParties[8] = new() { Abbrv = Party.APP, Name = "Action Peoples Party", Colour = "Tomato" };
        PoliticalParties[9] = new() { Abbrv = Party.BP, Name = "Boot Party", Colour = "LightGreen", Logo = "" };
        PoliticalParties[10] = new() { Abbrv = Party.LP, Name = "Labour Party", Colour = "LimeGreen", Logo = "https://upload.wikimedia.org/wikipedia/en/5/5f/Labour_Party_%28Nigeria%29_logo.png" };
        PoliticalParties[11] = new() { Abbrv = Party.NNPP, Name = "New Nigeria People's Party", Colour = "MediumBlue", Logo = "https://pbs.twimg.com/profile_images/1526600560400556033/0KiCFafJ_400x400.jpg" };
        PoliticalParties[12] = new() { Abbrv = Party.NRM, Name = "National Rescue Movement", Colour = "Gold", Logo = "https://www.nrm.org.ng/images/logo2.fw.png" };
        PoliticalParties[13] = new() { Abbrv = Party.PDP, Name = "People's Democratic Party", Colour = "Red", Logo = "https://upload.wikimedia.org/wikipedia/en/6/62/Logo_of_the_Peoples_Democratic_Party_%28Nigeria%29.png"};
        PoliticalParties[14] = new() { Abbrv = Party.PRP, Name = "People's Redemption Party", Colour = "FireBrick", Logo = "https://flashpointnews.files.wordpress.com/2022/07/fb_img_1657657966479.jpg" };
        PoliticalParties[15] = new() { Abbrv = Party.SDP, Name = "Social Democratic Party", Colour = "Orange", Logo = "https://media.premiumtimesng.com/wp-content/files/2018/03/Social-Democratic-Party.png" };
        PoliticalParties[16] = new() { Abbrv = Party.YPP, Name = "Young Progressive Party", Colour = "Goldenrod", Logo = "https://ypp.ng/wp-content/uploads/2020/12/cropped-ypp-logo.png" };
        PoliticalParties[17] = new() { Abbrv = Party.ZLP, Name = "Zenith Labour Party", Colour = "Black", Logo = "https://149520306.v2.pressablecdn.com/wp-content/uploads/2022/08/Zenith-Labour-Party-ZLP-1.png" };

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
            Candidates = JsonSerializer.Deserialize<List<Candidate>>(json, options) ?? new List<Candidate>();
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

