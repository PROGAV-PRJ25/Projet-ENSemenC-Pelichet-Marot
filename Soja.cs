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

    public override void Arroser(int quantiteEau)
    {
        base.Arroser(quantiteEau); // Logique de base
        
        if (quantiteEau > 150) 
        {
            Console.WriteLine("⚠️ Trop d'eau pour le soja ! Risque de pourriture.");
        }
    }
}