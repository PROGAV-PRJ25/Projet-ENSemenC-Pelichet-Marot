public abstract class Plante
{
    public string NomPlante { get; protected set; }
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }

    // Jauge d'hydratation commune à toutes les plantes (0 = morte, 100 = parfaitement hydratée)
    public float Hydratation { get; protected set; } = 100f;

    // Vitesse de déshydratation (% perdu par jour)
    public float VitesseDeshydratation { get; protected set; }
    public float TemperatureMinimale { get; protected set; }
    public float TemperatureMaximale { get; protected set; }
    public float JoursHorsLimiteTemperature { get; protected set; } = 0;
    public const int JoursMortTemperature = 10;

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

    public virtual void Update(float tempsEcouleEnJours)
    {
        float perte = tempsEcouleEnJours * VitesseDeshydratation;
        Hydratation = Math.Max(0, Hydratation - perte); // Math.Max pour pas que l'hydratation descende en dessous de zéro (plante morte)
    }

    public virtual void Arroser(int quantiteEau)
    { // Absorption = quantité * coeff du terrain * ajustement plante
        float gain = quantiteEau * TerrainIdeal.CoeffAbsorptionEau * 0.01f;
        Hydratation = Math.Min(100, Hydratation + gain);
        Console.WriteLine(
            $"{NomPlante} a absorbé {gain:F1}% d'eau (sol {TerrainIdeal.NomTerrain})"
        );
    }

    public virtual bool ASoif
    {
        get { return Hydratation < 30f; } //return true SSI l'hydratation de la plante est inférieure à 30. seuil personnalisable dans la classe fille
    }
    public virtual void EffetTemperature(float temperatureActuelle, float tempsEcouleEnJours)
    {
        bool EstEnDanger = (temperatureActuelle < TemperatureMinimale || temperatureActuelle > TemperatureMaximale);
        if (EstEnDanger)
        {
            // 1. Accumule les jours dangereux
            JoursHorsLimiteTemperature += tempsEcouleEnJours;

            // 2. Applique un stress immédiat
            float temperatureIdeale = (TemperatureMinimale + TemperatureMaximale) / 2;
            float stressTemperature = Math.Abs(temperatureIdeale - temperatureActuelle) * 0.5f * tempsEcouleEnJours;
            Hydratation = Math.Max(0, Hydratation - stressTemperature);
            Console.WriteLine($"{NomPlante} subit un stress thermique !");

            // 3. Vérifie la mort thermique
            if (JoursHorsLimiteTemperature >= JoursMortTemperature && Hydratation > 0)
            {
                Hydratation = 0;
                Console.WriteLine($"[MORT] {NomPlante} a succombé à la température");
            }
            else // Réinitialisation du compteur si la plante n'est plus en danger thermique
            {
                JoursHorsLimiteTemperature = 0;

            }
        }
    }

    public abstract void VerifierSante();
    public abstract void Pousser();
}
