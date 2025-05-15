public enum MenuOption
{
    NouvellePartie,
    ChargerSauvegarde,
    ReglesDuJeu,
    Quitter
}

public static class StartMenu
{
    private static readonly string[] _asciiArt = new[]
    {
            " ######    #####   ######     ##       ####   #######  ######            ######   ######   #######   #####    ####    ####",
            "  ##  ##  ##   ##  # ## #    ####     ##  ##   ##   #   ##  ##            ##  ##   ##  ##   ##   #  ##   ##    ##      ##",
            "  ##  ##  ##   ##    ##     ##  ##   ##        ## #     ##  ##            ##  ##   ##  ##   ## #    #          ##      ##",
            "  #####   ##   ##    ##     ##  ##   ##        ####     #####             #####    #####    ####     #####     ##      ##",
            "  ##      ##   ##    ##     ######   ##  ###   ## #     ## ##             ##  ##   ## ##    ## #         ##    ##      ##   #",
            "  ##      ##   ##    ##     ##  ##    ##  ##   ##   #   ##  ##            ##  ##   ##  ##   ##   #  ##   ##    ##      ##  ##",
            " ####      #####    ####    ##  ##     #####  #######  #### ##           ######   #### ##  #######   #####    ####    #######",
            ""
        };

    private static readonly string[] _options = new[]
    {
            "Nouvelle partie",
            "Charger une sauvegarde",
            "Règles du jeu",
            "Quitter"
        };

    public static MenuOption Show()
    {
        int current = 0;

        ConsoleKey key;
        do
        {
            Console.Clear();

            // Affiche l'ASCII-art
            foreach (var line in _asciiArt)
                Console.WriteLine(line);

            Console.WriteLine(); // espace

            // Affiche les options
            for (int i = 0; i < _options.Length; i++)
            {
                if (i == current)
                {
                    // option sélectionnée : fond cyan, texte noir
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($" → {_options[i]}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"   {_options[i]}");
                }
            }

            // Lecture de la touche utilisateur
            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow)
            {
                current = (current - 1 + _options.Length) % _options.Length;
            }
            else if (key == ConsoleKey.DownArrow)
            {
                current = (current + 1) % _options.Length;
            }

        } while (key != ConsoleKey.Enter);

        return (MenuOption)current;
    }
}