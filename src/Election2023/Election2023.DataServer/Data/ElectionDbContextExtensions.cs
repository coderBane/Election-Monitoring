namespace Election2023.DataServer.Data
{
    public static class ElectionDbContextExtentions
    {
        private static State[] States = Array.Empty<State>();

        private static void DbCleanUp(this ElectionDbContext context )
        {

        }

        private static void SeedStates(this ElectionDbContext context)
        {
            if (context.States.Any()) return;
            
            int district = 3;
            States = new State[]
            {
                new State { Name = StateName.Abia, RegisteredVoters = 2120808, EligibleVoters = 1949197, 
                    NoOfPollingUnits = 4062, NoOfWards = 184, NoOfLgas = 17, NoOfFedConstituencies = 8, 
                    NoOfDistricts = district, NoOfStateConstituencies = 24},
                new State { Name = StateName.Adamawa, RegisteredVoters = 2196566, EligibleVoters = 1970650, 
                    NoOfPollingUnits = 4104, NoOfWards = 226, NoOfLgas = 21, NoOfFedConstituencies = 8, 
                    NoOfDistricts = district, NoOfStateConstituencies = 24},
            };
        }

        private static void SeedDistrict(this ElectionDbContext context)
        {
            
        }
    }
}

