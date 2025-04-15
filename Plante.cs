public abstract class Plante
{
    public string NomPlante { get; protected set; }
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }
    public float Hydratation { get; protected set; } = 100f;
    public float VitesseDeshydratation { get; protected set; }
    public float TemperatureMinimale { get; protected set; }
    public float TemperatureMaximale { get; protected set; }
    public float JoursHorsLimiteTemperature { get; protected set; } = 0;
    public const int JoursMortTemperature = 10;

    public bool EstEnStressThermique { get; protected set; } = false;

    protected Plante(
        string nomPlante,
        int espace,
        Terrain terrain,
        List<Saison> saison,
        float vitesseDeshydratation,
        float temperatureMinimale,
        float temperatureMaximale
    )
    {
        NomPlante = nomPlante;
        EspacePris = espace;
        TerrainIdeal = terrain;
        SaisonCompatible = saison;
        VitesseDeshydratation = vitesseDeshydratation;
        TemperatureMinimale = temperatureMinimale;
        TemperatureMaximale = temperatureMaximale;
    }

    public virtual void Arroser(int quantiteEau)
    {
        float gain = quantiteEau * TerrainIdeal.CoeffAbsorptionEau;
        Hydratation = Math.Min(100, Hydratation + gain);
    }

    public virtual bool ASoif => Hydratation < 30f;

    public virtual void Update(float temperatureActuelle, float tempsEcouleEnJours)
    {
        if (Hydratation <= 0)
        {
            Console.WriteLine($"[MORT] {NomPlante} est déjà morte");
            return;
        }
        UpdateHydratation(tempsEcouleEnJours);
        UpdateStressTemperature(temperatureActuelle, tempsEcouleEnJours);
        AppliquerEffetsStress(tempsEcouleEnJours);
        VerifierMort();
    }

    protected virtual void UpdateHydratation(float tempsEcouleEnJours)
    {
        float perte = tempsEcouleEnJours * VitesseDeshydratation;
        Hydratation = Math.Max(0, Hydratation - perte);
    }

    protected virtual void UpdateStressTemperature(float temperatureActuelle, float tempsEcouleEnJours)
    {
        bool estEnDanger = temperatureActuelle < TemperatureMinimale || temperatureActuelle > TemperatureMaximale;

        EstEnStressThermique = estEnDanger;

        if (estEnDanger)
        {
            JoursHorsLimiteTemperature += (int)Math.Ceiling(tempsEcouleEnJours); // Arrondi à jours entiers
        }
        else
        {
            JoursHorsLimiteTemperature = 0;
        }
    }


    protected virtual void AppliquerEffetsStress(float tempsEcouleEnJours)
    {

        if (EstEnStressThermique)
        {
            // Effets fixes quand en stress
            Hydratation = Math.Max(0, Hydratation - 20f * tempsEcouleEnJours); // Désydratation accélérée
        }
    }

    protected virtual void VerifierMort()
    {
        if (JoursHorsLimiteTemperature >= JoursMortTemperature && Hydratation > 0)
        {
            Hydratation = 0;
            Console.WriteLine($"[MORT] {NomPlante} tuée par la température");
            return;
        }

        if (Hydratation <= 0)
        {
            Console.WriteLine($"[MORT] {NomPlante} morte de déshydratation");
        }
    }


    public abstract void VerifierSante();
    public abstract void Pousser();
}