public class Soja : Plante
{
    public Soja()
        : base(
            nomPlante: "Soja",
            acronyme: "So",
            espace: 2,
            terrain: new TerrainClassiqueTerreux(), // Sol idéal
            saison: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 2f, // Perd 2% d'eau/jour
            temperatureMinimale: 10f,
            temperatureMaximale: 35f
        )
    { }

    public override void VerifierMort()
    {
        // à implémenter

    }

    public override void Pousser()
    {
        // à implémenter
    }
    public override void Desherber()
    {
        // à implémenter
    }
}
