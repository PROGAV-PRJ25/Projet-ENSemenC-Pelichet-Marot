public class MaladieMildew : Maladie
{
    public MaladieMildew()
        : base(
            nom: "Mildew",
            desc: "Taches blanches sur les feuilles",
            proba: 0.001f,
            gravite: 5f
        )
    {}
}