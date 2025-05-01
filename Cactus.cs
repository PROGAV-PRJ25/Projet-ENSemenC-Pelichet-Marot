public class Cactus : Plante
{
    public Cactus()
        : base(
            nomPlante: "Cactus",
            acronyme: "Ca",
            espacePris: 1,
            terrainIdeal: new TerrainSableux(),
            saisonCompatible: new List<Saison> { new SaisonSeche() },
            vitesseDeshydratation: 0.5f, // Très faible, il garde bien l’eau
            temperatureMinimale: 10f,
            temperatureMaximale: 45f
        )
    { }



    public override void Pousser()
    {
        // À implémenter
    }


    public override void Desherber()
    {
        // À implémenter
    }


    public override void Arroser()
    {
        // À implémenter
    }
}
