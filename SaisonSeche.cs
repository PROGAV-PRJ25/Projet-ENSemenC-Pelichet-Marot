public class SaisonSeche : Saison
{
    public SaisonSeche() 
        : base(
            nomSaison: "Sèche",
            probaPluie: 0.1f,
            luminositeMoyenne: 0.9f,
            probaIntemperie: 0.3f,
            temperatureMoyenne: 30f,
            variationTemperature: 8f
        ) {}
}