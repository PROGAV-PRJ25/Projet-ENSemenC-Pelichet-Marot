public class Cafe : Plante
{
    public Cafe()
        : base(
            nomPlante: "Café",
            acronyme: "Cf",
            espacePris: 3, // Un peu plus d'espace pour le caféier
            terrainIdeal: new TerrainArgileux(), // Terrain riche et bien drainé
            saisonCompatible: new List<Saison> { new SaisonPluvieuse(), new SaisonSeche() }, // Supporte les 2 saisons
            vitesseDeshydratation: 1.2f, // Moins sensible à la sécheresse, mais quand même sensible
            temperatureMinimale: 15f, // Température minimale
            temperatureMaximale: 30f // Température maximale
        )
    { }
    

    public override void Pousser()
    {
        // Le café pousse de manière lente et régulière en saison des pluies, maturation en saison sèche.
        // À implémenter quand on aura un système de saison actif.
        // Console.WriteLine($"{NomPlante} pousse lentement pendant la saison des pluies.");
    }


    public override void Desherber()
    {
        // Le caféier a des racines profondes, le désherbage est donc plus complexe.
       
    }
    public override void Arroser()
    {
        // À implémenter
    }
}
