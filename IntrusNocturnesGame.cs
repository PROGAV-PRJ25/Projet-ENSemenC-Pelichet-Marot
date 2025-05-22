public class IntrusNocturnesGame : MiniJeu
{
    private readonly GestionPlateau _plateau;
    private readonly Random _rng = new();

    private readonly (string emoji, string nom, int rep, string lib)[] _table =
    {
        ("🐗", "sanglier",      1, "Lancer des cheveux !"),
        ("🦇", "chauve-souris", 2, "Placer de l’huile d’eucalyptus !"),
        ("🐀", "rat",           4, "Clou de girofle !"),
        ("🐺", "loup",          3, "Musique à fond !")
    };

    public IntrusNocturnesGame(GestionPlateau plateau)
    {
        _plateau = plateau;
    }

    public bool Run()
    {
        var intrus = _table[_rng.Next(_table.Length)];

        Console.Clear();
        Console.WriteLine("=== MODE URGENCE : INTRUS NOCTURNES ===\n");
        Console.WriteLine($"Un {intrus.emoji} {intrus.nom} rôde près de vos cultures !");
        Console.WriteLine("Choisissez l’action adéquate :\n");
        Console.WriteLine(" 1) Lancer des cheveux !");
        Console.WriteLine(" 2) Placer de l’huile d’eucalyptus !");
        Console.WriteLine(" 3) Musique à fond !");
        Console.WriteLine(" 4) Clou de girofle !");
        Console.WriteLine();

        int choix = 0;
        int timeout = 10;                       // secondes
        DateTime fin = DateTime.Now.AddSeconds(timeout);

        while (DateTime.Now < fin && choix == 0)
        {
            // Affiche le temps restant sur une seule ligne
            int reste = (int)Math.Ceiling((fin - DateTime.Now).TotalSeconds);
            Console.Write($"\rTemps restant : {reste} s  "); // overwrite

            if (Console.KeyAvailable)
            {
                choix = Console.ReadKey(true).Key switch
                {
                    ConsoleKey.D1 or ConsoleKey.NumPad1 => 1,
                    ConsoleKey.D2 or ConsoleKey.NumPad2 => 2,
                    ConsoleKey.D3 or ConsoleKey.NumPad3 => 3,
                    ConsoleKey.D4 or ConsoleKey.NumPad4 => 4,
                    _ => 0
                };
            }
            Thread.Sleep(40);
        }
        Console.WriteLine(); // retour à la ligne après la barre

        if (choix == intrus.rep)
        {
            Console.WriteLine("\nExcellent choix ! L’intrus senfuit sans faire de dégâts.");
            Pause();
            return true;
        }

        if (choix == 0)             // aucun choix avant la fin du temps
            Console.WriteLine("\nTrop tard ! Vous n’avez pas réagi à temps…");
        else
            Console.WriteLine("\nAïe. Mauvais Répulsif. L'intrus saccage une plante...");

        TuerPlanteAleatoire();
        Pause();
        return false;
    }

    // ------------------------------------------------------------------
    private void TuerPlanteAleatoire()
    {
        // Récupère toutes les coordonnées de plantes vivantes
        var vivantes = new List<(int x, int y)>();
        int h = _plateau.Hauteur;          // propriétés déjà présentes dans VuePotager
        int w = _plateau.Largeur;

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                var p = _plateau.GetTerrain(x, y).Plante;
                if (p != null && !p.EstMorte)
                    vivantes.Add((x, y));
            }

        if (vivantes.Count == 0) return;   // rien à tuer

        var (xSel, ySel) = vivantes[_rng.Next(vivantes.Count)];
        var terrain = _plateau.GetTerrain(xSel, ySel);
        var plante = terrain.Plante;
        if (plante != null)   // test null explicite
            plante.Tuer();
    }


    private static void Pause()
    {
        Console.WriteLine("\n\nApuyez sur une touche pour reprendre la partie.");
        Console.ReadKey(true);
    }
}
