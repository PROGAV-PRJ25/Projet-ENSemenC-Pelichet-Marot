public class Maladie
{
    public string NomMaladie { get; private set; }
    public string Description { get; private set; }
    // On pourrait ajouter d'autres propriétés comme la gravité, le taux de propagation, etc.

    public Maladie(string nomMaladie, string description)
    {
        NomMaladie = nomMaladie;
        Description = description;
    }

    public virtual void AppliquerEffets(Plante plante)
    {
        Console.WriteLine($"[MALADIE] {plante.NomPlante} est affecté par : {NomMaladie}");
        // Ici, on pourrait implémenter des effets spécifiques de la maladie sur la plante
        // (réduction de l'hydratation, ralentissement de la croissance, etc.)
    }
}