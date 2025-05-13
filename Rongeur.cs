public class Rongeur : Animal
{
    public Rongeur() : base(
        nom: "Rongeur",
        desc: "Mangeur de racines, mortel",
        proba: 0.0005f) { }
    public override void AppliquerEffets(Plante p)
    {
        // plante meurt immédiatement
        p.Tuer();
    }   
    
}