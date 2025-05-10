public class Puceron : Insecte
{
    public float Degats { get; } = 5f;
    public Puceron() : base(
        nom: "Puceron",
        desc: "Petit insecte suceur",
        proba: 0.0012f
        )
    { }
    public override void AppliquerEffets(Plante p)
    {
        // Ex : réduit un peu d’eau 
        p.ReduireHydratation(Degats);
        
    }
}