using System;

class Program
{
    static void Main(string[] args)
    {
        // 1. Définit les dimensions du plateau
        int largeurPlateau = 10;
        int hauteurPlateau = 10;

        // 2. Crée une instance du gestionnaire de potager (notre "chef d'orchestre")
        GestionPotager gestionnaire = new GestionPotager();

        // 3. Démarre la simulation en passant les dimensions du plateau
        gestionnaire.DemarrerSimulation(largeurPlateau, hauteurPlateau);

        // Le jeu se déroule à l'intérieur de la méthode DemarrerSimulation().
        // Quand le joueur choisit de quitter, la méthode se termine et le programme aussi.
    }
}