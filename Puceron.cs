public class Puceron : Insecte
{
    public float Degats { get; } = 2f;
    public Puceron() : base("Puceron", "Petit insecte suceur", 0.12f) { }
    public override void AppliquerEffets(Plante p)
    {
        // Ex : réduit un peu d’eau et ralentit la croissance
        p.ReduireHydratation(Degats);
        // Etc.
    }
}