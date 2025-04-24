public class GestionPotager
{
    // La vue pour afficher le potager
    private VuePotager view;

    // Le contrôleur pour gérer les actions du joueur
    private GestionPlateau controller;

    // Le plateau de terrains
    private Terrain[,] plateau;

    // Méthode pour démarrer la simulation de potager
    public void DemarrerSimulation(int largeurPlateau, int hauteurPlateau)
    {
        // 1. Générer le plateau de terrains
        plateau = GenerateurPlateau.GenererPlateau(largeurPlateau, hauteurPlateau);

        // 2. Créer la vue et le contrôleur, et leur donner le plateau
        view = new VuePotager(plateau);
        controller = new GestionPlateau(plateau, view);

        // 3. Lancer la boucle principale du jeu en appelant le contrôleur
        controller.GererEntreesUtilisateur();
    }
}