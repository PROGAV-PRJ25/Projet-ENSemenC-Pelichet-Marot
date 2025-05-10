public class GestionPlateau
{
    private readonly Terrain[,] _plateau;
    private readonly Graines _graines;
    private VuePotager _vue;
    private Meteo _meteo;
    public int CurseurX { get; private set; }
    public int CurseurY { get; private set; }

    // Injecte les graines et la vue dès la construction
    public GestionPlateau(Terrain[,] plateau, VuePotager vue, Graines graines)
    {
        _plateau = plateau;
        _vue = vue;
        _graines = graines;
        CurseurX = CurseurY = 0;
    }

    public void SetMeteo(Meteo meteo) => _meteo = meteo;

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
            switch (key)
            {
                case ConsoleKey.A: // Arroser (coût fixe 5 graines)
                    if (terrain.Plante != null)
                    {
                        const int coutArrosage = 5;
                        if (_graines.PeutDepenser(coutArrosage))
                        {
                            terrain.Plante.Arroser();
                            _graines.Depenser(coutArrosage);
                            Console.WriteLine($"\nArrosage ! (-{coutArrosage} graines)");
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            Console.WriteLine("\nPas assez de graines pour arroser !");
                            Thread.Sleep(1000);
                        }
                    }
                    choixFait = true;
                    break;

                case ConsoleKey.D: // Désherber
                    Thread.Sleep(1000);
                    terrain.Plante = null;
                    choixFait = true;
                    break;

                case ConsoleKey.R: // Récolter
                    if (terrain.Plante != null)
                    {
                        int gain = terrain.Plante.Recolter();
                        if (gain > 0)
                        {
                            _graines.Ajouter(gain);
                            // on retire la plante du plateau
                            terrain.Plante = null;
                            Console.WriteLine($"\nRécolté ! +{gain} graines obtenues.");
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine("\nLa plante n'est pas encore mature.");
                            Thread.Sleep(2000);
                        }
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
                            terrain.Plante = _vue.ChoisirNouvellePlante();
                        }
                    }
                    choixFait = true;
                    break;


                case ConsoleKey.Spacebar: // Annuler
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

}
