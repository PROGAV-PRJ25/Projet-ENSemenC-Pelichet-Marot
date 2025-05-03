public class Soja : Plante
{
    public Soja()
        : base(
            nomPlante: "Soja",
            acronyme: "So",
            espacePris: 2,
            terrainIdeal: new TerrainClassiqueTerreux(), // Sol idéal
            saisonCompatible: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 2f, // Perd 2% d'eau/jour
            temperatureMinimale: 10f,
            temperatureMaximale: 35f)
    { 
        HydratationCritique = 45f; // Spécifique au Soja
    }
    public override void Arroser()
    {
        base.Arroser();
    }
    public override void Pousser()
    {
        //tu feras ça plus tard
    }
}
