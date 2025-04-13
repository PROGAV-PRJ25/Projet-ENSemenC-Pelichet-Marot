public abstract class Terrain
{
    public string NomTerrain { get; }
    public float Fertilité { get; }
    public float CoeffAbsorptionEau { get; }

    protected Terrain(string nomTerrain, float fertilite, float coeffAbsorptionEau)
    {
        NomTerrain = nomTerrain;
        Fertilité = fertilite;
        CoeffAbsorptionEau = coeffAbsorptionEau;
    }
}
