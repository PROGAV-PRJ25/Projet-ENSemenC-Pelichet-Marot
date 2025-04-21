public class CanneASucre : Plante
{
    public CanneASucre()
        : base(
            nomPlante: "Canne à Sucre",
            acronyme: "Cs",
            espace: 3,
            terrain: new TerrainArgileux(),
            saison: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 2.5f, // Très gourmande en eau
            temperatureMinimale: 20f,
            temperatureMaximale: 38f
        )
    {
        EstVivace = true;
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


    public override void Arroser(int quantiteEau)
    {
        // À implémenter
    }
}


