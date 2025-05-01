using System;
using System.Collections.Generic;

public class GestionPlateau
{
    private Terrain[,] plateau;
    private VuePotager view;
    private GestionPotager gestionPotager;
    private bool interactionEnCours = false;
    private Dictionary<(int x, int y), Plante> positionsDesPlantes = new Dictionary<(int x, int y), Plante>();


    public GestionPlateau(Terrain[,] plateauInitial, VuePotager viewInitial, GestionPotager gestionPotager)
    {
        plateau = plateauInitial;
        view = viewInitial;
        this.gestionPotager = gestionPotager;
        MettreAJourPositionsDesPlantes();
    }

    public void GererEntreesUtilisateurModeClassique()
    {

        bool actionEffectuee = false;
        bool quitter = false;
        while (!actionEffectuee && !quitter)
        {
            view.AfficherPlateau(); // Affiche la vue principale
            Console.WriteLine("\nEspace - Action | I - Information | E - Jour suivant | Q - Quitter");
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
                case ConsoleKey.I:
                    AfficherInfo();
                    break;
                case ConsoleKey.E:
                    actionEffectuee = true; // Passer au jour suivant
                    break;
                case ConsoleKey.Q:
                    quitter = true;
                    gestionPotager.ArreterSimulation();
                    break;
            }
        }
    }
    private void AfficherInfo()
    {
        Terrain terrain = plateau[view.CurseurY, view.CurseurX];
        view.AfficherPlanteOuTerrain(terrain, view.CurseurX, view.CurseurY  );

        bool actionChoisie = false;
        while (!actionChoisie)
        {
            ConsoleKeyInfo actionTouche = Console.ReadKey(true);

            if (actionTouche.Key == ConsoleKey.R)
            {
                actionChoisie = true;
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
                        terrain.Plante.Arroser();
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
    public bool CheckEspaceRespecte(int x, int y)
    {
        Plante? plante = plateau[y, x].Plante;
        if (plante == null || plante.EspacePris <= 1) return true;

        int rayon;
        if (plante.EspacePris == 1)
        {
            rayon = 0;
        }
        else if (plante.EspacePris == 2)
        {
            rayon = 1;
        }
        else if (plante.EspacePris >= 3)
        {
            rayon = 2;
        }
        else
        {
            return false;
        }


        Console.WriteLine($"[DEBUG ESPACEMENT] Vérification pour plante en ({x}, {y}), EspacePris: {plante.EspacePris}, Rayon: {rayon}");

        for (int i = y - rayon; i <= y + rayon; i++)
        {
            for (int j = x - rayon; j <= x + rayon; j++)
            {
                if ((i == y && j == x) || i < 0 || i >= plateau.GetLength(0) || j < 0 || j >= plateau.GetLength(1))
                    continue;
                Console.WriteLine($"[DEBUG ESPACEMENT] Vérification voisine en ({j}, {i}), Plante présente: {plateau[i, j].Plante != null}");
                if (plateau[i, j].Plante != null)
                {
                    Console.WriteLine($"[DEBUG ESPACEMENT] Plante voisine trouvée: {plateau[i, j].Plante.NomPlante} en ({j}, {i})");
                    return false;
                }
            }
        }
        return true;
    }

    public void MettreAJourPotager(Meteo meteoActuelle)
    {
        for (int y = 0; y < plateau.GetLength(0); y++)
        {
            for (int x = 0; x < plateau.GetLength(1); x++)
            {
                Plante? planteActuelle = plateau[y, x].Plante;
                if (planteActuelle != null)
                {
                    bool espaceOk = CheckEspaceRespecte(x, y);
                    planteActuelle.Update(1f, meteoActuelle.Temperature, espaceOk);

                    // Mettre à jour le dictionnaire si la plante existe et à la bonne position
                    if (positionsDesPlantes.ContainsKey((x, y)) && positionsDesPlantes[(x, y)] != planteActuelle)
                    {
                        positionsDesPlantes[(x, y)] = planteActuelle;
                    }
                    else if (!positionsDesPlantes.ContainsKey((x, y)))
                    {
                        positionsDesPlantes.Add((x, y), planteActuelle);
                    }
                }
                else if (positionsDesPlantes.ContainsKey((x, y))) // Si une plante était là mais n'est plus
                {
                    positionsDesPlantes.Remove((x, y));
                }
            }
        }
    }
     private void MettreAJourPositionsDesPlantes()
    {
        positionsDesPlantes.Clear();
        for (int y = 0; y < plateau.GetLength(0); y++)
        {
            for (int x = 0; x < plateau.GetLength(1); x++)
            {
                if (plateau[y, x].Plante != null)
                {
                    positionsDesPlantes.Add((x, y), plateau[y, x].Plante);
                }
            }
        }
    }
    public void SetVuePotager(VuePotager vue)
    {
        view = vue;
    }

}