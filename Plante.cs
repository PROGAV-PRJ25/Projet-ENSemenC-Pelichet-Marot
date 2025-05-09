using System;
using System.Collections.Generic;



public abstract class Plante
{
    // Générateur partagé pour la maladie
    private static readonly Random _rng = new Random();

    private Graines _graines;

    // — Propriétés / Attributs — 
    public int PrixGraines { get; protected set; } = 0;
    public string NomPlante { get; protected set; }
    public string Acronyme { get; protected set; }
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }

    public float HydratationCritique { get; protected set; } = 30f;
    public int LuminositeIdeale { get; protected set; } = 3; // échelle 1–5
    public float TemperatureMinimale { get; protected set; }
    public float TemperatureMaximale { get; protected set; }
    public float VitesseDeshydratation { get; protected set; }

    public float HydratationActuelle { get; set; } = 100f;
    public int LuminositeActuelle { get; protected set; } = 3;
    public float TemperatureActuelle { get; set; } = 15f;

    public Obstacle? ObstacleActuel { get; private set; } = null;
    public bool EstMorte { get; protected set; } = false;

    public float VitesseCroissance { get; protected set; }
    public float HauteurActuelle { get; protected set; } = 0f;
    public float HauteurMaximale { get; protected set; } = 1f;


    // Pour le suivi de maturation
    public int JoursDepuisPlantation { get; private set; } = 0;
    private float sommeSatisfaction = 0f;
    public bool EstMature => HauteurActuelle >= HauteurMaximale;

    // Nombre de graines produites à maturation parfaite.
    public int RendementBase { get; protected set; } = 1;


    // — Constructeur — 
    protected Plante(
        Graines graines,
        int prixGraines,
        string nomPlante,
        string acronyme,
        int espacePris,
        Terrain terrainIdeal,
        List<Saison> saisonCompatible,
        float vitesseDeshydratation,
        float temperatureMinimale,
        float temperatureMaximale,
        float vitesseCroissance,
        float hauteurMaximale
    )
    {
        _graines = graines;
        PrixGraines = prixGraines;
        NomPlante = nomPlante;
        Acronyme = acronyme;
        EspacePris = espacePris;
        TerrainIdeal = terrainIdeal;
        SaisonCompatible = saisonCompatible;

        VitesseDeshydratation = vitesseDeshydratation;
        TemperatureMinimale = temperatureMinimale;
        TemperatureMaximale = temperatureMaximale;

        VitesseCroissance = vitesseCroissance;
        HauteurMaximale = hauteurMaximale;
        HauteurActuelle = 0f;

        // États initiaux
        HydratationActuelle = 100f;
        LuminositeActuelle = LuminositeIdeale;
        TemperatureActuelle = (temperatureMinimale + temperatureMaximale) / 2f;
    }

    // — Méthodes d’aide pour l’extérieur (ex: Obstacle) — 
    public void ReduireHydratation(float montant)
    {
        HydratationActuelle = Math.Max(0f, HydratationActuelle - montant);
    }

    public void Tuer()
    {
        EstMorte = true;
    }

    // — Actions de base — 
    public virtual void Arroser()
    {
        if (_graines.PeutDepenser(5))
        {
            _graines.Depenser(5);
            HydratationActuelle = 100f;
        }
    }

    public void SetLuminosite(int indice)
    {
        LuminositeActuelle = Math.Clamp(indice, 1, 5);
    }

    public virtual void SetTemperature(float temperature)
    {
        TemperatureActuelle = temperature;
    }

    // — Évaluation des 5 conditions — 
    public virtual float EvaluerConditions(bool espaceRespecte, Saison saisonActuelle, Terrain terrainActuel)
    {
        int defauts = 0;
        // Hydratation
        if (HydratationActuelle < HydratationCritique) defauts++;
        // 2) Luminosité : valide si == idéale ou == idéale−1
        if (LuminositeActuelle != LuminositeIdeale && LuminositeActuelle != (LuminositeIdeale - 1))
        {
            defauts++;
        }
        // Température
        if (TemperatureActuelle < TemperatureMinimale
         || TemperatureActuelle > TemperatureMaximale) defauts++;
        // Maladie
        if (ObstacleActuel != null) defauts++;
        // Espacement
        if (!espaceRespecte) defauts++;
        // Saison de semis
        bool condSaison = SaisonCompatible.Any(s => s.NomSaison == saisonActuelle.NomSaison);
        if (!condSaison) defauts++;
        // Terrain préféré
        bool condTerrain = terrainActuel.GetType() == TerrainIdeal.GetType();
        if (!condTerrain) defauts++;

        return (float)defauts / 7f;
    }

    // — Mise à jour journalière — 
    public virtual void Update(
    float tempsEcouleEnJours,
    float temperatureDuJour,
    bool espaceRespecte,
    float coeffAbsorptionEau,
    int luminositeDuJour,
    Saison saisonActuelle,
    Terrain terrainActuel
)
    {
        if (EstMorte)
            return;

        // 1) Appliquer la température du jour
        SetTemperature(temperatureDuJour);

        // 2) Effets de l’obstacle (maladie, insecte, animal…)
        if (ObstacleActuel != null)
            ObstacleActuel.AppliquerEffets(this);

        // 3) Calcul du taux de non-satisfaction (7 critères)
        float tauxNonOpt = EvaluerConditions(
            espaceRespecte,
            saisonActuelle,
            terrainActuel
        );
        if (tauxNonOpt >= 0.5f)
        {
            Tuer();
            return;
        }
        float tauxOpt = 1f - tauxNonOpt;

        // 4) Comptabiliser le jour et accumuler la satisfaction
        JoursDepuisPlantation++;
        sommeSatisfaction += tauxOpt;

        // 5) Croissance proportionnelle à la satisfaction
        Pousser(tauxOpt);

        // 6) Perte d’eau modulée par le sol
        float perteEau = VitesseDeshydratation
                       * tempsEcouleEnJours
                       * (1f - coeffAbsorptionEau);
        ReduireHydratation(perteEau);

        // 7) Fixer l’indice de luminosité du jour (1 à 5)
        SetLuminosite(luminositeDuJour);
    }




    public int Recolter()
    {
        if (!EstMature)
            return 0;

        // rendement proportionnel à la satisfaction moyenne
        float moyenne = sommeSatisfaction / JoursDepuisPlantation;
        int grainesGain = (int)Math.Round(RendementBase * moyenne);

        // on marque la plante comme morte (elle disparaîtra au désherbage)
        Tuer();
        return grainesGain;
    }

    // — Croissance — 
    protected virtual void Pousser(float tauxSatisfaction)
    {
        float delta = VitesseCroissance * tauxSatisfaction;
        HauteurActuelle = Math.Min(HauteurMaximale, HauteurActuelle + delta);
    }


    public void PlacerObstacle(Obstacle obs)
    {
        ObstacleActuel = obs;
    }
}

