public abstract class Terrain
{
    public string Nom {get;}
    public float Fertilité {get;}
    public float RetentionEau {get;}
    protected Terrain(string nom, float fertilite, float retentionEau)
    {
        Nom=nom;
        Fertilité=fertilite;
        RetentionEau=retentionEau;
    }

}