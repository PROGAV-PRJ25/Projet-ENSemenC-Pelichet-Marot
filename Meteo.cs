public class Meteo
{
    public float QuantitePluie { get; }
    public float Luminosite { get; }
    public float Temperature { get; }
    public bool Intemperie { get; }
    public Saison SaisonActuelle { get; }
    public string Description { get; }

    private Meteo(float pluie, float luminosite, float temperature, bool intemperie, Saison saison)
    {
        QuantitePluie = Math.Clamp(pluie, 0, 1);
        Luminosite = Math.Clamp(luminosite, 0.1f, 1);
        Temperature = temperature;
        Intemperie = intemperie;
        SaisonActuelle = saison;
        Description = GenererDescription(); // Initialise la description ici
        
    }


    public static Meteo GenererPourSaison(Saison saison)
    {
        var rand = new Random();

        float pluie = CalculerPluie(saison, rand);
        float luminosite = CalculerLuminosite(saison, rand);
        float temperature = CalculerTemperature(saison, rand);
        bool intemperie = DeterminerIntemperie(saison, rand);

        return new Meteo(pluie, luminosite, temperature, intemperie, saison);
    }

    private static float CalculerPluie(Saison saison, Random rand)
    {
        return rand.NextSingle() < saison.ProbabilitePluie
            ? saison.LuminositeMoyenne * 0.8f + rand.NextSingle() * 0.4f
            : 0f;
    }

    private static float CalculerLuminosite(Saison saison, Random rand)
    {
        return saison.LuminositeMoyenne * (0.9f + rand.NextSingle() * 0.2f);
    }

    private static float CalculerTemperature(Saison saison, Random rand)
    {
        return saison.TemperatureMoyenne + (rand.NextSingle() * 2 - 1) * saison.VariationTemperature;
    }

    private static bool DeterminerIntemperie(Saison saison, Random rand)
    {
        return rand.NextSingle() < saison.ProbabiliteIntemperie;
    }

    private string GenererDescription()
    {
        string desc = $"Météo : saison {SaisonActuelle.NomSaison}, ";
        desc += $"Température : {Temperature:F1}°C, ";
        desc += $"Pluie : {QuantitePluie:P0}, ";
        desc += $"Luminosité : {Luminosite:P0}";

        if (Intemperie)
        {
            desc += ", Orage ⚡";
        }

        return desc;
    }
}