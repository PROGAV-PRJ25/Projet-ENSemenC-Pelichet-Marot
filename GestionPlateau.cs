using System;
using System.Collections.Generic;

public class GestionPlateau
{
    private Terrain[,] plateau;
    private VuePotager view;
     private GestionPotager gestionPotager;
    private bool interactionEnCours = false; 

    public GestionPlateau(Terrain[,] plateauInitial, VuePotager viewInitial, GestionPotager gestionPotager)
    {
        plateau = plateauInitial;
        view = viewInitial;
        this.gestionPotager = gestionPotager;
    }

    public void GererEntreesUtilisateur()
    {
        bool quitter = false;

        while (!quitter)
        {
            view.AfficherPlateau();
            ConsoleKeyInfo touche = Console.ReadKey(true);

            switch (touche.Key)
            {
                case ConsoleKey.UpArrow:
                    DeplacerCurseur(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    DeplacerCurseur(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    DeplacerCurseur(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    DeplacerCurseur(1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    interactionEnCours = true; // Démarre l'interaction
                    GererInteraction();
                    interactionEnCours = false; // Termine l'interaction
                    break;
                case ConsoleKey.Q:
                    quitter = true;
                    break;
            }
        }
    }

    private void GererInteraction()
    {
        Terrain terrain = plateau[view.CurseurY, view.CurseurX];
        view.AfficherActionsCase(terrain); // Affiche les actions spécifiques

        bool actionChoisie = false;
        while (!actionChoisie)
        {
            ConsoleKeyInfo actionTouche = Console.ReadKey(true);
            switch (actionTouche.Key)
            {
                case ConsoleKey.A:
                    if (terrain.Plante != null)
                    {
                        terrain.Plante.Arroser(200);
                        Console.WriteLine("\nArrosage effectué !");
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.D:
                    if (terrain.Plante != null)
                    {
                        terrain.Plante.Desherber();
                        Console.WriteLine("\nDésherbage effectué !");
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.R:
                    if (terrain.Plante != null)
                    {
                        terrain.Plante = null;
                        Console.WriteLine("\nRécolte effectuée !");
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.P:
                    if (terrain.Plante == null)
                    {
                        terrain.Plante = ChoisirNouvellePlante();
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.Spacebar: // Retour à la vue du potager
                    actionChoisie = true;
                    break;
            }
            if (actionChoisie) System.Threading.Thread.Sleep(500);
        }
    }

    public void GererEntreesUtilisateurModeClassique()
    {
        
        bool actionEffectuee = false;
        bool quitter = false;
        while (!actionEffectuee && !quitter)
        {
            view.AfficherPlateau(); // Affiche la vue principale
            Console.WriteLine("\nAppuyez sur Espace pour interagir avec la parcelle sélectionnée.");
            ConsoleKeyInfo touche = Console.ReadKey(true);

            switch (touche.Key)
            {
                case ConsoleKey.UpArrow:
                    DeplacerCurseur(0, -1);
                    break;
                case ConsoleKey.DownArrow:
                    DeplacerCurseur(0, 1);
                    break;
                case ConsoleKey.LeftArrow:
                    DeplacerCurseur(-1, 0);
                    break;
                case ConsoleKey.RightArrow:
                    DeplacerCurseur(1, 0);
                    break;
                case ConsoleKey.Spacebar:
                    interactionEnCours = true;
                    GererInteractionModeClassique(); // Nouvelle méthode pour les interactions en mode classique
                    interactionEnCours = false;
                    break;
                case ConsoleKey.E:
                    actionEffectuee = true; // Passer au jour suivant
                    break;
                case ConsoleKey.Q:
                    quitter = true;
                    break; 
            }
        }
    }

    private void GererInteractionModeClassique()
    {
        Terrain terrain = plateau[view.CurseurY, view.CurseurX];
        view.AfficherActionsCase(terrain);

        bool actionChoisie = false;
        while (!actionChoisie)
        {
            ConsoleKeyInfo actionTouche = Console.ReadKey(true);
            switch (actionTouche.Key)
            {
                case ConsoleKey.A:
                    if (terrain.Plante != null)
                    {
                        terrain.Plante.Arroser(200);
                        Console.WriteLine("\nArrosage effectué !");
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.D:
                    if (terrain.Plante != null)
                    {
                        Console.WriteLine($"\n{terrain.Plante.NomPlante} a été désherbé.");
                        terrain.Plante.Desherber(); // Appel de la méthode Desherber de la plante (si tu veux y mettre une logique spécifique)
                        terrain.Plante = null; // Suppression de la plante de la case !
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.R:
                    if (terrain.Plante != null)
                    {
                        terrain.Plante = null;
                        Console.WriteLine("\nRécolte effectuée !");
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.P:
                    if (terrain.Plante == null)
                    {
                        terrain.Plante = ChoisirNouvellePlante();
                    }
                    actionChoisie = true;
                    break;
                case ConsoleKey.Spacebar:
                    actionChoisie = true; // Retour à la vue du potager
                    break;
            }
            if (actionChoisie) System.Threading.Thread.Sleep(500);
        }
    }


    private void DeplacerCurseur(int deltaX, int deltaY)
    {
        int nouvelleX = view.CurseurX + deltaX;
        int nouvelleY = view.CurseurY + deltaY;

        if (nouvelleX >= 0 && nouvelleX < plateau.GetLength(1) &&
            nouvelleY >= 0 && nouvelleY < plateau.GetLength(0))
        {
            view.CurseurX = nouvelleX;
            view.CurseurY = nouvelleY;
        }
    }

    private void InteragirAvecCase()
    {
        // Cette méthode n'est plus directement appelée en mode classique
    }

    private Plante ChoisirNouvellePlante()
    {
        Console.Clear();
        Console.WriteLine("Choisissez une plante :");
        Console.WriteLine("1 - Soja (So)");
        Console.WriteLine("2 - Maïs (Ma)");
        Console.WriteLine("3 - Canne à sucre (Cs)");
        Console.WriteLine("4 - Café (Cf)");
        Console.WriteLine("5 - Cactus (Ca)");
        Console.WriteLine("6 - Coton (Co)");
        Console.WriteLine("0 - Rien");

        Console.Write("\nVotre choix : ");
        string choix = Console.ReadLine();

        switch (choix)
        {
            case "1": return new Soja();
            case "2": return new Mais();
            case "3": return new CanneASucre();
            case "4": return new Cafe();
            case "5": return new Cactus();
            case "6": return new Coton();
            case "0": return null;
            default:
                Console.WriteLine("Choix invalide. Veuillez réessayer.");
                System.Threading.Thread.Sleep(1000);
                return ChoisirNouvellePlante();
        }
    }

}