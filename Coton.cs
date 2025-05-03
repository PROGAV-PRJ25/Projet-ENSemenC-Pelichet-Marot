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
    {
        HydratationCritique = 35f; // Spécifique au Coton
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
