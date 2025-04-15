public class Soja : Plante
{
    public Soja()
        : base(
            nomPlante: "Soja",
            espace: 2,
            terrain: new TerrainArgileux(), // Sol idéal
            saison: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 2f, // Perd 2% d'eau/jour
            temperatureMinimale:10f,
            temperatureMaximale:35f
        )
    { }

    public override void VerifierSante()
    {
        if (Hydratation <= 0)
        {
            Console.WriteLine($"{NomPlante} est morte de soif.");
        }
        else if (Hydratation < 30)
        {
            Console.WriteLine($"{NomPlante} a besoin d'eau.");
        }
    }

    public override void Pousser()
    {
        Console.WriteLine($"{NomPlante} pousse normalement.");
    }
}
