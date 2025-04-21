public class Coton : Plante
{
    public Coton()
        : base(
            nomPlante: "Coton",
            acronyme: "Co",
            espace: 2,
            terrain: new TerrainSableux(),
            saison: new List<Saison> { new SaisonSeche() },
            vitesseDeshydratation: 1.8f,
            temperatureMinimale: 20f,
            temperatureMaximale: 40f
        )
    {
        EstVivace = false;
    }


    public override void VerifierMort()
    {
        // À implémenter
    }


    public override void Pousser()
    {
        // À implémenter
    }


    public override void Desherber()
    {
        // À implémenter
    }


   
}
