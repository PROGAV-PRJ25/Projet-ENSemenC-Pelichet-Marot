using System;
using System.Collections.Generic;
using System.Linq;

public static class GenerateurBiome
{
    public static Terrain[,] GenererPlateau(int largeur, int hauteur)
    {
        var plateau = new Terrain[hauteur, largeur];
        var rand = new Random();

        int tailleBiomeMin = 10;
        int tailleBiomeMax = 25;

        // Définir les types de terrains et le nombre de cases à attribuer à chacun
        List<(Func<Terrain> constructeur, int nombreCases)> biomes = new()
        {
            (() => new TerrainSableux(), rand.Next(tailleBiomeMin, tailleBiomeMax)),
            (() => new TerrainArgileux(), rand.Next(tailleBiomeMin, tailleBiomeMax)),
            (() => new TerrainClassiqueTerreux(), rand.Next(tailleBiomeMin, tailleBiomeMax)),
        };

        bool[,] occupe = new bool[hauteur, largeur];

        foreach (var (creerTerrain, tailleBiome) in biomes)
        {
            int startX, startY;
            do
            {
                startX = rand.Next(largeur);
                startY = rand.Next(hauteur);
            } while (occupe[startY, startX]);

            var file = new Queue<(int x, int y)>();
            file.Enqueue((startX, startY));

            int casesPlacees = 0;
            while (file.Count > 0 && casesPlacees < tailleBiome)
            {
                var (x, y) = file.Dequeue();

                if (x < 0 || y < 0 || x >= largeur || y >= hauteur || occupe[y, x])
                    continue;

                plateau[y, x] = creerTerrain();
                occupe[y, x] = true;
                casesPlacees++;

                var voisins = new List<(int x, int y)>
                {
                    (x+1, y), (x-1, y), (x, y+1), (x, y-1)
                };
                voisins = voisins.OrderBy(_ => rand.Next()).ToList();

                foreach (var v in voisins)
                    file.Enqueue(v);
            }
        }

        // Remplir les cases restantes avec du terrain classique
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                if (plateau[y, x] == null)
                    plateau[y, x] = new TerrainClassiqueTerreux();
            }
        }

        return plateau;
    }
}
