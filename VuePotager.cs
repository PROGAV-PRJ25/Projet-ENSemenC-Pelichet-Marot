using System;

public class VuePotager
{
    private Terrain[,] plateau;
    private int curseurX = 0;
    private int curseurY = 0;
    private Meteo meteoActuelle;

    public VuePotager(Terrain[,] plateauInitial)
    {
        plateau = plateauInitial;
    }

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

    public void SetMeteo(Meteo meteo)
    {
        meteoActuelle = meteo;
    }

    public void AfficherPlateau()
    {
        Console.Clear();
        Console.WriteLine("=== POTAGER VU DU CIEL ===");

        if (meteoActuelle != null)
        {
            Console.WriteLine($"  {meteoActuelle.Description}\n");
        }
        else
        {
            Console.WriteLine("  Météo non disponible.\n");
        }

        AfficherLegendeTerrain("Argileux", ConsoleColor.DarkGray);
        AfficherLegendeTerrain("Sableux", ConsoleColor.DarkYellow);
        AfficherLegendeTerrain("Classique", ConsoleColor.DarkGreen);
        Console.ResetColor();

        Console.WriteLine($"Flèches: Se déplacer (X:{curseurX}, Y:{curseurY}) | Espace: Interagir | Q: Quitter | E: Jour suivant\n");

        for (int y = 0; y < plateau.GetLength(0); y++)
        {
            for (int x = 0; x < plateau.GetLength(1); x++)
            {
                AfficherCase(x, y);
            }
            Console.WriteLine();
        }
    }

    private void AfficherCase(int x, int y)
    {
        bool estCurseur = (x == curseurX && y == curseurY);
        var terrain = plateau[y, x];

        Console.BackgroundColor = estCurseur ? ConsoleColor.DarkCyan : ConsoleColor.Black;
        Console.ForegroundColor = terrain.Couleur;

        Console.Write("[");

        if (terrain.Plante != null)
        {
            Console.ForegroundColor = terrain.Plante.EstMorte ? ConsoleColor.Red : ConsoleColor.DarkBlue;
            Console.Write(terrain.Plante.Acronyme.PadRight(2)); // Assure-toi que l'acronyme prend 2 caractères, espace à droite si besoin
        }
        else
        {
            Console.Write("  "); // Deux espaces pour aligner avec les acronymes
        }

        Console.ForegroundColor = terrain.Couleur;
        Console.Write("]");
        Console.ResetColor();
    }

    private void AfficherLegendeTerrain(string nomTerrain, ConsoleColor couleur)
    {
        Console.ForegroundColor = couleur;
        Console.Write("■ ");
        Console.ResetColor();
        Console.WriteLine(nomTerrain);
    }

    // Nouvelle méthode pour afficher les actions spécifiques après l'interaction
    public void AfficherActionsCase(Terrain terrain)
    {
        Console.WriteLine("\n=== ACTION PARCELLE ===");
        Console.WriteLine($"Terrain {terrain.NomTerrain}");

        if (terrain.Plante != null)
        {
            Console.WriteLine($"Plante: {terrain.Plante.NomPlante}");
            Console.WriteLine("A - Arroser | R - Récolter | D - Désherber ");
        }
        else
        {
            Console.WriteLine("P - Planter");
        }
        Console.WriteLine("\nEspace - Changer de parcelle");
    }
}