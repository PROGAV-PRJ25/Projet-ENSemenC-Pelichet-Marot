public class Meteo
{
    public float QuantitePluie { get; }
    public float Luminosite { get; }
    public float Temperature { get; }
    public bool Intemperie { get; }
    public Saison SaisonActuelle { get; }
    public string Description { get; }

    private Meteo(float pluie, float luminosite, float temperature, bool intemperie, Saison saison, int jourActuel)
    {
        //'QuantitePluie' vaut 'pluie' sauf si ce dernier est plus petit que 0 ou plus grand que 1. 
        //Dans ces cas il vaut respectivement 0 ou 1.
        QuantitePluie = Math.Clamp(pluie, 0, 1);
        Luminosite = Math.Clamp(luminosite, 0.1f, 1);
        Temperature = temperature;
        Intemperie = intemperie;
        SaisonActuelle = saison;
        Description = GenererDescription(jourActuel);  // Initialise la description ici
    }

    public static Meteo GenererPourSaison(Saison saison, int jourActuel)
    {
        //On génère un unique random pour tous les calcules réalisés. De cette façon, plus rand est grand, 
        //plus il fait chaud, sec et lumineux. Il n'y a pas une énorme pluie et une luminosité énorme en même temps.
        var rand = new Random();

        float pluie = CalculerPluie(saison, rand);
        float luminosite = CalculerLuminosite(saison, rand);
        float temperature = CalculerTemperature(saison, rand);
        bool intemperie = DeterminerIntemperie(saison, rand);
        if(pluie != 0f)
        {
            luminosite -= 0.2f;
            temperature -= 5f;
        }

        return new Meteo(pluie, luminosite, temperature, intemperie, saison, jourActuel);
    }

    //La fonction CalculerPluie compare un nombre aléatoire entre 0 et 1. Si ce dernier est compris entre 0
    //et la ProbabilitePluie de la saison correspondante, alors il pleut (sinon return 0f) et cette pluie est 
    //légèrement randomisée (entre 80% et 120% de la probalité classique de la saison).
    private static float CalculerPluie(Saison saison, Random rand)
    {
        return rand.NextSingle() < saison.ProbabilitePluie
            ? saison.ProbabilitePluie * 0.8f + rand.NextSingle() * 0.4f
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

    private string GenererDescription(int jourActuel)
    {
        //Les F1 et P0 permettent de limiter le nombre de chiffres après la virgule.
        string desc = $"Jour : {jourActuel}";
        desc += $"\nMétéo : saison {SaisonActuelle.NomSaison}, ";
        desc += $"Température : {Temperature:F1}°C, ";
        desc += $"Pluie : {QuantitePluie:P0}, ";
        desc += $"Luminosité : {Luminosite:P0}";

        //Durant les 14 premiers jours, le joueur ne peut pas recevoir d'orage ou de grêle sur ses plantations.
        if (jourActuel > 14 && Intemperie)
        {
            Random rand = new Random();
            int nombre = rand.Next(0, 2);
            if(nombre == 0)
            {
                desc += ", Orage ⚡";
            }
            else
            {
                desc += ", Grêle 🧊";
            }
        }

        return desc;
    }
}