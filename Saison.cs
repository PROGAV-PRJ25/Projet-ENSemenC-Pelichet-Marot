public abstract class Saison
{
    public string NomSaison { get; }
    public float ProbabilitePluie { get; protected set; }
    public float LuminositeMoyenne { get; protected set; }
    public float ProbabiliteIntemperie { get; protected set; }
    public float TemperatureMoyenne { get; protected set; }
    public float VariationTemperature { get; protected set; }

    protected Saison(
        string nomSaison,
        float probaPluie,
        float luminositeMoyenne,
        float probaIntemperie,
        float temperatureMoyenne,
        float variationTemperature
    )
    {
        NomSaison = nomSaison;
        ProbabilitePluie = probaPluie;
        LuminositeMoyenne = luminositeMoyenne;
        ProbabiliteIntemperie = probaIntemperie;
        TemperatureMoyenne = temperatureMoyenne;
        VariationTemperature = variationTemperature;
    }

    public virtual Meteo GenererMeteoDuJour()
    {
        Random rand = new Random();

        float pluieAujourdhui;
        float luminositeAujourdhui;
        bool intemperieAujourdhui;
        float temperature;

        // 1. Déterminer s'il pleut aujourd'hui
        if (rand.NextSingle() < ProbabilitePluie)
        {
            // S'il pleut : pluie = 80% de la luminosité moyenne + aléatoire (0-40%)
            pluieAujourdhui = LuminositeMoyenne * 0.8f + rand.NextSingle() * 0.4f;
        }
        else
        {
            // Sinon : pas de pluie
            pluieAujourdhui = 0f;
        }

        // 2. Calculer la luminosité (90-110% de la moyenne)
        luminositeAujourdhui = LuminositeMoyenne * (0.9f + rand.NextSingle() * 0.2f);

        // 3. Température aléatoire dans la plage saisonnière
        temperature = TemperatureMoyenne + (rand.NextSingle() * 2 - 1) * VariationTemperature;
                         

        // 4. Déterminer s'il y a une intempérie
        intemperieAujourdhui = rand.NextSingle() < ProbabiliteIntemperie;

        

        return new Meteo(pluieAujourdhui, luminositeAujourdhui, temperature, intemperieAujourdhui);
    }
}