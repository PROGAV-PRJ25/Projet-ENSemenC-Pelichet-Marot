using System.Diagnostics;
public class NettoyagePotagerGame : MiniJeu
{
    private readonly Graines _graines;
    private readonly Random  _rng = new();

    public NettoyagePotagerGame(Graines graines)
    {
        _graines = graines;
    }

    public bool Run()
    {
        const int longueur = 15;
        var directions = new[] {
            ConsoleKey.UpArrow, ConsoleKey.DownArrow,
            ConsoleKey.LeftArrow, ConsoleKey.RightArrow
        };
        var sequence = Enumerable.Range(0, longueur)
            .Select(_ => directions[_rng.Next(directions.Length)])
            .ToArray();

        // Durée max du mini-jeu
        TimeSpan dureeMax = TimeSpan.FromSeconds(10);
        var sw = Stopwatch.StartNew();

        // Écran d’intro
        Console.Clear();
        Console.WriteLine("!!! MODE URGENCE : NETTOYAGE DES MAUVAISES HERBES !!!\n");
        Console.WriteLine("Répétez la séquence de flèches en moins de 10 s.");
        Thread.Sleep(3000);

        for (int i = 0; i < longueur; i++)
        {
            // 1) Test du temps
            if (sw.Elapsed > dureeMax)
            {
                Console.Clear();
                Console.WriteLine("⏱️  Temps écoulé ! Échec.");
                Thread.Sleep(2000);
                return false;
            }

            // 2) Affichage « MODE URGENCE » + compte à rebours
            Console.Clear();
            Console.WriteLine("!!! MODE URGENCE : NETTOYAGE DES MAUVAISES HERBES !!!\n");
            int reste = (int)(dureeMax - sw.Elapsed).TotalSeconds;
            Console.WriteLine($"Temps restant : {reste,2}s\n");

            // 3) Affichage des poubelles pour le « nettoyage »
            string poubelles = string.Concat(Enumerable.Repeat("🌿 ", longueur - i));
            Console.WriteLine(poubelles + "\n");

            // 4) Affichage de la flèche à taper
            Console.WriteLine($"Étape {i+1}/{longueur}  :  {KeyToSym(sequence[i])}");

            // 5) Lecture de la touche
            var key = Console.ReadKey(true).Key;
            if (key != sequence[i])
            {
                Console.WriteLine("\n❌ Mauvaise flèche : Échec du nettoyage.");
                Thread.Sleep(2000);
                return false;
            }
        }

        // Si on arrive ici, toutes les flèches ont été saisies dans le temps
        Console.WriteLine("\n✅ Nettoyage réussi ! +10 graines");
        _graines.Ajouter(10);
        Thread.Sleep(2000);
        return true;
    }

    private static string KeyToSym(ConsoleKey k) => k switch {
        ConsoleKey.UpArrow    => "↑",
        ConsoleKey.DownArrow  => "↓",
        ConsoleKey.LeftArrow  => "←",
        ConsoleKey.RightArrow => "→",
        _                     => "?"
    };
}
