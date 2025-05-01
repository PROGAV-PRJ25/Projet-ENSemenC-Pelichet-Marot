public abstract class Plante
{
    public string NomPlante { get; protected set; } = "Plante générique";
    public string Acronyme { get; protected set; } = "Pl";
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public Meteo Meteo { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }
    public float Hydratation { get; protected set; } = 100f;
    public float VitesseDeshydratation { get; protected set; }
    public float TemperatureMinimale { get; protected set; }
    public float TemperatureMaximale { get; protected set; }
    public float DiffTemperature { get; protected set; }
    public float JoursHorsLimiteTemperature { get; protected set; } = 0;
    public const int JoursMortTemperature = 10;
    public bool EstEnStressThermique { get; protected set; } = false;
    public bool SanteCritique { get; protected set; }
    public bool EstVivace { get; protected set; } = false;
    public bool EstMorte { get; protected set; } = false;

    protected Plante(
        string nomPlante,
        string acronyme,
        int espace,
        Terrain terrain,
        List<Saison> saison,
        float vitesseDeshydratation,
        float temperatureMinimale,
        float temperatureMaximale
    )
    {
        NomPlante = nomPlante;
        Acronyme = acronyme;
        EspacePris = espace;
        TerrainIdeal = terrain;
        SaisonCompatible = saison;
        VitesseDeshydratation = vitesseDeshydratation;
        TemperatureMinimale = temperatureMinimale;
        TemperatureMaximale = temperatureMaximale;
    }

    public virtual float CalculerVivacite(Meteo meteo)
    {
        if (meteo.Temperature < TemperatureMinimale)
        {
            DiffTemperature = TemperatureMinimale - meteo.Temperature;
        }
        else if (meteo.Temperature > TemperatureMaximale)
        {
            DiffTemperature = meteo.Temperature - TemperatureMaximale;
        }
        else
        {
            DiffTemperature = 0;
        }
        return Hydratation - (0.1f * DiffTemperature); //à compléter
    }

    public virtual void Arroser(int quantiteEau)
    {
        float gain = quantiteEau * TerrainIdeal.CoeffAbsorptionEau;
        Hydratation = Math.Min(100, Hydratation + gain);
    }

    public virtual bool ASoif => Hydratation < 30f;

    public virtual void Update(Meteo meteo, float tempsEcouleEnJours)
    {
        if (Hydratation <= 0)
        {
            Console.WriteLine($"[MORT] {NomPlante} est déjà morte");
            return;
        }

        AppliquerPluie(meteo, tempsEcouleEnJours);
        UpdateHydratation(tempsEcouleEnJours);
        UpdateStressTemperature(meteo.Temperature, tempsEcouleEnJours);
        AppliquerEffetsStress(tempsEcouleEnJours);
        VerifierMort();
    }

    protected virtual void AppliquerPluie(Meteo meteo, float tempsEcouleEnJours)
    {
        if (meteo.QuantitePluie > 0)
        {
            float absorption = meteo.QuantitePluie * TerrainIdeal.CoeffAbsorptionEau * 0.3f;
            float gain = absorption * tempsEcouleEnJours;
            Hydratation = Math.Min(100, Hydratation + gain);

            Console.WriteLine($"{NomPlante} absorbe {meteo.QuantitePluie:P0} de pluie " +
                            $"(Terrain: {TerrainIdeal.GetType().Name}, " +
                            $"Coeff: {TerrainIdeal.CoeffAbsorptionEau:P0}) → " +
                            $"+{gain:F1}% hydratation");
        }
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

    public virtual void VerifierMort()
    {
        if (EstMorte)
        {
            return;
        }

        if (JoursHorsLimiteTemperature >= JoursMortTemperature && Hydratation > 0)
        {
            EstMorte = true;
            Console.WriteLine($"[MORT] {NomPlante} tuée par la température");
            return;
        }

        if (Hydratation <= 0)
        {
            EstMorte = true;
            Console.WriteLine($"[MORT] {NomPlante} morte de déshydratation");
        }
    }



    public abstract void Pousser();
    public abstract void Desherber();
}