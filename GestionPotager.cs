using System;
public class GestionPotager
{
    private VuePotager view;
    private GestionPlateau plateauController;
    private Terrain[,] plateau;

    public Saison saisonActuelle;
    private int jourActuel = 1;
    private bool simulationEnCours = true;

    public void DemarrerSimulation(int largeurPlateau, int hauteurPlateau)
    {
        plateau = GenerateurBiome.GenererPlateau(largeurPlateau, hauteurPlateau);
        view = new VuePotager(plateau, plateauController);
        plateauController = new GestionPlateau(plateau, view, this);

        saisonActuelle = new SaisonPluvieuse();

        LancerModeClassique();
    }
    public void ArreterSimulation()
    {
        simulationEnCours = false;
    }

    private void LancerModeClassique()
{
    bool continuer = true;
    while (continuer && simulationEnCours)
    {
        Meteo meteoDuJour = Meteo.GenererPourSaison(saisonActuelle, jourActuel);
        view.SetMeteo(meteoDuJour); // Passer la météo à la vue

        Console.WriteLine($"\n----- Jour {jourActuel} -----");

        for (int y = 0; y < plateau.GetLength(0); y++)
        {
            for (int x = 0; x < plateau.GetLength(1); x++)
            {
                if (plateau[y, x].Plante != null)
                {
                    bool espaceOk = plateauController.CheckEspaceRespecte(x, y); // Utiliser la méthode de GestionPlateau
                    plateau[y, x].Plante.Update(1f, meteoDuJour.Temperature, espaceOk);
                }
            }
        }

        view.AfficherPlateau();
        plateauController.GererEntreesUtilisateurModeClassique();

        if (!simulationEnCours) break;

        Console.WriteLine("\nAppuyez à nouveau pour confirmer votre choix | R: Retour");

        ConsoleKeyInfo touche = Console.ReadKey(true);
        switch (touche.Key)
        {
            case ConsoleKey.E:
                jourActuel++;
                break;
            case ConsoleKey.Q:
                continuer = false;
                break;
            case ConsoleKey.R:
                break; 
        }

        if (jourActuel > 30)
        {
            continuer = false;
            Console.WriteLine("\nFin de la simulation classique.");
        }
    }
}
}