public abstract class Plante
{
    public string Nom {get; protected set;}
    public int EspacePris {get; protected set;}
    public Terrain TerrainIdeal {get; protected set;}
    public List<Saison> SaisonCompatible {get; protected set;}

    protected Plante(string nom, int espace, Terrain terrain, List<Saison> saison)
    {
        Nom=nom;
        EspacePris=espace;
        TerrainIdeal=terrain;
        SaisonCompatible=saison;
    }
    public abstract void VerifierSante();
    public abstract void Pousser();
    

}