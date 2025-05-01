public class Coton : Plante
{
    public Coton()
        : base(
            nomPlante: "Coton",
            acronyme: "Co",
            espacePris: 2,
            terrainIdeal: new TerrainSableux(),
            saisonCompatible: new List<Saison> { new SaisonSeche() },
            vitesseDeshydratation: 1.8f,
            temperatureMinimale: 20f,
            temperatureMaximale: 40f
        )
    { }

    public override void Arroser()
    {
        // À implémenter
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
