public class Mais : Plante
{
    public Mais()
        : base(
            nomPlante: "Maïs",
            acronyme: "Ma",
            espacePris: 3, // Prend plus de place que le soja
            terrainIdeal: new TerrainSableux(), // Préfère les sols drainants
            saisonCompatible: new List<Saison> { new SaisonPluvieuse(), new SaisonSeche() }, // Vivace = supporte les 2 saisons
            vitesseDeshydratation: 1.5f, // Moins sensible à la sécheresse que le soja
            temperatureMinimale: 15f,  // Plus sensible au froid
            temperatureMaximale: 40f   // Supporte mieux la chaleur
        )
    {}
    public override void Arroser()
    {
        
    }
    public override void Pousser()
    {
        // Croissance rapide en saison des pluies (à faire quand on aura implémenté SaisonActuelle)
       // var croissance = SaisonActuelle is SaisonPluvieuse ? 1.8f : 0.7f;
      //  Console.WriteLine($"{NomPlante} pousse à {(croissance * 100):F0}% de vitesse.");


    }


    public override void Desherber()
    {
        // à implémenter
    }
}
