// Meteo.cs
using System;

public class Meteo
{
    private static readonly Random _rng = new Random();

    public float QuantitePluie   { get; }
    public float Luminosite      { get; }
    public float Temperature     { get; }
    public bool  Intemperie      { get; }
    public Saison SaisonActuelle { get; }
    public string Description    { get; }

    private Meteo(
        float pluie,
        float luminosite,
        float temperature,
        bool intemperie,
        Saison saison,
        int jourActuel
    )
    {
        QuantitePluie   = Math.Clamp(pluie,  0f, 1f);
        Luminosite      = Math.Clamp(luminosite,  0.1f, 1f);
        Temperature     = temperature;
        Intemperie      = intemperie;
        SaisonActuelle  = saison;
        Description     = GénérerDescription(jourActuel);
    }

    public static Meteo GenererPourSaison(Saison saison, int jourActuel)
    {
        float pluie       = CalculerPluie(saison);
        float luminosite  = CalculerLuminosite(saison);
        float temperature = CalculerTemperature(saison);
        bool  intemperie  = DeterminerIntemperie(saison);

        if (pluie > 0f)
        {
            luminosite = Math.Max(0.1f, luminosite - 0.2f);
            temperature -= 5f;
        }

        return new Meteo(pluie, luminosite, temperature, intemperie, saison, jourActuel);
    }

    private static float CalculerPluie(Saison saison)
    {
        if (_rng.NextSingle() < saison.ProbabilitePluie)
        {
            // Pluie randomisée entre 80% et 120% de la probabilité de la saison
            var factor = 0.8f + _rng.NextSingle() * 0.4f;
            return Math.Clamp(saison.ProbabilitePluie * factor, 0f, 1f);
        }
        return 0f;
    }

    private static float CalculerLuminosite(Saison saison)
    {
        var factor = 0.9f + _rng.NextSingle() * 0.2f;
        return Math.Clamp(saison.LuminositeMoyenne * factor, 0.1f, 1f);
    }

    private static float CalculerTemperature(Saison saison)
    {
        return saison.TemperatureMoyenne
             + ( _rng.NextSingle() * 2f - 1f )
             * saison.VariationTemperature;
    }

    private static bool DeterminerIntemperie(Saison saison)
        => _rng.NextSingle() < saison.ProbabiliteIntemperie;

    private string GénérerDescription(int jour)
    {
        var desc = $"Jour : {jour}\n" +
                   $"Saison       : {SaisonActuelle.NomSaison}\n" +
                   $"Température  : {Temperature:F1}°C\n" +
                   $"Pluie        : {QuantitePluie:P0}\n" +
                   $"Luminosité   : {Luminosite:P0}";

        if (jour > 14 && Intemperie)
        {
            desc += _rng.Next(2) == 0
                ? "\n⚡ Orage"
                : "\n🧊 Grêle";
        }

        return desc;
    }
}
