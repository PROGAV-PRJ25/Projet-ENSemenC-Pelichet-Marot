public class Oiseau : Animal
{
    public Oiseau()
        : base(
            nom:  "Oiseau", 
            desc: "Cueille les fruits, réduit le rendement prochain", 
            proba: 0.007f)
    { }

    public override void AppliquerEffets(Plante p)
    {
        // Diminue une fois le rendement
        p.DiminuerRendement(5);

        // Puis l'oiseau s'envole : on retire l'obstacle
        
    }
}

