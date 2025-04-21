using System.Collections.Generic;
using System.Threading;

public class GestionnairePotager
{
    private int cursorX = 0;
    private int cursorY = 0;
    private bool showPlants = true;
    private DateTime lastBlinkTime = DateTime.Now;

    public void AfficherPlateau(Terrain[,] plateau)
    {
        bool quitter = false;

        while (!quitter)
        {
            if ((DateTime.Now - lastBlinkTime).TotalMilliseconds > 500)
            {
                showPlants = !showPlants;
                lastBlinkTime = DateTime.Now;
            }

            Console.Clear();
            Console.WriteLine("=== POTAGER VU DU CIEL ===");
            Console.WriteLine("Flèches: Se déplacer | Espace: Interagir | Q: Quitter\n");

            for (int y = 0; y < plateau.GetLength(0); y++)
            {
                for (int x = 0; x < plateau.GetLength(1); x++)
                {
                    bool estCurseur = (x == cursorX && y == cursorY);
                    var terrain = plateau[y, x];

                    Console.BackgroundColor = estCurseur ? ConsoleColor.DarkCyan : ConsoleColor.Black;
                    Console.ForegroundColor = terrain.Couleur;
                    Console.Write("[");

                    if (terrain.Plante != null && (showPlants || !terrain.Plante.SanteCritique))
                    {
                        Console.ForegroundColor = terrain.Plante.SanteCritique
                            ? ConsoleColor.Red
                            : ConsoleColor.DarkBlue;
                        Console.Write(terrain.Plante.Acronyme);
                    }
                    else
                    {
                        Console.Write("  ");
                    }

                    Console.ForegroundColor = terrain.Couleur;
                    Console.Write("]");
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            AfficherInfosCase(plateau[cursorY, cursorX]);

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow when cursorY > 0: cursorY--; break;
                case ConsoleKey.DownArrow when cursorY < plateau.GetLength(0) - 1: cursorY++; break;
                case ConsoleKey.LeftArrow when cursorX > 0: cursorX--; break;
                case ConsoleKey.RightArrow when cursorX < plateau.GetLength(1) - 1: cursorX++; break;
                case ConsoleKey.Spacebar: Interagir(plateau[cursorY, cursorX]); break;
                case ConsoleKey.Q: quitter = true; break;
            }
        }
    }

    private void AfficherInfosCase(Terrain terrain)
    {
        Console.WriteLine("\n=== CASE SÉLECTIONNÉE ===");
        Console.WriteLine($"Position: ({cursorX}, {cursorY})");
        Console.WriteLine($"Type: {terrain.NomTerrain}");

        if (terrain.Plante != null)
        {
            Console.WriteLine($"\nPlante: {terrain.Plante.NomPlante}");
            Console.WriteLine($"Santé: {(terrain.Plante.SanteCritique ? "CRITIQUE" : "Bonne")}");
            Console.WriteLine("\nActions disponibles:");
            Console.WriteLine("A - Arroser | D - Désherber | R - Récolter");
        }
        else
        {
            Console.WriteLine("\nAucune plante");
            Console.WriteLine("\nAction: P - Planter");
        }
    }

    private void Interagir(Terrain terrain)
    {
        Console.Write("\nChoisir une action: ");
        var key = Console.ReadKey(true).Key;

        if (terrain.Plante == null)
        {
            if (key == ConsoleKey.P)
                terrain.Plante = ChoixNouvellePlante();
        }
        else
        {
            switch (key)
            {
                case ConsoleKey.A:
                    terrain.Plante.Arroser(10);
                    Console.WriteLine("\nArrosage effectué !");
                    break;
                case ConsoleKey.D:
                    terrain.Plante.Desherber();
                    Console.WriteLine("\nDésherbage effectué !");
                    break;
                case ConsoleKey.R:
                    terrain.Plante = null;
                    Console.WriteLine("\nRécolte effectuée !");
                    break;
            }
            Thread.Sleep(800);
        }
    }

    private Plante ChoixNouvellePlante()
    {
        Console.Clear();
        Console.WriteLine("Choisissez une plante :");
        Console.WriteLine("1 - Soja (So)");
        Console.WriteLine("2 - Maïs (Ma)");
        Console.WriteLine("3 - Coton (Co)");
        Console.WriteLine("4 - Canne à sucre (Ca)");
        Console.WriteLine("5 - Café (Cf)");
        Console.WriteLine("6 - Cactus (Cx)");

        Console.Write("\nVotre choix : ");

        return Console.ReadKey(true).Key switch
        {
            ConsoleKey.D1 or ConsoleKey.NumPad1 => new Soja(),
            ConsoleKey.D2 or ConsoleKey.NumPad2 => new Mais(),
            ConsoleKey.D3 or ConsoleKey.NumPad3 => new Coton(),
            ConsoleKey.D4 or ConsoleKey.NumPad4 => new CanneASucre(),
            ConsoleKey.D5 or ConsoleKey.NumPad5 => new Cafe(),
            ConsoleKey.D6 or ConsoleKey.NumPad6 => new Cactus(),
            _ => null
        };
    }
}