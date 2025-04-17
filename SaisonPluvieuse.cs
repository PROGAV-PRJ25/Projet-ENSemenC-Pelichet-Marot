public class SaisonPluvieuse : Saison
{
    public SaisonPluvieuse()
        : base(
            nomSaison: "Pluvieuse",
            probaPluie: 0.7f,
            luminositeMoyenne: 0.6f,
            probaIntemperie: 0.5f,
            temperatureMoyenne: 20f,
            variationTemperature: 5f
        ) {}
}