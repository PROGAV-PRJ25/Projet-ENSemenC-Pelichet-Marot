using System.Text.Json;

public static class SaveManager
{
    private const string SaveDir = "Saves";

    static SaveManager()
    {
        if (!Directory.Exists(SaveDir))
            Directory.CreateDirectory(SaveDir);
    }

    // Écriture JSON de SaveData
    private static void Écrire(string slot, SaveData data)
    {
        var opts = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, opts);
        File.WriteAllText(Path.Combine(SaveDir, slot + ".json"), json);
    }

    // Ancienne méthode conservée si besoin
    public static void Sauvegarder(string slot, SaveData data)
        => Écrire(slot, data);

    // NOUVELLE surcharge : on construit directement SaveData à partir de la simulation
    public static void Sauvegarder(string slot, GestionPotager sim)
    {
        // 1) Crée et initialise SaveData avec contexte et météo
        var meteo = sim.GetDerniereMeteo();
        var data = new SaveData
        {
            Semaine = sim.GetSemaine(),
            Graines = sim.GetGraines().Nombre,
            MeteoPluie = meteo.QuantitePluie,
            MeteoLuminosite = meteo.Luminosite,
            MeteoTemperature = meteo.Temperature,
            MeteoIntemperie = meteo.Intemperie,
            MeteoSaison = meteo.SaisonActuelle.NomSaison
        };

        var plateau = sim.GetPlateau();
        int hauteur = sim.GetHauteur();
        int largeur = sim.GetLargeur();

        // 2) Sauvegarde du biome complet
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                data.Terrains.Add(new TerrainCell
                {
                    X = x,
                    Y = y,
                    TypeTerrain = plateau[y, x].NomTerrain
                });
            }
        }

        // 3) Sauvegarde des plantes et de leur état complet
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                var p = plateau[y, x].Plante;
                if (p == null) continue;

                data.Plantes.Add(new PlantCell
                {
                    X = x,
                    Y = y,
                    TypePlante = p.NomPlante,
                    TerrainNom = plateau[y, x].NomTerrain,
                    HauteurActuelle = p.HauteurActuelle,
                    EstMature = p.EstMature,
                    EstMorte = p.EstMorte,
                    SemainesDepuisPlantation = p.SemainesDepuisPlantation,
                    SommeSatisfaction = p.SommeSatisfaction,
                    HydratationActuelle = p.HydratationActuelle,
                    LuminositeActuelle = p.LuminositeActuelle,
                    TemperatureActuelle = p.TemperatureActuelle,
                    RendementBase = p.RendementBase,
                    ObstacleNom = p.ObstacleActuel?.Nom
                });
            }
        }

        // 4) Sérialisation et écriture dans le fichier JSON
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        string path = Path.Combine("Saves", slot + ".json");
        File.WriteAllText(path, json);
    }


    public static List<string> ListerSlots()
        => Directory
           .GetFiles(SaveDir, "*.json")
           .Select(f => Path.GetFileNameWithoutExtension(f))
           .OrderBy(n => n)
           .ToList();

    public static SaveData Charger(string slot)
    {
        string path = Path.Combine(SaveDir, slot + ".json");
        if (!File.Exists(path)) return null;
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<SaveData>(json);
    }
}
