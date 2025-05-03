public class CanneASucre : Plante
{
    public CanneASucre()
        : base(
            nomPlante: "Canne à Sucre",
            acronyme: "Cs",
            espacePris: 3,
            terrainIdeal: new TerrainClassiqueTerreux(),
            saisonCompatible: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 2.5f, // Très gourmande en eau
            temperatureMinimale: 20f,
            temperatureMaximale: 38f
        )
    {
        HydratationCritique = 60f; // Spécifique à la Canne à Sucre
    }


    public override void Pousser()
    {
        // À implémenter
    }
    public override void Arroser()
    {
        base.Arroser();
    }
}


