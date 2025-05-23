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
        " ██████    █████   ██████     ██       ████   ███████  ██████            ██████   ██████   ███████   █████    ████    ████",
        "  ██  ██  ██   ██  █ ██ █    ████     ██  ██   ██   █   ██  ██            ██  ██   ██  ██   ██   █  ██   ██    ██      ██",
        "  ██  ██  ██   ██    ██     ██  ██   ██        ██ █     ██  ██            ██  ██   ██  ██   ██ █    █          ██      ██",
        "  █████   ██   ██    ██     ██  ██   ██        ████     █████             █████    █████    ████     █████     ██      ██",
        "  ██      ██   ██    ██     ██████   ██  ███   ██ █     ██ ██             ██  ██   ██ ██    ██ █         ██    ██      ██   █",
        "  ██      ██   ██    ██     ██  ██    ██  ██   ██   █   ██  ██            ██  ██   ██  ██   ██   █  ██   ██    ██      ██  ██",
        " ████      █████    ████    ██  ██     █████  ███████  ████ ██           ██████   ████ ██  ███████   █████    ████    ███████",
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
            {
                Console.WriteLine("  " + line);
            }

            Console.WriteLine();

            // Affiche les options avec sélection en rouge
            for (int i = 0; i < _options.Length; i++)
            {
                string optionText = (i == current ? "→ " : "  ") + _options[i];
                Console.ForegroundColor = i == current ? ConsoleColor.Red : ConsoleColor.White;
                Console.WriteLine(optionText);
                Console.ResetColor();
            }

            Console.WriteLine();

            // Drapeau du Brésil ajusté
            string[] drapeau = new[]
            {
                new string('X', 18) + new string('█', 24),
                new string('X', 11) + new string('█', 13) + new string('▒', 6) + new string('█', 21),
                new string('X', 6) + new string('█', 13) + new string('▒', 14) + new string('█', 27),
                new string('X', 4) + new string('█', 12) + new string('▒', 22) + new string('█', 13),
                new string('X', 3) + new string('█', 11) + new string('▒', 8) + new string('▓', 10) + new string('▒', 8) + new string('█', 29),
                new string('X', 2) + new string('█', 10) + new string('▒', 8) + new string('▓', 14) + new string('▒', 8) + new string('█', 13),
                new string('X', 2) + new string('█', 8) + new string('▒', 8) + new string('░', 4) + new string('▓', 14) + new string('▒', 8) + new string('█', 31),
                new string('X', 1) + new string('█', 7) + new string('▒', 10) + new string('▓', 2) + new string('░', 4) + new string('▓', 12) + new string('▒', 10) + new string('█', 8),
                new string('X', 1) + new string('█', 5) + new string('▒', 12) + new string('▓', 4) + new string('░', 8) + new string('▓', 6) + new string('▒', 12) + new string('█', 24),
                new string('X', 1) + new string('█', 5) + new string('▒', 12) + new string('▓', 8) + new string('░', 8) + new string('▓', 2) + new string('▒', 12) + new string('█', 45),
                new string('X', 1) + new string('█', 7) + new string('▒', 10) + new string('▓', 14) + new string('░', 4) + new string('▒', 10) + new string('█', 13),
                new string('X', 2) + new string('█', 8) + new string('▒', 10) + new string('▓', 14) + new string('▒', 10) + new string('█', 36),
                new string('X', 2) + new string('█', 10) + new string('▒', 10) + new string('▓', 10) + new string('▒', 10) + new string('█', 14),
                new string('X', 2) + new string('█', 12) + new string('▒', 26) + new string('█', 20),
                new string('X', 3) + new string('█', 15) + new string('▒', 18) + new string('█', 18),
                new string('X', 5) + new string('█', 17) + new string('▒', 10) + new string('█', 18),
                new string('X', 8) + new string('█', 16) + new string('▒', 6) + new string('█', 18),
                new string('X', 13) + new string('█', 28),
            };

            foreach (var line in drapeau)
            {
                foreach (char c in line)
                {
                    Console.ForegroundColor = c == '█' ? ConsoleColor.Green
                       : c == '▒' ? ConsoleColor.Yellow
                       : c == '▓' ? ConsoleColor.Blue
                       : c == '░' ? ConsoleColor.White
                       : ConsoleColor.Black;
                       Console.Write("█");
                }
                Console.ResetColor();
                Console.WriteLine();
            }

            key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.UpArrow)
                current = (current - 1 + _options.Length) % _options.Length;
            else if (key == ConsoleKey.DownArrow)
                current = (current + 1) % _options.Length;

        } while (key != ConsoleKey.Enter);

        return (MenuOption)current;
    }
}