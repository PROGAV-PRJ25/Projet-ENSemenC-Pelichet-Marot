using System.Text.Json;

public static class SaveManager
{
    private const string SaveDir = "Saves";

    static SaveManager()
    {
        // S’assure que le dossier existe
        if (!Directory.Exists(SaveDir))
            Directory.CreateDirectory(SaveDir);
    }

    // Sauvegarde dans Saves/{slot}.json
    public static void Sauvegarder(string slot, SaveData data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(data, options);
        File.WriteAllText(Path.Combine(SaveDir, slot + ".json"), json);
    }

    // Liste tous les slots disponibles (sans l’extension)
    public static List<string> ListerSlots()
    {
        return Directory
            .GetFiles(SaveDir, "*.json")
            .Select(f => Path.GetFileNameWithoutExtension(f))
            .OrderBy(n => n)
            .ToList();
    }

    // Charge un slot donné, ou null si introuvable
    public static SaveData Charger(string slot)
    {
        string path = Path.Combine(SaveDir, slot + ".json");
        if (!File.Exists(path)) return null;
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<SaveData>(json);
    }
}
