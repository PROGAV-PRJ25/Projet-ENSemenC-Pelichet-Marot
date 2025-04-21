class Program
{
    static void Main()
    {
        var plateau = new Terrain[10, 10];
        InitialiserPlateau(plateau);
        var gestionnaire = new GestionnairePotager();
        gestionnaire.AfficherPlateau(plateau);
    }

    static void InitialiserPlateau(Terrain[,] plateau)
    {
        int hauteur = plateau.GetLength(0);
        int largeur = plateau.GetLength(1);
        var rand = new Random();

        // Créer des zones de terrain (biomes) en blocs
        int tailleBiomeMin = 10;
        int tailleBiomeMax = 25;

        List<(Func<Terrain> constructeur, int nombreCases)> biomes = new()
        {
            (() => new TerrainSableux(), rand.Next(tailleBiomeMin, tailleBiomeMax)),
            (() => new TerrainArgileux(), rand.Next(tailleBiomeMin, tailleBiomeMax)),
            (() => new TerrainClassiqueTerreux(), rand.Next(tailleBiomeMin, tailleBiomeMax)),
        };

        // Marquer les cases disponibles
        bool[,] occupe = new bool[hauteur, largeur];

        foreach (var (creerTerrain, tailleBiome) in biomes)
        {
            // Choisir un point de départ libre
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

                // Ajouter voisins aléatoirement (priorité au hasard pour un effet organique)
                var voisins = new List<(int x, int y)>
                {
                    (x+1, y), (x-1, y), (x, y+1), (x, y-1)
                };
                voisins = voisins.OrderBy(_ => rand.Next()).ToList();

                foreach (var v in voisins)
                    file.Enqueue(v);
            }
        }

        // Compléter les cases restantes avec du terrain classique par défaut
        for (int y = 0; y < hauteur; y++)
        {
            for (int x = 0; x < largeur; x++)
            {
                if (plateau[y, x] == null)
                    plateau[y, x] = new TerrainClassiqueTerreux();
            }
        }
    }
}
