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
        HydratationIdeale = 75f; // Spécifique au Soja
        LuminositeIdeale = 85f; // Spécifique au Soja
        Console.WriteLine($"[DEBUG SOJA] EspacePris: {EspacePris}");
    }
    public override void Arroser()
    {
        
    }
    public override void Pousser()
    {
        //tu feras ça plus tard
    }
    public override void Desherber()
    {
        //tu feras ça plus tard
    }
}
