public class Simulation
{
    public double Vitesse { get; set; }
    public Simulation(double vitesse)
    {
        Vitesse = vitesse;
    }

    public void Simuler(string[,] plateau)
    {
        int longueur = plateau.GetLength(0);
        int largeur = plateau.GetLength(1);

        bool conditionPlante = false;
        int posX = 0;
        int posY = 0;
        bool quitter = false;

        while (!quitter)
        {
            for (int time = 0; time < 10; time++)
            {
                Console.Clear();
                Console.WriteLine("\nIl potagio :\n");

                // Affichage du plateau avec curseur
                for (int i = 0; i < longueur; i++)
                {
                    for (int j = 0; j < largeur; j++)
                    {
                        string caseContenue = plateau[i, j];
                        ConsoleColor couleur = ConsoleColor.Gray;

                        // Couleur selon contenu
                        switch (caseContenue)
                        {
                            case "E": couleur = ConsoleColor.Blue; break;
                            case "T": couleur = ConsoleColor.Green; break;
                            case "P": 
                            if(time%2 ==1 && conditionPlante == false)
                            {
                                couleur = ConsoleColor.Gray;
                            }
                            else
                            {
                                couleur = ConsoleColor.DarkGreen;
                            }
                            break;
                            case "S": couleur = ConsoleColor.Yellow; break;
                        }

                        // Curseur actif ?
                        if (i == posY && j == posX)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.ForegroundColor = couleur;
                        }

                        Console.Write($"[{caseContenue}]");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }

                // Instructions
                Console.WriteLine("\nUtilise les flèches pour déplacer le curseur.");
                Console.WriteLine("Appuie sur Entrée pour sélectionner une case.");
                Console.WriteLine("Appuie sur 'A' pour quitter.");

                // Lire la touche
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (posY > 0) posY--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (posY < longueur - 1) posY++;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (posX > 0) posX--;
                        break;
                    case ConsoleKey.RightArrow:
                        if (posX < largeur - 1) posX++;
                        break;
                    case ConsoleKey.Enter:
                        Console.WriteLine($"\nTu as sélectionné la case en ({posY}, {posX}) contenant : {plateau[posY, posX]}");
                        Console.WriteLine("Appuie sur une touche pour continuer...");
                        Console.ReadKey(true);
                        break;
                    case ConsoleKey.A:
                        quitter = true;
                        break;
                }
            }
        }
    }
}
