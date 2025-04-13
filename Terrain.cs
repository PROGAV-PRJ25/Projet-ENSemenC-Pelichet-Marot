public abstract class Terrain
{
    public string NomTerrain {get;}
    public float Fertilité {get;}
    public float RetentionEau {get;}
    protected Terrain(string nomTerrain, float fertilite, float retentionEau)
    {
        NomTerrain=nomTerrain;
        Fertilité=fertilite;
        RetentionEau=retentionEau;
    }

}