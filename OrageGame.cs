using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

public class OrageGame : IMiniJeu
{
    private readonly GestionPlateau _controller;
    private readonly VuePotager _vue;
    private readonly Graines _graines;
    private readonly TimeSpan _timeout = TimeSpan.FromSeconds(10);

    public OrageGame(GestionPlateau controller, VuePotager vue, Graines graines)
    {
        _controller = controller;
        _vue = vue;
        _graines = graines;
    }

    public bool Run()
{
    Console.Clear();
    Console.WriteLine("Gérer un gros orage : posez la bâche sur 5 plantes en 10 s !");
    Thread.Sleep(1000);

    // 1) Récupère jusqu'à 5 plantes existantes
    var targets = _controller.Choisir5PlantesAleatoires();

    // 2) Complète à 5 avec des cases aléatoires si nécessaire
    var rnd = new Random();
    int h = _vue.Hauteur, w = _vue.Largeur;
    while (targets.Count < 5)
    {
        var cand = (x: rnd.Next(w), y: rnd.Next(h));
        if (!targets.Contains(cand))
            targets.Add(cand);
    }

    // 3) Vide le buffer clavier
    while (Console.KeyAvailable) Console.ReadKey(true);

    // 4) Boucle non bloquante
    var sw      = Stopwatch.StartNew();
    var proteges = new HashSet<(int x, int y)>();
    var cursor   = (x: 0, y: 0);

    while (sw.Elapsed.TotalSeconds < 10 && proteges.Count < targets.Count)
    {
        Console.Clear();
        _vue.AfficherPotagerMasque(targets, proteges, cursor);
        Console.WriteLine(
            $"\nProtégé : {proteges.Count}/{targets.Count}  " +
            $"Temps restant : {(int)(10 - sw.Elapsed.TotalSeconds)}s"
        );

        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (cursor.y > 0) cursor.y--;
                    break;
                case ConsoleKey.DownArrow:
                    if (cursor.y < h - 1) cursor.y++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (cursor.x > 0) cursor.x--;
                    break;
                case ConsoleKey.RightArrow:
                    if (cursor.x < w - 1) cursor.x++;
                    break;
                case ConsoleKey.Spacebar:
                    // Si on est sur une cible non encore protégée
                    if (targets.Contains(cursor) && !proteges.Contains(cursor))
                    {
                        proteges.Add(cursor);
                    }
                    break;
            }
        }
        else
        {
            // petite pause pour ne pas boucler à fond
            Thread.Sleep(50);
        }
    }

    sw.Stop();
    bool win = proteges.Count == targets.Count;

    // Affichage final des cases protégées
    Console.Clear();
    // on ré-affiche avec toutes les protections posées 
    _vue.AfficherPotagerMasque(targets, proteges, cursor);
    Thread.Sleep(500);  

    if (win)
    {
        Console.WriteLine("\n✅ Orage contenu ! +20 graines");
        _graines.Ajouter(20);
    }
    else
    {
        Console.WriteLine("\n❌ Trop tard ! -10 graines");
        _graines.Depenser(10);
    }

    Thread.Sleep(2000);
    return win;
}


}
