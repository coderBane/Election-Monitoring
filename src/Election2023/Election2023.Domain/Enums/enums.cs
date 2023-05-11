namespace Election2023.Domain.Enums
{
    public enum StateName
    {
        Abia = 1, Adamawa, AkwaIbom, Anambra, Bauchi, Bayelsa, Benue, Borno, CrossRiver, Delta,
        Ebonyi, Edo, Ekiti, Enugu, Gombe, Imo, Jigawa, Kaduna, Kano, Katsina, Kebbi, Kogi, Kwara, 
        Lagos, Nasarawa, Niger, Ogun, Ondo, Osun, Oyo, Plateau, Rivers, Sokoto, Taraba, Yobe, Zamfara, FCT
    }

    public enum ElectionType { Presidential = 1, Gubernatorial, Senatorial, HouseOfRepresentatives, HouseOfAssembly }

    public enum Party { A, AA, AAC, ADC, ADP, APC, APM, APP, APGA, BP, LP, NNPP, NRM, PDP, PRP, SDP, YPP, ZLP }

    public enum Gender { Male, Female, Unspecified }
}

