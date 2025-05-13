public class Rongeur : Animal
{

    public Rongeur() : base(
        nom: "Rongeur",
        desc: "Mangeur de racines, mortel",
        proba: 0.0005f)
    { }
    public override void AppliquerEffets(Plante p)
    {
        // plante meurt immédiatement
        p.Tuer();
        Console.WriteLine($"[MORT] les racines de {p.NomPlante} ont été grignotées par un rongeur");
        Thread.Sleep(2000);
    }

}