using System;

public class GestionPlateau
{
    // Le plateau de terrains
    private Terrain[,] plateau;

    // La vue pour afficher le plateau
    private VuePotager view;

    // Constructeur : on a besoin du plateau et de la vue pour fonctionner
    public GestionPlateau(Terrain[,] plateauInitial, VuePotager viewInitial)
    {
        plateau = plateauInitial;
        view = viewInitial;
    }

    // Méthode principale : gère les entrées du joueur et les actions
    public void GererEntreesUtilisateur()
    {
        bool quitter = false;

        // Boucle principale du jeu
        while (!quitter)
        {
            // Affiche le plateau
            view.AfficherPlateau();

            // Lit l'entrée du joueur
            ConsoleKeyInfo touche = Console.ReadKey(true);

            // Traite l'entrée du joueur
            switch (touche.Key)
            {
                case ConsoleKey.UpArrow:
                    DeplacerCurseur(0, -1); // Déplace le curseur vers le haut
                    break;
                case ConsoleKey.DownArrow:
                    DeplacerCurseur(0, 1);  // Déplace le curseur vers le bas
                    break;
                case ConsoleKey.LeftArrow:
                    DeplacerCurseur(-1, 0); // Déplace le curseur vers la gauche
                    break;
                case ConsoleKey.RightArrow:
                    DeplacerCurseur(1, 0);  // Déplace le curseur vers la droite
                    break;
                case ConsoleKey.Spacebar:
                    InteragirAvecCase();    // Interagit avec la case sélectionnée
                    break;
                case ConsoleKey.Q:
                    quitter = true;         // Quitte le jeu
                    break;
            }
        }
    }

    // Méthode pour déplacer le curseur
    private void DeplacerCurseur(int deltaX, int deltaY)
    {
        // Calcule la nouvelle position du curseur
        int nouvelleX = view.CurseurX + deltaX;
        int nouvelleY = view.CurseurY + deltaY;

        // Vérifie si la nouvelle position est valide (à l'intérieur du plateau)
        if (nouvelleX >= 0 && nouvelleX < plateau.GetLength(1) &&
            nouvelleY >= 0 && nouvelleY < plateau.GetLength(0))
        {
            view.CurseurX = nouvelleX; // Met à jour la position du curseur dans la vue
            view.CurseurY = nouvelleY;
        }
    }

    // Méthode pour gérer l'interaction avec la case sélectionnée
    private void InteragirAvecCase()
    {
        Terrain terrain = plateau[view.CurseurY, view.CurseurX]; // Obtient le terrain sélectionné

        Console.Write("\nChoisir une action: ");
        ConsoleKeyInfo touche = Console.ReadKey(true);

        // Si la case n'a pas de plante, on propose de planter
        if (terrain.Plante == null)
        {
            if (touche.Key == ConsoleKey.P)
            {
                terrain.Plante = ChoisirNouvellePlante(); // Le joueur choisit une plante à planter
            }
        }
        else // Sinon, on propose les actions sur la plante
        {
            switch (touche.Key)
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
            System.Threading.Thread.Sleep(800); // Petite pause pour que le joueur voie le message
        }
    }

    // Méthode pour que le joueur choisisse une plante à planter
    private Plante ChoisirNouvellePlante()
    {
        Console.Clear();
        Console.WriteLine("Choisissez une plante :");
        Console.WriteLine("1 - Soja (So)");
        Console.WriteLine("2 - Maïs (Ma)");
        Console.WriteLine("3 - Coton (Co)");
        Console.WriteLine("4 - Canne à sucre (Ca)");
        Console.WriteLine("5 - Café (Cf)");
        Console.WriteLine("6 - Cactus (Cx)");
        Console.WriteLine("0 - Annuler");

        Console.Write("\nVotre choix : ");
        string choix = Console.ReadLine();

        // Crée la plante choisie ou retourne null si le choix est invalide
        switch (choix)
        {
            case "1": return new Soja();
            case "2": return new Mais();
            case "3": return new Coton();
            case "4": return new CanneASucre();
            case "5": return new Cafe();
            case "6": return new Cactus();
            case "0": return null;
            default:
                Console.WriteLine("Choix invalide. Veuillez réessayer.");
                System.Threading.Thread.Sleep(1000);
                return ChoisirNouvellePlante(); // Récursion pour réessayer
        }
    }
}