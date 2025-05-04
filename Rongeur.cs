public class Rongeur : Animal
{
    public Rongeur() : base("Rongeur", "Mangeur de racines", 0.05f) { }
    public override void AppliquerEffets(Plante p)
    {
        // Ex : plante meurt immédiatement
        p.Tuer();
    }   
    
}