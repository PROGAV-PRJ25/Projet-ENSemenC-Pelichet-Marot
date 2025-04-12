public class Soja : Plante
{
    public Soja() : base(
        nom: "Soja",
        espace: 2,
        terrain: new TerrainArgileux(),
        saison: new List<Saison> { new SaisonPluvieuse() },
        besoinEau:0.7f // Besoin moyen (70%)
    )
    {

    }
    public override void VerifierSante()
    {
        Console.WriteLine($"{Nom} : Vérification des pucerons...");
    }

    public override void Pousser()
    {
        Console.WriteLine($"{Nom} pousse en terrain {TerrainIdeal.Nom} !");
    }
}