using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Plante
{
    public string NomPlante { get; protected set; } = "Plante générique";
    public string Acronyme { get; protected set; } = "Pl";
    public int EspacePris { get; protected set; }
    public Terrain TerrainIdeal { get; protected set; }
    public List<Saison> SaisonCompatible { get; protected set; }
    public float HydratationIdeale { get; protected set; } = 80f; // Valeur par défaut
    public float LuminositeIdeale { get; protected set; } = 70f; // Valeur par défaut (ajoutée)
    public float TemperatureMinimale { get; protected set; }
    public float TemperatureMaximale { get; protected set; }
    public float VitesseDeshydratation { get; protected set; } // Ajout de la propriété
    public float HydratationActuelle { get; protected set; } = 100f;
    public float LuminositeActuelle { get; protected set; } = 50f;
    public float TemperatureActuelle { get; set; } = 15f;
    public Maladie MaladieActuelle { get; set; } = null;
    public bool EstMorte { get; protected set; } = false;
    private int joursSansMaladie = 0;
    private const int probMaladieInverse = 10;

    protected Plante(
        string nomPlante,
        string acronyme,
        int espacePris,
        Terrain terrainIdeal,
        List<Saison> saisonCompatible,
        float vitesseDeshydratation, 
        float temperatureMinimale,
        float temperatureMaximale
    )
    {
        NomPlante = nomPlante;
        Acronyme = acronyme;
        EspacePris = espacePris;
        TerrainIdeal = terrainIdeal;
        SaisonCompatible = saisonCompatible;
        VitesseDeshydratation = vitesseDeshydratation;
        TemperatureMinimale = temperatureMinimale;
        TemperatureMaximale = temperatureMaximale;
    }

    public virtual void Arroser()
    {
        HydratationActuelle = 100f; 
    }

    public virtual void RecevoirLumiere(float intensite)
    {
        LuminositeActuelle = Math.Min(100f, LuminositeActuelle + intensite);
    }

    public virtual void SetTemperature(float temperature)
    {
        TemperatureActuelle = temperature;
    }

    public virtual float EvaluerConditions(bool espaceRespecte)
    {
        int conditionsNonOptimales = 0;
        int totalConditions = 5; // Hydratation, Luminosité, Température, Maladie, Espacement

        if (Math.Abs(HydratationActuelle - HydratationIdeale) >= 20f)
        {
            conditionsNonOptimales++;
        }

        if (Math.Abs(LuminositeActuelle - LuminositeIdeale) >= 20f)
        {
            conditionsNonOptimales++;
        }

        if (TemperatureActuelle < TemperatureMinimale || TemperatureActuelle > TemperatureMaximale)
        {
            conditionsNonOptimales++;
        }

        if (MaladieActuelle != null)
        {
            conditionsNonOptimales++;
        }

        if (!espaceRespecte)
        {
            conditionsNonOptimales++;
        }

        return (float)conditionsNonOptimales / totalConditions;
    }

    public virtual void Update(float tempsEcouleEnJours, float temperatureDuJour, bool espaceRespecte)
    {
        if (EstMorte) return;

        TemperatureActuelle = temperatureDuJour;

        if (MaladieActuelle == null)
        {
            if (joursSansMaladie >= probMaladieInverse - 1)
            {
                if (new Random().Next(probMaladieInverse) == 0)
                {
                    AttraperMaladie(new Maladie("Mildew", "Apparition de taches blanches sur les feuilles."));
                    joursSansMaladie = 0;
                }
                else
                {
                    joursSansMaladie++;
                }
            }
            else
            {
                joursSansMaladie++;
            }
        }
        else
        {
            MaladieActuelle.AppliquerEffets(this);
        }

        float evaluationNonOptimale = EvaluerConditions(espaceRespecte);

        if (evaluationNonOptimale >= 0.6f) // Au moins 3 conditions non respectées
        {
            EstMorte = true;
            Console.WriteLine($"[MORT] {NomPlante} est morte car au moins 3 conditions n'étaient pas réunies.");
        }
        else
        {
            Console.WriteLine($"{NomPlante} a {(1 - evaluationNonOptimale) * 100:F0}% de ses conditions respectées." + (MaladieActuelle != null ? $" (Malade de {MaladieActuelle.NomMaladie})" : (espaceRespecte ? "" : " (Espacement non respecté)")));
        }

        HydratationActuelle = Math.Max(0f, HydratationActuelle - (VitesseDeshydratation * tempsEcouleEnJours));
        LuminositeActuelle = Math.Max(0f, LuminositeActuelle - 1f * tempsEcouleEnJours);
    }

    public virtual void AttraperMaladie(Maladie maladie)
    {
        MaladieActuelle = maladie;
        Console.WriteLine($"[MALADIE] {NomPlante} a attrapé : {maladie.NomMaladie}");
    }

    public abstract void Pousser();
    public abstract void Desherber();
}