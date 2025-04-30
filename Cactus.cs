public class Cactus : Plante
{
    public Cactus()
        : base(
            nomPlante: "Cactus",
            acronyme: "Ca",
            espace: 1,
            terrain: new TerrainSableux(),
            saison: new List<Saison> { new SaisonSeche() },
            vitesseDeshydratation: 0.5f, // Très faible, il garde bien l’eau
            temperatureMinimale: 10f,
            temperatureMaximale: 45f
        )
    {
        EstVivace = true;
    }

    public override float CalculerVivacite(Meteo meteo)
    {
        return base.CalculerVivacite(meteo);
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
