public class MaladieMildew : Maladie
{
    public MaladieMildew()
        : base(
            nom: "Mildew",
            desc: "Taches blanches sur les feuilles, déshydrate",
            proba: 0.001f,
            gravite: 5f
        )
    {}
    public override void AppliquerEffets(Plante p)
    {
        // c’est un champignon, il pompe l’eau
        p.ReduireHydratation(Gravite);
    }
}