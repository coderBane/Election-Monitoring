namespace Election2023.Entities.Models
{
    public enum StateName
    {
        Abia = 1, Adamawa, AkwaIbom, Anambra, Bauchi, Bayelsa, Benue, Borno, CrossRiver, Delta,
        Ebonyi, Edo, Ekiti, Enugu, FCT, Gombe, Imo, Jigawa, Kaduna, Kano, Kastina, Kebbi, Kogi,
        Kwara, Lagos, Nasarawa, Niger, Ogun, Ondo, Osun, Oyo, Plateau, Rivers, Sokoto, Taraba, Yobe, Zamfara
    }

    public enum ElectionType { Presidential, Gubernatorial, Senatorial, HOR, StateHouseAssembly }

    public enum Party { A, AA, ADC, ADP, APC, APGA, LP, NNPP, PDP, SDP, YPP, ZLP }

    public enum Gender { Male, Female, Unspecified }
}

