public class Meteo
{
    private static readonly Random _rng = new Random();

    public float QuantitePluie { get; }
    public int Luminosite { get; }    // 1 (nul) à 5 (très fort)
    public float Temperature { get; }
    public bool Intemperie { get; }
    public Saison SaisonActuelle { get; }
    public string Description { get; }

    private Meteo(
        float pluie,
        int luminosite,
        float temperature,
        bool intemperie,
        Saison saison,
        int semaineActuelle
    )
    {
        QuantitePluie = Math.Clamp(pluie, 0f, 1f);
        Luminosite = luminosite;           // déjà dans [1..5]
        Temperature = temperature;
        Intemperie = intemperie;
        SaisonActuelle = saison;
        Description = GenererDescription(semaineActuelle);
    }

    public static Meteo GenererPourSaison(Saison saison, int semaineActuelle)
    {
        // Pluie et température comme avant
        float pluie = CalculerPluie(saison);
        float temperature = CalculerTemperature(saison);
        bool intemperie = DeterminerIntemperie(saison);

        // Nouvelle échelle 1–5 selon la saison
        int lum;
        if (saison is SaisonPluvieuse)
            lum = _rng.Next(1, 4); // 1, 2 ou 3
        else
            lum = _rng.Next(3, 6); // 3, 4 ou 5

        // Impact de la pluie sur la température seulement
        if (pluie > 0f)
            temperature -= 5f;

        return new Meteo(pluie, lum, temperature, intemperie, saison, semaineActuelle);
    }

    private static float CalculerPluie(Saison saison)
    {
        if (_rng.NextSingle() < saison.ProbabilitePluie)
        {
            float factor = 0.8f + _rng.NextSingle() * 0.4f;
            return Math.Clamp(saison.ProbabilitePluie * factor, 0f, 1f);
        }
        return 0f;
    }

    private static float CalculerTemperature(Saison saison)
        => saison.TemperatureMoyenne
         + (_rng.NextSingle() * 2f - 1f)
         * saison.VariationTemperature;

    private static bool DeterminerIntemperie(Saison saison)
        => _rng.NextSingle() < saison.ProbabiliteIntemperie;

    private string GenererDescription(int semaine)
    {
        // Cartographie verbale pour les indices 1–5
        string[] niveaux = { "nul", "faible", "modéré", "fort", "très fort" };

        string desc = $"📆 Semaine           : {semaine}\n" +
                      $"🍂 Saison            : {SaisonActuelle.NomSaison}\n" +
                      $"🌡️  Température       : {Temperature:F1}°C\n" +
                      $"🌧️  Pluie             : {QuantitePluie:P0}\n" +
                      $"☀️  Ensoleillement    : indice {Luminosite} ({niveaux[Luminosite - 1]})";

        if (semaine > 5 && Intemperie)
        {
            desc += _rng.Next(2) == 0
                ? "\n⛈️ Orage"
                : "\n❄️ Grêle";
        }

        return desc;
    }
}
