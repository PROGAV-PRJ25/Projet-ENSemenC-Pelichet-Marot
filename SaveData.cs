using System.Collections.Generic;

public class TerrainCell
{
    public int    X           { get; set; }
    public int    Y           { get; set; }
    public string TypeTerrain { get; set; }
}

public class PlantCell
{
    public int    X                        { get; set; }
    public int    Y                        { get; set; }
    public string TypePlante               { get; set; }
    
    // Nouveau : pour reconstituer le type de sol
    public string TerrainNom               { get; set; }

    // État de croissance et de vie
    public float  HauteurActuelle          { get; set; }
    public bool   EstMature                { get; set; }
    public bool   EstMorte                 { get; set; }

    // Suivi temporel et satisfaction
    public int    SemainesDepuisPlantation { get; set; }
    public float  SommeSatisfaction        { get; set; }

    // Jauges
    public float  HydratationActuelle      { get; set; }
    public int    LuminositeActuelle       { get; set; }
    public float  TemperatureActuelle      { get; set; }

    // Récolte et bonus
    public int    RendementBase            { get; set; }

    // Obstacle en cours
    public string ObstacleNom              { get; set; }
}


public class SaveData
{
    // Contexte de la partie
    public int    Semaine          { get; set; }
    public int    Graines          { get; set; }

    // Météo au moment de la sauvegarde
    public float  MeteoPluie       { get; set; }
    public int    MeteoLuminosite  { get; set; }
    public float  MeteoTemperature { get; set; }
    public bool   MeteoIntemperie  { get; set; }
    public string MeteoSaison      { get; set; }

    // Biome complet
    public List<TerrainCell> Terrains { get; set; } = new();

    // Plantes et obstacles
    public List<PlantCell>   Plantes   { get; set; } = new();
}
