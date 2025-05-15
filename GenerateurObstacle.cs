public static class GenerateurObstacle
{
    private static readonly Random _rng = new Random();

    // Catalogue de constructeurs d’obstacles
    private static readonly List<Func<Obstacle>> _catalogue = new()
        {
            () => new MaladieMildew(),
            () => new MaladieRouille(),
            () => new Puceron(),
            () => new Rongeur(),
            () => new Oiseau()
            // ajoute ici d’autres obstacles/insectes/animaux si besoin
        };

    /// <summary>
    /// Tente de générer un obstacle : 
    /// on parcourt le catalogue dans un ordre aléatoire,
    /// on crée une instance, on teste SeDeclare(), et
    /// on retourne le premier qui se déclenche.
    /// Si aucun ne se déclenche, on renvoie null.
    /// </summary>
    public static Obstacle? GenererObstacle()
    {
        // Mélange aléatoire des entrées
        foreach (var factory in _catalogue.OrderBy(_ => _rng.Next()))
        {
            var obs = factory();
            if (obs.SeDeclare())
                return obs;
        }
        return null;
    }

    public static Obstacle CreerParNom(string nom)
    {
        return nom switch
        {
            "Puceron" => new Puceron(),
            "Rouille" => new MaladieRouille(),
            "Mildew" => new MaladieMildew(),
            "Oiseau" => new Oiseau(),
            "Rongeur" => new Rongeur(),
            _ => null
        };
    }
}