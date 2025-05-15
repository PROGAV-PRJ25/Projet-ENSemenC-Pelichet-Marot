 public class SaveData
    {
        public int Semaine { get; set; }
        public int Graines { get; set; }
        public List<PlantCell> Plantes { get; set; } = new();
    }

    /// <summary>
    /// État d’une plante sur une case X,Y.
    /// </summary>
    public class PlantCell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string TypePlante { get; set; }           // Ex: "Soja", "Cafe"
        public float HauteurActuelle { get; set; }
        public float HydratationActuelle { get; set; }
    }