using System;

public class VuePotager
{
    //Ici on utilise le polymorphisme. C'est à dire que l'on utilise le classe parente 'Terrain' de manière à ce
    //que nos fonctions s'adapte directement à la classe fille correspondante. Ceci évite la multiplication du code.
    private Terrain[,] plateau;
    private int curseurX = 0;
    private int curseurY = 0;
    private Meteo meteoActuelle;
    private GestionPlateau plateauController;

    public VuePotager(Terrain[,] plateauInitial,  GestionPlateau gestionPlateau)
    {
        plateau = plateauInitial;
        plateauController = gestionPlateau;
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
            Console.WriteLine($"{meteoActuelle.Description}\n");
        }
        else
        {
            Console.WriteLine("Météo non disponible.\n");
        }

        AfficherLegendeTerrain("Argileux", ConsoleColor.DarkGray);
        AfficherLegendeTerrain("Sableux", ConsoleColor.DarkYellow);
        AfficherLegendeTerrain("Classique", ConsoleColor.DarkGreen);
        AfficherLegendeTerrain("Aquatique", ConsoleColor.Cyan);
        Console.ResetColor();

        Console.WriteLine($"Flèches: Se déplacer (X:{curseurX}, Y:{curseurY})");

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
            Console.ForegroundColor = terrain.Plante.EstMorte
                ? ConsoleColor.Red
                : ConsoleColor.DarkBlue;
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

    public void AfficherPlanteOuTerrain(Terrain terrain, int xPlante, int yPlante)
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
            bool espaceRespecte = (plateauController != null) ? plateauController.CheckEspaceRespecte(xPlante, yPlante) : true; // Nécessite un accès à plateauController et aux coordonnées

            Console.WriteLine($"[DEBUG AFFICHAGE] Vérification espacement pour plante en ({xPlante}, {yPlante}), Résultat: {(espaceRespecte ? "OK" : "KO")}");

            Console.WriteLine($"\n=== {terrain.Plante.NomPlante} ===");
            Console.WriteLine();
            Console.WriteLine($"Acronyme : {terrain.Plante.Acronyme}");
            Console.WriteLine($"Terrain Ideal : {terrain.Plante.TerrainIdeal}");
            Console.WriteLine($"Saison de semi : {terrain.Plante.SaisonCompatible.FirstOrDefault()?.NomSaison ?? "N/A"}");
            Console.WriteLine($"Vitesse de désydratation : {terrain.Plante.VitesseDeshydratation}% par jour");
            Console.WriteLine($"Température idéale : {terrain.Plante.TemperatureMinimale}°C - {terrain.Plante.TemperatureMaximale}°C (Actuelle: {terrain.Plante.TemperatureActuelle}°C)");
            Console.WriteLine();
            Console.WriteLine($"Hydratation : {(Math.Abs(terrain.Plante.HydratationActuelle - terrain.Plante.HydratationIdeale) < 20f ? "[✅]" : "[❌]")} {terrain.Plante.HydratationActuelle:F1}% (Idéal: {terrain.Plante.HydratationIdeale}%)");
            Console.WriteLine($"Luminosité : {(Math.Abs(terrain.Plante.LuminositeActuelle - terrain.Plante.LuminositeIdeale) < 20f ? "[✅]" : "[❌]")} {terrain.Plante.LuminositeActuelle:F1}% (Idéal: {terrain.Plante.LuminositeIdeale}%)");
            Console.WriteLine($"Température : {(terrain.Plante.TemperatureActuelle >= terrain.Plante.TemperatureMinimale && terrain.Plante.TemperatureActuelle <= terrain.Plante.TemperatureMaximale ? "[✅]" : "[❌]")}");
            Console.WriteLine($"Espacement : {(espaceRespecte ? "[✅]" : "[❌]")} (Besoin: {terrain.Plante.EspacePris})");
            Console.WriteLine($"Maladie : {(terrain.Plante.MaladieActuelle == null ? "[✅]" : $"[❌] {terrain.Plante.MaladieActuelle.NomMaladie}")}");
        }
        Console.WriteLine();
        Console.WriteLine("R - Retour");
    }

    public void AfficherActionsCase(Terrain terrain)
    {
        Console.WriteLine("\n=== ACTION PARCELLE ===");
        Console.WriteLine($"\nTerrain {terrain.NomTerrain}");

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
