public abstract class Obstacle
{
    public string Nom        { get; protected set; }
    public string Description{ get; protected set; }

    /// <summary>
    /// Chance [0–1] qu’il apparaisse/propage.
    /// </summary>
    public float Probabilite { get; protected set; }

    protected Obstacle(string nom, string desc, float proba)
    {
        Nom         = nom;
        Description = desc;
        Probabilite = proba;
    }

    /// <summary>
    /// Décide aléatoirement si l’obstacle survient (ou se propage).
    /// </summary>
    public bool SeDeclare()
       => new Random().NextDouble() < Probabilite;

    /// <summary>
    /// Effets de l’obstacle sur la plante (eau, croissance, dégâts…).
    /// </summary>
    public abstract void AppliquerEffets(Plante plante);
}