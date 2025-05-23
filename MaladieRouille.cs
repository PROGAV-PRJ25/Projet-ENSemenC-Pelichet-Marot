public class MaladieRouille : Maladie
{
    public MaladieRouille()
        : base(
            nom: "Rouille",
            desc: "Poussière orange sur les tiges, ralenit la croissance",
            proba: 0.008f,
            gravite: 3f
        )
    {}
     public override void AppliquerEffets(Plante p)
    {
        // Ralentit la croissance, ne touche pas à l'hydratation
        p.RalentirCroissance(Gravite);
    }
}