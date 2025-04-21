using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var plateau = new Terrain[5, 5];
        InitialiserPlateau(plateau);
        var gestionnaire = new GestionnairePotager();
        gestionnaire.AfficherPlateau(plateau);
    }

    static void InitialiserPlateau(Terrain[,] plateau)
    {
        var rand = new Random();
        for (int y = 0; y < plateau.GetLength(0); y++)
        {
            for (int x = 0; x < plateau.GetLength(1); x++)
            {
                // Alternance entre terrains sableux et argileux
                if (rand.NextDouble() > 0.5)
                    plateau[y, x] = new TerrainSableux();
                else
                    plateau[y, x] = new TerrainArgileux();

                // Ajoute une plante aléatoire pour le test
                if (rand.NextDouble() > 0.7)
                    plateau[y, x].Plante = new Soja();
            }
        }
    }
}