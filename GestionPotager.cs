public class GestionPotager
{
    private VuePotager view;
    private GestionPlateau plateauController;
    private Terrain[,] plateau;
    public Saison saisonActuelle;
    private int jourActuel = 1;

    public void DemarrerSimulation(int largeurPlateau, int hauteurPlateau)
    {
        plateau = GenerateurPlateau.GenererPlateau(largeurPlateau, hauteurPlateau);
        view = new VuePotager(plateau);
        plateauController = new GestionPlateau(plateau, view, this);

        saisonActuelle = new SaisonPluvieuse();

        LancerModeClassique();
    }

    private void LancerModeClassique()
    {
        bool continuer = true;
        while (continuer)
        {
            Meteo meteoDuJour = Meteo.GenererPourSaison(saisonActuelle);
            view.SetMeteo(meteoDuJour); // Passer la météo à la vue

            Console.WriteLine($"\n----- Jour {jourActuel} -----");
            // On ne répète pas la description ici, elle sera dans l'affichage du plateau

            foreach (var terrain in plateau)
            {
                if (terrain.Plante != null)
                {
                    terrain.Plante.Update(meteoDuJour, 1f);
                }
            }

            view.AfficherPlateau();
            plateauController.GererEntreesUtilisateurModeClassique();

            Console.WriteLine("\nAppuyez sur Entrée pour passer au jour suivant...");
            Console.ReadKey();
            jourActuel++;

            if (jourActuel > 30)
            {
                continuer = false;
                Console.WriteLine("\nFin de la simulation classique.");
            }
        }
    }
}