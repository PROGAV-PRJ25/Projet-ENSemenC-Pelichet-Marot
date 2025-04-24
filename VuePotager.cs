using System;

public class VuePotager
{
    // Le plateau de terrains à afficher
    private Terrain[,] plateau;

    // Position du curseur du joueur
    private int curseurX = 0;
    private int curseurY = 0;

    // Constructeur : on a besoin du plateau pour l'afficher
    public VuePotager(Terrain[,] plateauInitial)
    {
        plateau = plateauInitial;
    }

    // Propriétés pour accéder à la position du curseur et la modifier
    public int CurseurX
    {
        get { return curseurX; }
        set { curseurX = value; }
    }

    public int CurseurY
    {
        get { return curseurY; }
        set { curseurY = value; }
    }

    // Méthode principale pour afficher le plateau et les informations
    public void AfficherPlateau()
    {
        // Efface la console pour un nouvel affichage
        Console.Clear();

        // Titre du potager
        Console.WriteLine("=== POTAGER VU DU CIEL ===");

        // Affiche la légende des types de terrain
        AfficherLegendeTerrain("Argileux", ConsoleColor.DarkGray);
        AfficherLegendeTerrain("Sableux", ConsoleColor.DarkYellow);
        AfficherLegendeTerrain("Classique", ConsoleColor.DarkGreen);
        Console.ResetColor(); // Réinitialise les couleurs de la console

        Console.WriteLine("Flèches: Se déplacer | Espace: Interagir | Q: Quitter\n");

        // Affiche chaque case du plateau
        for (int y = 0; y < plateau.GetLength(0); y++)
        {
            for (int x = 0; x < plateau.GetLength(1); x++)
            {
                AfficherCase(x, y);
            }
            Console.WriteLine(); // Nouvelle ligne après chaque ligne du plateau
        }

        // Affiche les informations de la case sélectionnée
        AfficherInfosCase(plateau[curseurY, curseurX]);
    }

    // Méthode pour afficher une seule case du plateau
    private void AfficherCase(int x, int y)
    {
        // Vérifie si la case correspond à la position du curseur
        bool estCurseur = (x == curseurX && y == curseurY);
        var terrain = plateau[y, x];

        // Définit les couleurs de fond et de texte de la case
        Console.BackgroundColor = estCurseur ? ConsoleColor.DarkCyan : ConsoleColor.Black;
        Console.ForegroundColor = terrain.Couleur;

        Console.Write("[");

        // Affiche l'acronyme de la plante s'il y en a une
        if (terrain.Plante != null)
        {
            Console.ForegroundColor = terrain.Plante.EstMorte ? ConsoleColor.Red : ConsoleColor.DarkBlue;
            Console.Write(terrain.Plante.Acronyme);
        }
        else
        {
            Console.Write("  "); // Espace vide si pas de plante
        }

        Console.ForegroundColor = terrain.Couleur;
        Console.Write("]");
        Console.ResetColor(); // Réinitialise les couleurs
    }

    // Méthode pour afficher la légende des types de terrain
    private void AfficherLegendeTerrain(string nomTerrain, ConsoleColor couleur)
    {
        Console.ForegroundColor = couleur;
        Console.Write("■ "); // Symbole pour le terrain
        Console.ResetColor();
        Console.WriteLine(nomTerrain);
    }

    // Méthode pour afficher les informations d'une case sélectionnée
    private void AfficherInfosCase(Terrain terrain)
    {
        Console.WriteLine("\n=== CASE SÉLECTIONNÉE ===");
        Console.WriteLine($"Position: ({curseurX}, {curseurY})");
        Console.WriteLine($"Type: {terrain.NomTerrain}");

        if (terrain.Plante != null)
        {
            Console.WriteLine($"\nPlante: {terrain.Plante.NomPlante}");
            Console.WriteLine($"Santé: {(terrain.Plante.EstMorte ? "Morte" : "Vivante")}");
            Console.WriteLine("\nActions disponibles:");
            Console.WriteLine("A - Arroser | D - Désherber | R - Récolter");
        }
        else
        {
            Console.WriteLine("\nAucune plante");
            Console.WriteLine("\nAction: P - Planter");
        }
    }
}