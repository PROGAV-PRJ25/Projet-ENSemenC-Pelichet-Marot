public class GestionPlateau
{
    private readonly Terrain[,] _plateau;
    private readonly Graines _graines;
    private VuePotager _vue;
    private Meteo _meteo;
    private Compost? _compostActuel = null;
    public bool CompostExiste() => _compostActuel != null;
    public int CurseurX { get; private set; }
    public int CurseurY { get; private set; }
    public int Hauteur => _plateau.GetLength(0);
    public int Largeur => _plateau.GetLength(1);

    // Injecte les graines et la vue dès la construction
    public GestionPlateau(Terrain[,] plateau, VuePotager vue, Graines graines)
    {
        _plateau = plateau;
        _vue = vue;
        _graines = graines;
        CurseurX = CurseurY = 0;
    }

    public void SetMeteo(Meteo meteo)
    {
        _meteo = meteo;

        // --- NOUVEAU : on propage immédiatement la météo à toutes les plantes ---
        // Cela permet de mettre à jour TemperatureActuelle et LuminositeActuelle
        // avant l'affichage final, évitant tout décalage.
        for (int y = 0; y < _plateau.GetLength(0); y++)
        {
            for (int x = 0; x < _plateau.GetLength(1); x++)
            {
                var p = _plateau[y, x].Plante;
                if (p != null && !p.EstMorte)
                {
                    // Met à jour la température
                    p.SetTemperature(meteo.Temperature);
                    // Met à jour l’indice de luminosité
                    p.SetLuminosite(meteo.Luminosite);
                }
            }
        }
    }



    public void MettreAJourPotager(Meteo meteo)
    {
        for (int y = 0; y < _plateau.GetLength(0); y++)
            for (int x = 0; x < _plateau.GetLength(1); x++)
            {
                var terrain = _plateau[y, x];
                var plante = terrain.Plante;
                if (plante == null || plante.EstMorte) continue;

                if (plante.ObstacleActuel == null)
                {
                    var obs = GenerateurObstacle.GenererObstacle();
                    if (obs != null)
                        plante.PlacerObstacle(obs);
                }

                bool espaceOk = IsEspacementOk(x, y);
                plante.Update(
                    tempsEcouleEnSemaines: 1f,
                    temperatureSemaine: meteo.Temperature,
                    espaceRespecte: espaceOk,
                    coeffAbsorptionEau: terrain.CoeffAbsorptionEau,
                    luminositeSemaine: meteo.Luminosite,
                    saisonActuelle: meteo.SaisonActuelle,
                    terrainActuel: terrain
                );
            }
    }

    public void GererInteractionUtilisateur(out bool avancerSemaine, out bool quitterSimulation)
    {
        avancerSemaine = quitterSimulation = false;
        bool actionTerminee = false;
        while (!actionTerminee)
        {
            _vue.AfficherPlateau();
            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.UpArrow: MoveCurseur(0, -1); break;
                case ConsoleKey.DownArrow: MoveCurseur(0, 1); break;
                case ConsoleKey.LeftArrow: MoveCurseur(-1, 0); break;
                case ConsoleKey.RightArrow: MoveCurseur(1, 0); break;

                case ConsoleKey.Spacebar:
                    HandleCaseAction();
                    actionTerminee = true;
                    break;

                case ConsoleKey.E:
                    avancerSemaine = true;
                    actionTerminee = true;
                    break;

                case ConsoleKey.Q:
                    quitterSimulation = true;
                    actionTerminee = true;
                    break;

                case ConsoleKey.I:
                    HandleInfo();
                    break;
            }
        }
    }

    private void HandleInfo()
    {
        var terrain = _plateau[CurseurY, CurseurX];
        _vue.AfficherPlanteOuTerrain(terrain, CurseurX, CurseurY);
        Console.WriteLine("\n\n[Appuyez sur Espace pour revenir]");
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
        Thread.Sleep(1000);
    }

    private void MoveCurseur(int dx, int dy)
    {
        int nx = CurseurX + dx, ny = CurseurY + dy;
        if (nx >= 0 && nx < _plateau.GetLength(1)
         && ny >= 0 && ny < _plateau.GetLength(0))
        {
            CurseurX = nx;
            CurseurY = ny;
        }
    }

    private void HandleCaseAction()
    {
        var terrain = _plateau[CurseurY, CurseurX];
        _vue.AfficherActionsCase(terrain);

        bool choixFait = false;
        while (!choixFait)
        {
            var key = Console.ReadKey(true).Key;
            if (terrain.Plante is Compost)
            {
                if (key == ConsoleKey.E)
                {
                    terrain.Plante = null;
                    _compostActuel = null;
                    Console.WriteLine("\nLe compost a été retiré.");
                    Thread.Sleep(1000);
                }
                return; // on sort sans afficher les autres choix
            }
            switch (key)
            {
                case ConsoleKey.A: //Arroser
                    if (terrain.Plante != null)
                    {
                        var p = terrain.Plante;
                        const int coutArrosage = 5;

                        if (p.EstMorte)
                        {
                            Console.WriteLine("\n💀 La plante est morte. Il faut désherber !");
                        }
                        else if (_graines.PeutDepenser(coutArrosage))
                        {
                            terrain.Plante.Arroser();
                            _graines.Depenser(coutArrosage);
                            Console.WriteLine($"\nArrosage ! (-{coutArrosage} graines)");
                        }
                        else
                        {
                            Console.WriteLine("\nPas assez de graines pour arroser !");
                        }
                        Thread.Sleep(1000);
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.D: // Désherber
                    if (terrain.Plante != null)
                    {
                        // Si c’était le compost, on le supprime proprement
                        if (terrain.Plante is Compost)
                        {
                            _compostActuel = null;
                        }
                        else if (_compostActuel != null)
                        {
                            _compostActuel.AjouterRemplissage();
                        }

                        terrain.Plante = null;
                        Console.WriteLine("\n🪓 Plante désherbée.");
                        Thread.Sleep(1000);
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.R: //Récolter
                    if (terrain.Plante != null)
                    {
                        var p = terrain.Plante;

                        if (p.EstMorte)
                        {
                            Console.WriteLine("\n💀 La plante est morte. Il faut désherber !");
                        }
                        else if (!p.EstMature)
                        {
                            Console.WriteLine("\n🌱 La plante est encore en croissance...");
                        }
                        else if (p.EstVivace && !p.PeutProduireFruits)
                        {
                            Console.WriteLine("\n⏳ En attente de refloraison... Patience !");
                        }
                        else
                        {
                            int gain = p.Recolter();
                            _graines.Ajouter(gain);
                            Console.WriteLine($"\n✅ Récolté ! +{gain} graines obtenues.");

                            if (!p.EstVivace)
                                terrain.Plante = null;
                        }

                        Thread.Sleep(2000);
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.S: // Soigner
                    if (terrain.Plante != null && terrain.Plante.ObstacleActuel is Maladie)
                    {
                        var p = terrain.Plante;
                        const int coutSoin = 10;

                        if (p.EstMorte)
                        {
                            Console.WriteLine("\n💀 La plante est morte. Il faut désherber !");
                        }
                        else if (_graines.PeutDepenser(coutSoin))
                        {
                            terrain.Plante.LancerTraitement();
                            _graines.Depenser(coutSoin);
                            Console.WriteLine("\n🧪 Traitement lancé. La plante sera soignée au prochain tour.");
                        }
                        else
                        {
                            Console.WriteLine("\n❌ Pas assez de graines pour soigner !");
                        }
                        Thread.Sleep(1500);
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.O: // Ajouter ombrelle
                    if (terrain.Plante != null && terrain.Plante.Accessoire == Plante.Equipement.Aucun)
                    {
                        var p = terrain.Plante;
                        if (p.EstMorte)
                        {
                            Console.WriteLine("\n💀 La plante est morte. Il faut désherber !");
                        }
                        else if (_graines.PeutDepenser(10))
                        {
                            terrain.Plante.Equiper(Plante.Equipement.Ombrelle);
                            _graines.Depenser(10);
                            Console.WriteLine("\n☂️ Ombrelle installée !");
                        }
                        else
                        {
                            Console.WriteLine("\n❌ Pas assez de graines !");
                        }
                        Thread.Sleep(1500);
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.Z:
                    if (terrain.Plante != null && terrain.Plante.Accessoire == Plante.Equipement.Aucun)
                    {
                        var p = terrain.Plante;
                        if (p.EstMorte)
                        {
                            Console.WriteLine("\n💀 La plante est morte. Il faut désherber !");
                        }
                        else if (_graines.PeutDepenser(10))
                        {
                            terrain.Plante.Equiper(Plante.Equipement.Serre);
                            _graines.Depenser(10);
                            Console.WriteLine("\n🏠 Serre installée !");
                        }
                        else
                        {
                            Console.WriteLine("\n❌ Pas assez de graines !");
                        }
                        Thread.Sleep(1500);
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.X: // Retirer équipement
                    if (terrain.Plante != null && terrain.Plante.Accessoire != Plante.Equipement.Aucun)
                    {
                        terrain.Plante.Desequiper();
                        Console.WriteLine("\n✅ L'équipement a été retiré.");
                        Thread.Sleep(1500);
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.P: // Planter
                    if (terrain.Plante == null)
                    {
                        if (terrain is TerrainAquatique)
                        {
                            Console.WriteLine("\n\nVous ne pouvez pas planter sur un terrain aquatique 🌊");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            var nouvelle = _vue.ChoisirNouvellePlante();
                            if (nouvelle != null)
                            {
                                terrain.Plante = nouvelle;
                                nouvelle.SetTemperature(_meteo.Temperature);
                                nouvelle.SetLuminosite(_meteo.Luminosite);

                                if (nouvelle is Compost compost)
                                    _compostActuel = compost;
                            }
                        }
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.Spacebar:
                    choixFait = true;
                    break;
            }
        }
    }

    private bool IsEspacementOk(int x, int y)
    {
        var plante = _plateau[y, x].Plante;
        if (plante == null || plante.EspacePris <= 1) return true;
        int rayon = plante.EspacePris - 1;
        int y0 = Math.Max(0, y - rayon), y1 = Math.Min(_plateau.GetLength(0) - 1, y + rayon);
        int x0 = Math.Max(0, x - rayon), x1 = Math.Min(_plateau.GetLength(1) - 1, x + rayon);
        for (int i = y0; i <= y1; i++)
            for (int j = x0; j <= x1; j++)
                if (!(i == y && j == x) && _plateau[i, j].Plante != null)
                    return false;
        return true;
    }

    public bool CheckEspaceRespecte(int x, int y) => IsEspacementOk(x, y);

    public int EspacementActuel(int x, int y)
    {
        var cible = _plateau[y, x].Plante;
        if (cible == null) return -1;
        int minDist = int.MaxValue;
        int h = _plateau.GetLength(0), w = _plateau.GetLength(1);
        for (int yy = 0; yy < h; yy++)
            for (int xx = 0; xx < w; xx++)
                if (!(xx == x && yy == y) && _plateau[yy, xx].Plante != null)
                {
                    int dist = Math.Max(Math.Abs(xx - x), Math.Abs(yy - y));
                    if (dist < minDist) minDist = dist;
                }
        return minDist == int.MaxValue ? -1 : minDist;
    }

    public List<(int x, int y)> Choisir5PlantesAleatoires()
    {
        var toutes = new List<(int x, int y)>();
        int h = _plateau.GetLength(0), w = _plateau.GetLength(1);
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                if (_plateau[y, x].Plante != null)
                    toutes.Add((x, y));

        // Mélange
        var rnd = new Random();
        for (int i = toutes.Count - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            (toutes[i], toutes[j]) = (toutes[j], toutes[i]);
        }

        // On retourne au plus 5
        return toutes.GetRange(0, Math.Min(5, toutes.Count));
    }
    public void SetCompostActuel(Compost compost)
    {
        _compostActuel = compost;
    }
    public Terrain GetTerrain(int x, int y)
    {
        return _plateau[y, x];
    }


}
