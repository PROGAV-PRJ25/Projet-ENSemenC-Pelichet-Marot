public class VuePotager
{
    private readonly Terrain[,] _plateau;
    private readonly GestionPlateau _controller;
    private Meteo _meteo;
    private Graines _graines;
    private const int CellWidth = 2;

    public VuePotager(Terrain[,] plateau, GestionPlateau controller, Graines graines)
    {
        _plateau = plateau;
        _controller = controller;
        _graines = graines;
    }

    public void SetMeteo(Meteo meteo) => _meteo = meteo;

    public void AfficherPlateau()
    {
        Console.Clear();
        Console.WriteLine("=== POTAGER VU DU CIEL ===");
        Console.WriteLine($"Nombre de graine : {_graines.Nombre}\n");

        if (_meteo != null)
            Console.WriteLine(_meteo.Description + "\n");
        else
            Console.WriteLine("Météo non disponible.\n");

        AfficherLegende();

        Console.WriteLine($"Curseur: X={_controller.CurseurX}, Y={_controller.CurseurY}\n");

        for (int y = 0; y < _plateau.GetLength(0); y++)
        {
            for (int x = 0; x < _plateau.GetLength(1); x++)
                AfficherCase(x, y);
            Console.WriteLine();
        }

        Console.WriteLine(
            "\n🡅/🡇/🡄/🡆 : déplacer  \n" +
            "Espace : action | E : jour suivant | I: info | Q : quitter");
    }

    private void AfficherCase(int x, int y)
    {
        bool estCurseur = (x == _controller.CurseurX && y == _controller.CurseurY);
        var terrain = _plateau[y, x];

        Console.BackgroundColor = estCurseur ? ConsoleColor.Cyan : ConsoleColor.Black;
        Console.ForegroundColor = terrain.Couleur;
        Console.Write('[');

        if (terrain.Plante != null)
        {
            Console.ForegroundColor = terrain.Plante.EstMorte
                ? ConsoleColor.Red
                : ConsoleColor.White;
            var acronyme = terrain.Plante.Acronyme;
            Console.Write(acronyme.PadRight(CellWidth).Substring(0, CellWidth));
        }
        else
        {
            Console.Write(new string(' ', CellWidth));
        }

        Console.ForegroundColor = terrain.Couleur;
        Console.Write(']');
        Console.ResetColor();
    }

    private void AfficherLegende()
    {
        var légendes = new (string Nom, ConsoleColor Couleur)[]
        {
            ("Argile", ConsoleColor.DarkGray),
            ("Sable",  ConsoleColor.Yellow),
            ("Terre",ConsoleColor.Green),
            ("Eau",ConsoleColor.Cyan)
        };

        foreach (var (nom, couleur) in légendes)
        {
            Console.ForegroundColor = couleur;
            Console.Write("■ ");
            Console.ResetColor();
            Console.Write(nom + "  ");
        }
        Console.WriteLine("\n");
    }

    public void AfficherActionsCase(Terrain terrain)
    {
        Console.Clear();
        Console.WriteLine("=== ACTION PARCELLE ===\n");
        Console.WriteLine($"Terrain : {terrain.NomTerrain}\n");

        if (terrain.Plante != null)
        {
            Console.WriteLine($"Plante : {terrain.Plante.NomPlante}\n");
            if (_graines.PeutDepenser(5))
            {
                Console.WriteLine("A : Arroser (5 graines)");
            }
            else
            {
                Console.WriteLine("Vous n'avez pas assez de graines pour arroser (5 graines)");
            }
            Console.WriteLine("D : Désherber");
            Console.WriteLine("R : Récolter");
        }
        else
        {
            Console.WriteLine("Aucune plante.\n");
            Console.WriteLine("P : Planter");
        }

        Console.WriteLine("\nEspace : annuler");
    }
    public void AfficherPlanteOuTerrain(Terrain terrain, int xPlante, int yPlante)
    {
        Console.Clear();

        if (terrain.Plante == null)
        {
            // Infos terrain
            Console.WriteLine($"=== Terrain : {terrain.NomTerrain} ===\n");
            Console.WriteLine($"Fertilité        : {terrain.Fertilité:P0}");
            Console.WriteLine($"Absorption eau   : {terrain.CoeffAbsorptionEau:P0}");
        }
        else
        {
            var p = terrain.Plante;

            // 1) Préférences de la plante
            Console.WriteLine($"=== Préférences de {p.NomPlante} ===\n");
            Console.WriteLine($"Terrain idéal       : {p.TerrainIdeal.NomTerrain}");
            Console.WriteLine($"Saisons semis       : {string.Join(", ", p.SaisonCompatible.Select(s => s.NomSaison))}");
            Console.WriteLine($"Hydratation critique : {p.HydratationCritique:F1}");
            Console.WriteLine($"Température tolérée : de {p.TemperatureMinimale}°C à {p.TemperatureMaximale}°C");
            Console.WriteLine($"Espace requis       : {p.EspacePris} case(s)");


            // 2) Conditions actuelles
            Console.WriteLine("\n=== Conditions actuelles ===\n");

            // Hydratation
            bool condHyd = p.HydratationActuelle >= p.HydratationCritique;
            Console.WriteLine($"{(condHyd ? "✅" : "❌")} Hydratation : {p.HydratationActuelle:F1}% (critique {p.HydratationCritique}%)");

            // Luminosité
            bool condLum = Math.Abs(p.LuminositeActuelle - p.LuminositeIdeale) < 20f;
            Console.WriteLine($"{(condLum ? "✅" : "❌")} Luminosité  : {p.LuminositeActuelle:F1}% (idéal {p.LuminositeIdeale}%)");

            // Température
            bool condTemp = p.TemperatureActuelle >= p.TemperatureMinimale
                         && p.TemperatureActuelle <= p.TemperatureMaximale;
            Console.WriteLine($"{(condTemp ? "✅" : "❌")} Température : {p.TemperatureActuelle:F1}°C");

            // Espacement (appelle la méthode publique de votre controller)
            bool condEsp = _controller.CheckEspaceRespecte(xPlante, yPlante);
            Console.WriteLine($"{(condEsp ? "✅" : "❌")} Espacement  : {(condEsp ? "OK" : "KO")} (besoin {p.EspacePris})");

            // Maladie (sans déréférencer p.MaladieActuelle)
            bool condMal = p.MaladieActuelle == null;
            string nomMal = p.MaladieActuelle?.Nom ?? "Aucune";
            Console.WriteLine($"{(condMal ? "✅" : "❌")} Maladie     : {nomMal}");
        }

        Console.WriteLine("\n[Appuyez sur Espace pour revenir]");
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
    }


    public Plante? ChoisirNouvellePlante()
    {
        Console.Clear();
        Console.WriteLine($"Graines disponibles : {_graines}");
        Console.WriteLine();
        Console.WriteLine("Plantes :");
        Console.WriteLine("1 - Soja (10 graines)");
        Console.WriteLine("2 - Maïs (12 graines)");
        Console.WriteLine("3 - Canne à sucre (14 graines)");
        Console.WriteLine("4 - Café (16 graines)");
        Console.WriteLine("5 - Cactus (20 graines)");
        Console.WriteLine("6 - Coton (15 graines)");
        Console.WriteLine("0 - Annuler");

        while (true)
        {
            Console.Write("\nVotre choix : ");
            var saisie = Console.ReadLine();
            Plante? plante = saisie switch
            {
                "1" => new Soja(_graines),
                "2" => new Mais(_graines),
                "3" => new CanneASucre(_graines),
                "4" => new Cafe(_graines),
                "5" => new Cactus(_graines),
                "6" => new Coton(_graines),
                "0" => null,
                _ => null
            };

            if (plante == null && saisie != "0")
            {
                Console.WriteLine("Choix invalide.");
                continue;
            }

            if (plante == null) return null;

            if (_graines.PeutDepenser(plante.PrixGraines))
            {
                Console.WriteLine($"Vous avez planté un(e) {plante.GetType().Name} !");
                _graines.Depenser(plante.PrixGraines);
                return plante;
            }
            else
            {
                Console.WriteLine("Pas assez de graines pour cette plante.");
            }
        }
    }
}