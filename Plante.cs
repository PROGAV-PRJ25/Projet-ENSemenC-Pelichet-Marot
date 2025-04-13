public abstract class Plante
{
    public string NomPlante { get; protected set; }
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }
    public float BesoinEau { get; protected set; }
    public DateTime DernierArrosage { get; protected set; }

    protected Plante(string nomPlante, int espace, Terrain terrain, List<Saison> saison, float besoinEau)
    {
        NomPlante = nomPlante;
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
            Console.WriteLine($"[ATTENTION] {NomPlante} n'a pas reçu assez d'eau !");
        }
        else
        {
            Console.WriteLine($"{NomPlante} a été correctement arrosé.");
        }
    }
    public virtual bool ASoif
    {
        get

        {
            return (DateTime.Now - DernierArrosage).TotalHours > 48;  //Si on a pas arrosé depuis 48h
        }
    }
}