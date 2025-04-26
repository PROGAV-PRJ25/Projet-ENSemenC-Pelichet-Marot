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
}