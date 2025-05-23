public class VuePotager
{
    private readonly Terrain[,] _plateau;
    private GestionPlateau _controller;
    private Meteo _meteo;
    private Graines _graines;
    private const int CellWidth = 2;
    public int Hauteur => _plateau.GetLength(0);
    public int Largeur => _plateau.GetLength(1);


    public VuePotager(Terrain[,] plateau, Graines graines)
    {
        _plateau = plateau;
        _graines = graines;
    }

    public void SetMeteo(Meteo meteo) => _meteo = meteo;
    public void SetController(GestionPlateau controller)
    {
        _controller = controller;
    }

    public void AfficherPlateau()
    {
        Console.Clear();
        Console.WriteLine("=== POTAGER VU DU CIEL ===");
        Console.WriteLine($"💵 Nombre de graines : {_graines.Nombre}\n");

        if (_meteo != null)
            Console.WriteLine(_meteo.Description + "\n");
        else
            Console.WriteLine("Météo non disponible.\n");

        Console.WriteLine($"Curseur: X={_controller.CurseurX}, Y={_controller.CurseurY}\n");

        AfficherLegende();

        for (int y = 0; y < _plateau.GetLength(0); y++)
        {
            for (int x = 0; x < _plateau.GetLength(1); x++)
                AfficherCase(x, y);
            Console.WriteLine();
        }

        Console.WriteLine(
            "\n🡅/🡇/🡄/🡆 : se déplacer  \n" +
            "Espace  : action  \nI       : info \nE       : semaine suivante\nQ       : quitter");
    }

    private void AfficherCase(int x, int y)
    {
        bool estCurseur = (x == _controller.CurseurX && y == _controller.CurseurY);
        var terrain = _plateau[y, x];

        Console.BackgroundColor = estCurseur ? ConsoleColor.Magenta : ConsoleColor.Black;
        Console.ForegroundColor = terrain.Couleur;
        Console.Write('[');

        if (terrain.Plante != null)
        {
            var p = terrain.Plante;

            // 1) Calcul du nombre de critères non optimaux
            bool espaceOk = _controller.CheckEspaceRespecte(x, y);
            float fracNonOpt = p.EvaluerConditions(
                espaceOk,
                _meteo.SaisonActuelle,
                terrain
            );
            int nonOptCount = (int)Math.Round(fracNonOpt * 7);

            // 2) Choix de la couleur, dans l’ordre de priorité :
            //    - Mort → rouge
            //    - 3 conditions non satisfaites → orange
            //    - Mûre → vert
            //    - Sinon → blanc
            if (p.EstMorte)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if (nonOptCount == 3)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if (p.EstMature && p.PeutProduireFruits)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }


            // 3) Affichage de l’acronyme
            var code = p.Acronyme.PadRight(CellWidth).Substring(0, CellWidth);
            Console.Write(code);
        }
        else
        {
            // Case vide
            Console.ForegroundColor = terrain.Couleur;
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
            ("Point d'eau",ConsoleColor.Cyan)
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
        Console.WriteLine("=== ACTION PARCELLE ===");
        Console.WriteLine($"\nTerrain : {terrain.NomTerrain}");

        if (terrain.Plante != null)
        {
            if (terrain.Plante is Compost)
            {
                Console.WriteLine($"\nObjet : {terrain.Plante.NomPlante}");
                Console.WriteLine("\n♻️ Compost installé");
                Console.WriteLine("\nE : Enlever le compost");
            }
            else
            {
                Console.WriteLine($"\nPlante : {terrain.Plante.NomPlante}\n");
                // _ Affichage de l'action Arroser _
                if (_graines.PeutDepenser(5))
                {
                    Console.WriteLine("A : Arroser 🚿 (5 graines)");
                }
                else
                {
                    Console.WriteLine("Vous n'avez pas assez de graines pour arroser (5 graines)");
                }

                // _ Affichage de l'action Soigner _
                if (terrain.Plante.ObstacleActuel is Maladie && _graines.PeutDepenser(10))
                {
                    Console.WriteLine("S : Soigner 🧪 (10 graines)");
                }
                else if (terrain.Plante.ObstacleActuel is Maladie)
                {
                    Console.WriteLine("La plante est malade mais vous n'avez pas assez de graines pour la soigner (10 graines)");
                }

                // _ Affichage de l'action Equiper _
                if (terrain.Plante.Accessoire == Plante.Equipement.Aucun && _graines.PeutDepenser(10))
                {
                    Console.WriteLine("Z : Ajouter une serre     📈🌡️  + 📈☀️  (10 graines)");
                    Console.WriteLine("O : Ajouter une ombrelle  📉🌡️  + 📉☀️  (10 graines)");
                }
                else if (terrain.Plante.Accessoire != Plante.Equipement.Aucun)
                {
                    Console.WriteLine("X : Retirer l'équipement ❌");
                }

                Console.WriteLine("");

                // _ Affichage des actions Récolter et Désherber_
                Console.WriteLine("R : Récolter 🫴 ");
                Console.WriteLine("D : Désherber 🪓");
            }
        }
        else
        {
            Console.WriteLine("Aucune plante.\n");
            Console.WriteLine("P : Planter 🌱");
        }

        Console.WriteLine("");
        Console.WriteLine("Espace : annuler");
    }
    public void AfficherPlanteOuTerrain(Terrain terrain, int xPlante, int yPlante)
    {
        Console.Clear();
        if (terrain.Plante is Compost compost)
        {
            Console.WriteLine("=== Compost installé ===");
            Console.WriteLine("");
            Console.WriteLine("Le compost transforme les déchets organiques en ressources !");
            Console.WriteLine("Obtenez 15 graines lorsqu'il est rempli (4 plantes désherbées) !");
            Console.WriteLine("");
            int segments = 10;
            int remplis = compost.Remplissage / 10;
            string barre = "[" + new string('#', remplis) + new string(' ', segments - remplis) + $"] {compost.Remplissage}%";
            Console.WriteLine($"♻️  Remplissage : {barre}");
            return;
        }
        if (terrain.Plante == null)
        {
            // Affichage quand il n'y a pas de plante
            Console.WriteLine($"=== Terrain {terrain.NomTerrain} ===\n");
            Console.WriteLine($"Fertilité 📈       : {terrain.Fertilité}");
            Console.WriteLine($"Absorption d'eau 💧: {terrain.CoeffAbsorptionEau}");
        }
        else
        {
            var p = terrain.Plante;


            // 1) Caractéristiques de la plante
            int labelWidth = 30;

            Console.WriteLine($"=== Caractéristiques de {p.NomPlante} ===\n");
            Console.WriteLine($"{"⛰️  Terrain idéal".PadRight(labelWidth)}   : {p.TerrainIdeal.NomTerrain}");
            Console.WriteLine($"{"🍂 Saison préférée".PadRight(labelWidth)}   : {string.Join(", ", p.SaisonCompatible.Select(s => s.NomSaison))}");
            Console.WriteLine($"{"💧 Hydratation critique".PadRight(labelWidth)}   : {p.HydratationCritique:F1}%");
            Console.WriteLine($"{"🌡️  Température tolérée".PadRight(labelWidth)}    : de {p.TemperatureMinimale}°C à {p.TemperatureMaximale}°C");
            Console.WriteLine($"{"↔️  Espace requis".PadRight(labelWidth)}   : {p.EspacePris} case{(p.EspacePris > 1 ? "s" : "")}");

            var niveaux = new[] { "très faible", "très faible à faible", "faible à modéré", "modéré à fort", "fort à très fort" };
            Console.WriteLine($"{"☀️  Ensoleillement souhaité".PadRight(labelWidth)}   : indices {p.LuminositeIdeale - 1} à {p.LuminositeIdeale}  ({niveaux[p.LuminositeIdeale - 1]})");
            Console.WriteLine($"{"🌸 Plante vivace ?".PadRight(labelWidth)}   : {(p.EstVivace ? "✅" : "❌")}\n");




            // 2) État actuel des conditions
            Console.WriteLine("\n=== État actuel ===\n");

            bool condHyd = p.HydratationActuelle >= p.HydratationCritique;
            bool condLum = p.LuminositeActuelle == p.LuminositeIdeale || p.LuminositeActuelle == (p.LuminositeIdeale - 1);
            bool condTemp = p.TemperatureActuelle >= p.TemperatureMinimale
                         && p.TemperatureActuelle <= p.TemperatureMaximale;

            int espActuelle = _controller.EspacementActuel(xPlante, yPlante);
            bool condEsp = espActuelle < 0 || espActuelle >= p.EspacePris;
            string texteEsp = espActuelle < 0
                ? "aucune autre plante"
                : $"{espActuelle} case{(espActuelle > 1 ? "s" : "")}";

            bool condObs = p.ObstacleActuel == null;
            string nomObs = p.ObstacleActuel?.Nom ?? "Aucun";
            string detailsObs = p.ObstacleActuel != null ? $" ({p.ObstacleActuel.Description})"
    : "";

            bool condSaison = p.SaisonCompatible
                .Any(s => s.NomSaison == _meteo.SaisonActuelle.NomSaison);

            bool condTerrain = terrain.GetType() == p.TerrainIdeal.GetType();



            // Affichage des 7 conditions
            Console.WriteLine($"{(condHyd ? "✅" : "❌")} {"Hydratation".PadRight(labelWidth)}: {p.HydratationActuelle:F1}%");
            Console.WriteLine($"{(condLum ? "✅" : "❌")} {"Ensoleillement".PadRight(labelWidth)}: indice {p.LuminositeActuelle}");
            Console.WriteLine($"{(condTemp ? "✅" : "❌")} {"Température".PadRight(labelWidth)}: {p.TemperatureActuelle:F1}°C");
            Console.WriteLine($"{(condEsp ? "✅" : "❌")} {"Espacement actuel".PadRight(labelWidth)}: {texteEsp} (besoin : {p.EspacePris})");
            Console.WriteLine($"{(condObs ? "✅" : "❌")} {"Obstacle".PadRight(labelWidth)}: {nomObs} {detailsObs}");
            Console.WriteLine($"{(condSaison ? "✅" : "❌")} {"Saison".PadRight(labelWidth)}: {_meteo.SaisonActuelle.NomSaison}");
            Console.WriteLine($"{(condTerrain ? "✅" : "❌")} {"Terrain".PadRight(labelWidth)}: {terrain.NomTerrain}");
            Console.WriteLine("");

            // _ Affichage de la présence d'une Serre ou d'une Ombrelle sur la plante _ 
            Console.WriteLine($"\nÉquipement : {terrain.Plante.Accessoire switch
            {
                Plante.Equipement.Serre => "🏠 Serre",
                Plante.Equipement.Ombrelle => "☂️  Ombrelle",
                _ => "Aucun"
            }}");

            // Affichage de la barre de croissance
            double ratio = p.HauteurActuelle / p.HauteurMaximale;
            int totalSegments = 10;
            int remplis = (int)Math.Round(ratio * totalSegments);
            string barre = "["
                         + new string('#', remplis)
                         + new string(' ', totalSegments - remplis)
                         + $"] {ratio * 100:F0}%";

            if (!(p is Compost) && p.EstMorte)
            {
                Console.WriteLine($"\n{p.NomPlante} est mort(e) 💀");
            }
            else
            {
                Console.WriteLine($"\nBarre de croissance... ");
                Console.WriteLine($"🌱  {barre}");
                Console.WriteLine($"Age : {p.SemainesDepuisPlantation} semaines");
                if (p.EstVivace && p.EstMature && !p.PeutProduireFruits)
                {
                    Console.WriteLine($"\n⏳ La plante est en attente de nouvelle production (vivace)");
                    Console.WriteLine($"Temps écoulé depuis la dernière récolte : {p.SemainesDepuisDerniereRecolte} semaines");
                    Console.WriteLine($"Refleurira après 10 semaines si la saison est favorable ({string.Join(", ", p.SaisonCompatible.Select(s => s.NomSaison))})");
                }

            }
        }
    }



    public Plante? ChoisirNouvellePlante()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Graines disponibles : {_graines}");
            Console.WriteLine("");
            Console.WriteLine("Plantes :");
            Console.WriteLine("1 - 🫛 Soja          (10 graines)");
            Console.WriteLine("2 - 🌽 Maïs          (12 graines)");
            Console.WriteLine("3 - 🎍 Canne à sucre (14 graines)");
            Console.WriteLine("4 - ☕ Café          (16 graines)");
            Console.WriteLine("5 - 🌵 Cactus        (20 graines)");
            Console.WriteLine("6 - ☁️ Coton         (15 graines)");
            Console.WriteLine("");
            Console.WriteLine("Objets :");
            Console.WriteLine("7 - 💩  Composte     (30 graines)");
            Console.WriteLine("");
            Console.WriteLine("0 - Annuler");
            Console.WriteLine("");
            Console.Write("\nVotre choix : ");

            var key = Console.ReadKey(true).Key;
            Plante? plante = key switch
            {
                ConsoleKey.D1 or ConsoleKey.NumPad1 => new Soja(_graines),
                ConsoleKey.D2 or ConsoleKey.NumPad2 => new Mais(_graines),
                ConsoleKey.D3 or ConsoleKey.NumPad3 => new CanneASucre(_graines),
                ConsoleKey.D4 or ConsoleKey.NumPad4 => new Cafe(_graines),
                ConsoleKey.D5 or ConsoleKey.NumPad5 => new Cactus(_graines),
                ConsoleKey.D6 or ConsoleKey.NumPad6 => new Coton(_graines),
                ConsoleKey.D7 or ConsoleKey.NumPad7 => new Compost(_graines),
                ConsoleKey.D0 or ConsoleKey.NumPad0 => null,
                _ => null
            };

            if (plante == null && key != ConsoleKey.D0 && key != ConsoleKey.NumPad0)
            {
                Console.WriteLine("\nChoix invalide.");
                Thread.Sleep(2000);
                continue;
            }

            if (plante == null)
            {
                // Annulation
                return null;
            }

            if (plante is Compost && _controller.CompostExiste())
            {
                Console.WriteLine("\n❌ Un compost est déjà présent sur le plateau !");
                Thread.Sleep(2000);
                continue;
            }

            if (_graines.PeutDepenser(plante.PrixGraines))
            {
                _graines.Depenser(plante.PrixGraines);
                if(plante.GetType().Name == "Compost")
                Console.WriteLine($"\nVous avez installé un {plante.GetType().Name} !");
                else
                Console.WriteLine($"\nVous avez planté un(e) {plante.GetType().Name} !");
                Thread.Sleep(1000);
                return plante;
            }
            else
            {
                Console.WriteLine("\nPas assez de graines pour cet achat.");
                Thread.Sleep(1000);
                continue;
            }
        }
    }

    public void AfficherPotagerMasque(List<(int x, int y)> targets, HashSet<(int x, int y)> proteges, (int x, int y) cursor)
    {
        Console.Clear();
        Console.WriteLine("=== MODE URGENCE : PROTÉGER LES PLANTES ===\n");

        for (int y = 0; y < Hauteur; y++)
        {
            for (int x = 0; x < Largeur; x++)
            {
                bool estCurseur = (x == cursor.x && y == cursor.y);
                bool estProtege = proteges.Contains((x, y));
                bool estCible = targets.Contains((x, y));

                // Couleur du fond si sur le curseur
                Console.BackgroundColor = estCurseur ? ConsoleColor.DarkCyan : ConsoleColor.Black;

                if (estProtege)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" 🛡️ ");
                }
                else if (estCible)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" 🌱 ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write(" XX ");
                }

                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }


}