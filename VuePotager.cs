using System;

public class VuePotager
{
    //Ici on utilise le polymorphisme. C'est à dire que l'on utilise le classe parente 'Terrain' de manière à ce 
    //que nos fonctions s'adapte directement à la classe fille correspondante. Ceci évite la multiplication du code.
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

        Console.WriteLine($"Flèches:(X:{curseurX}, Y:{curseurY}) | E: Jour suivant | Q: Quitter\n");

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
    public void AfficherPlanteOuTerrain(Terrain terrain)
    {
        Console.Clear();
        if (terrain.Plante == null)
        {
            Console.WriteLine($"\n=== Terrain {terrain.NomTerrain} ===");
            Console.WriteLine();
            Console.WriteLine($"Fertilité : {terrain.Fertilité}");
            Console.WriteLine($"Coefficient d'absorption d'eau : {terrain.CoeffAbsorptionEau}");
            Console.WriteLine($"Plantes acceptables : à définir");
            Console.WriteLine();
            Console.WriteLine($"Engrais : {terrain.Fertilité}");
            Console.WriteLine($"Eau : {terrain.CoeffAbsorptionEau}");
        }
        else
        {
            Console.WriteLine($"\n=== {terrain.Plante.NomPlante} ===");
            Console.WriteLine();
            Console.WriteLine($"Acronyme : {terrain.Plante.Acronyme}");
            Console.WriteLine($"Terrain Ideal : {terrain.Plante.TerrainIdeal}");
            Console.WriteLine($"Saison de semi : {terrain.Plante.SaisonCompatible}");
            Console.WriteLine($"Vitesse de désydratation : {terrain.Plante.VitesseDeshydratation}");
            Console.WriteLine($"Température min : {terrain.Plante.TemperatureMinimale}");
            Console.WriteLine($"Température max : {terrain.Plante.TemperatureMaximale}");
            Console.WriteLine($"Nombre de jours max de survie en dehors des limites de T : {terrain.Plante.JoursHorsLimiteTemperature} | Reste : à déf");
            Console.WriteLine();
            Console.WriteLine($"Hydratation : {terrain.Plante.Hydratation:F1}");
            if (terrain.Plante.EstEnStressThermique)
            {
                Console.WriteLine($"La plante est en stress thermique.");
            }
            else
            {
                Console.WriteLine($"La plante n'est pas stress thermique.");
            }
            Console.WriteLine($"Vivacité : {terrain.Plante.CalculerVivacite(meteoActuelle):Pi} %");
        }
        Console.WriteLine();
        Console.WriteLine("R - Retour");
    }

    // Nouvelle méthode pour afficher les actions spécifiques après l'interaction
    public void AfficherActionsCase(Terrain terrain)
    {
        Console.WriteLine("\n=== ACTION PARCELLE ===");
        Console.WriteLine($"\nTerrain {terrain.NomTerrain}");

        if (terrain.Plante != null)
        {
            Console.WriteLine($"Plante: {terrain.Plante.NomPlante}");
            Console.WriteLine("A - Arroser | C - Récolter | D - Désherber ");
        }
        else
        {
            Console.WriteLine("P - Planter");
        }
        Console.WriteLine("\nR - Retour");
    }
}