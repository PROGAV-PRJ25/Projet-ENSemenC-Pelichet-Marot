public abstract class Plante
{
    public string Nom { get; protected set; }
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }
    public float BesoinEau { get; protected set; }
    public DateTime DernierArrosage { get; protected set; }

    protected Plante(string nom, int espace, Terrain terrain, List<Saison> saison, float besoinEau)
    {
        Nom = nom;
        EspacePris = espace;
        TerrainIdeal = terrain;
        SaisonCompatible = saison;
        BesoinEau = besoinEau;
        DernierArrosage = DateTime.Now;
    }
    public abstract void VerifierSante();
    public abstract void Pousser();

    public virtual void Arroser(int quantiteEau)
    {
        DernierArrosage = DateTime.Now;

        if (quantiteEau < BesoinEau * 100) // Ex: BesoinEau=0.7 → 70 unités
        {
            Console.WriteLine($"[ATTENTION] {Nom} n'a pas reçu assez d'eau !");
        }
        else
        {
            Console.WriteLine($"{Nom} a été correctement arrosé.");
        }
    }
}