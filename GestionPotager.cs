public class GestionPotager
{
    private VuePotager view;
    private GestionPlateau plateauController;
    private Terrain[,] plateau;
    public Saison saisonActuelle;
    private int jourActuel = 1;
    public int JourActuel
    {
        get { return jourActuel; }
        set { jourActuel = value; }
    }
    public void DemarrerSimulation(int largeurPlateau, int hauteurPlateau)
    {
        //Ici démarre la notre jeu :
        //On génère dabord un plateau en fonction de sa largeur et de sa longueur.
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
            Meteo meteoDuJour = Meteo.GenererPourSaison(saisonActuelle, JourActuel);
            view.SetMeteo(meteoDuJour); // Passer la météo à la vue

            Console.WriteLine($"\n----- Jour {jourActuel} -----");
            // On ne répète pas la description ici, elle sera dans l'affichage du plateau. 
            //Je ne vois pas cette commande dans l'affichage. On supprime ?

            foreach (var terrain in plateau)
            {
                if (terrain.Plante != null)
                {
                    terrain.Plante.Update(meteoDuJour, 1f);
                }
            }

            bool actionDuJour = true;
            while (actionDuJour)
            {
                view.AfficherPlateau();
                plateauController.GererEntreesUtilisateurModeClassique();

                Console.WriteLine("\nAppuyez à nouveau pour confirmer votre choix | R: Retour");

                ConsoleKeyInfo touche = Console.ReadKey(true);
                switch (touche.Key)
                {
                    case ConsoleKey.E:
                        jourActuel++;
                        actionDuJour = false;
                        break;
                    case ConsoleKey.Q:
                        continuer = false;
                        actionDuJour = false;
                        break;
                    case ConsoleKey.R:
                        break;
                }
            }

            if (jourActuel > 30)
            {
                continuer = false;
                Console.WriteLine("\nFin de la simulation classique.");
            }
        }
    }
}