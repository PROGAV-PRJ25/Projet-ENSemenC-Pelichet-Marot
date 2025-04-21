public abstract class Terrain
{
    public string NomTerrain { get; }
    public float Fertilité { get; }
    public float CoeffAbsorptionEau { get; }
    public ConsoleColor Couleur { get; protected set; }
    public Plante? Plante { get; set; }

    protected Terrain(string nomTerrain, float fertilite, float coeffAbsorptionEau, ConsoleColor couleur)
    {
        NomTerrain = nomTerrain;
        Fertilité = fertilite;
        CoeffAbsorptionEau = coeffAbsorptionEau;
        Couleur = couleur;
    }
}