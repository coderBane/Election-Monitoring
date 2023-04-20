using System.Text.Json;

namespace Election2023.DataServer.Data
{
    public static class ElectionDbContextExtentions
    {
        
        private static List<State> States = new();

        public static void Initialize(this ElectionDbContext context, string path)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            context.SeedStates(path);
        } 

        private static void DbCleanUp(this ElectionDbContext context )
        {

        }

        private static void SeedStates(this ElectionDbContext context, string path)
        {
            if (context.States.Any()) return;

            var json = GetJson(path, "states.json");
            if (json is null)
                return;
            
            var options = new JsonSerializerOptions();
            options.Converters.Add(new StateNameConverter());
            States = JsonSerializer.Deserialize<List<State>>(json, options)!;

            if (States.Any()){
                context.States.AddRange(States);
                context.SaveChanges();
            }
        }

        private static void SeedDistrict(this ElectionDbContext context)
        {
            
        }

        private static string? GetJson(string contentPath, string file)
        {
            int index = contentPath.IndexOf("src");
            string assetsPath = Path.Combine(contentPath.Substring(0,index), "static", "assets", file);
            if (string.IsNullOrEmpty(assetsPath) | !File.Exists(assetsPath))
                return null;
            
            string json = File.ReadAllText(assetsPath);
            if (string.IsNullOrEmpty(json))
                return null;
            
            return json;
        }
    }
}

