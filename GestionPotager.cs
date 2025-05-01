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
        view = new VuePotager(plateau);
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

            foreach (var terrain in plateau)
            {
                if (terrain.Plante != null)
                {
                    terrain.Plante.Update(meteoDuJour, 1f);
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
                    break; // Ne rien faire pour le retour ici, la logique est gérée dans GestionPlateau
            }

            if (jourActuel > 30)
            {
                continuer = false;
                Console.WriteLine("\nFin de la simulation classique.");
            }
        }
    }
}