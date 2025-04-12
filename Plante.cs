public abstract class Plante
{
    public string Nom {get; protected set;}
    public int EspacePris {get; protected set;}
    public Terrain TerrainIdeal {get; protected set;}
    public List<Saison> SaisonCompatible {get; protected set;}
    public float BesoinEau {get; protected set;}
    public DateTime DernierArrosage {get; protected set;}

    protected Plante(string nom, int espace, Terrain terrain, List<Saison> saison, float besoinEau)
    {
        Nom=nom;
        EspacePris=espace;
        TerrainIdeal=terrain;
        SaisonCompatible=saison;
        BesoinEau=besoinEau;
        DernierArrosage=DateTime.Now;
    }
    public abstract void VerifierSante();
    public abstract void Pousser();


}