public class GestionPlateau
{
    private readonly Terrain[,] _plateau;
    private VuePotager _vue;
    private Meteo _meteo;
    public int CurseurX { get; private set; }
    public int CurseurY { get; private set; }

    public GestionPlateau(Terrain[,] plateau)
    {
        _plateau = plateau;
        CurseurX = CurseurY = 0;
    }

    public void SetVue(VuePotager vue) => _vue = vue;
    public void SetMeteo(Meteo meteo) => _meteo = meteo;

    /// <summary>
    /// Met à jour chaque plante selon la météo et l'espacement.
    /// Si une plante meurt, on la retire du terrain.
    /// </summary>
    public void MettreAJourPotager(Meteo meteo)
    {
        for (int y = 0; y < _plateau.GetLength(0); y++)
            for (int x = 0; x < _plateau.GetLength(1); x++)
            {
                var terrain = _plateau[y, x];
                var plante = terrain.Plante;
                if (plante == null || plante.EstMorte) continue;

                bool espaceOk = IsEspacementOk(x, y);
                plante.Update(
                    tempsEcouleEnJours: 1f,
                    temperatureDuJour: meteo.Temperature,
                    espaceRespecte: espaceOk,
                    coeffAbsorptionEau: terrain.CoeffAbsorptionEau,
                    luminositeDuJour: meteo.Luminosite,
                    saisonActuelle: meteo.SaisonActuelle,
                    terrainActuel: terrain
                );

                if (plante.EstMorte)
                    terrain.Plante = null;
            }
    }

    /// <summary>
    /// Boucle unique pour traiter toutes les touches utiles :
    /// déplacement, action (espace), jour suivant (E), quitter (Q).
    /// </summary>
    public void GererInteractionUtilisateur(
        out bool avancerJour,
        out bool quitterSimulation
    )
    {
        avancerJour = false;
        quitterSimulation = false;

        bool actionTerminee = false;
        while (!actionTerminee)
        {
            _vue.AfficherPlateau();
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                // Déplacement du curseur
                case ConsoleKey.UpArrow: MoveCurseur(0, -1); break;
                case ConsoleKey.DownArrow: MoveCurseur(0, 1); break;
                case ConsoleKey.LeftArrow: MoveCurseur(-1, 0); break;
                case ConsoleKey.RightArrow: MoveCurseur(1, 0); break;

                // Action sur la case courante
                case ConsoleKey.Spacebar:
                    HandleCaseAction();
                    break;

                // Passer au jour suivant
                case ConsoleKey.E:
                    avancerJour = true;
                    actionTerminee = true;
                    break;

                // Quitter la simulation
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
        Console.WriteLine("\n[Appuyez sur Espace pour revenir]");
        while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
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
                case ConsoleKey.A: // Arroser
                    terrain.Plante?.Arroser();
                    choixFait = true;
                    break;

                case ConsoleKey.D: // Désherber
                    terrain.Plante = null;
                    choixFait = true;
                    break;

                case ConsoleKey.R: // Récolter
                    if (terrain.Plante != null)
                        terrain.Plante = null;
                    choixFait = true;
                    break;

                case ConsoleKey.P: // Planter
                    if (terrain.Plante == null)
                        terrain.Plante = _vue.ChoisirNouvellePlante();
                    choixFait = true;
                    break;

                case ConsoleKey.Spacebar: // Annuler
                    choixFait = true;
                    break;
            }
        }
    }

    /// <summary>
    /// Vérifie qu'aucune autre plante n'existe dans le Chebyshev-radius = EspacePris-1.
    /// Si EspacePris ≤ 1, l'espacement n'est pas requis.
    /// </summary>
    private bool IsEspacementOk(int x, int y)
    {
        var plante = _plateau[y, x].Plante;
        if (plante == null || plante.EspacePris <= 1)
            return true;

        int rayon = plante.EspacePris - 1;
        int y0 = Math.Max(0, y - rayon);
        int y1 = Math.Min(_plateau.GetLength(0) - 1, y + rayon);
        int x0 = Math.Max(0, x - rayon);
        int x1 = Math.Min(_plateau.GetLength(1) - 1, x + rayon);

        for (int i = y0; i <= y1; i++)
            for (int j = x0; j <= x1; j++)
            {
                if ((i == y && j == x)) continue;
                if (_plateau[i, j].Plante != null)
                    return false;
            }
        return true;
    }
    public bool CheckEspaceRespecte(int x, int y)
       => IsEspacementOk(x, y);
}