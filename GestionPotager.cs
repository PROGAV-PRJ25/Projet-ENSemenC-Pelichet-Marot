using System;
using System.Threading;

public class GestionPotager
{
    // ── 1) Champs internes ────────────────────────────────────────────
    private readonly Terrain[,]      _plateau;
    private readonly GestionPlateau  _controller;
    private readonly VuePotager      _vue;
    private          Graines         _graines;
    private readonly ModeUrgenceManager _urgence;

    private Saison   _saisonActuelle;
    private Meteo    _derniereMeteo;
    public  Meteo    GetDerniereMeteo() => _derniereMeteo;

    private int      _semaineActuelle;
    private bool     _simulationEnCours;
    private readonly Random   _rng         = new Random();
    private readonly double   _urgenceProba = 0.1;

    // ── 2a) Nouveau constructeur : partie NEUVE ───────────────────────
    public GestionPotager(int largeur, int hauteur)
    {
        // Génération aléatoire du plateau
        _plateau        = GenerateurBiome.GenererPlateau(largeur, hauteur);

        // Monnaie
        _graines        = new Graines(initial: 200);

        // Vue & controller
        _vue            = new VuePotager(_plateau, _graines);
        _controller     = new GestionPlateau(_plateau, _vue, _graines);
        _vue.SetController(_controller);

        // Mode urgence
        _urgence = new ModeUrgenceManager(
            new NettoyagePotagerGame(_graines),
            new OrageGame(_controller, _vue, _graines)
        );

        // État initial
        _saisonActuelle   = new SaisonPluvieuse();
        _semaineActuelle  = 1;
        _simulationEnCours= true;

        // Calculer et stocker la première météo
        var meteo = Meteo.GenererPourSaison(_saisonActuelle, _semaineActuelle);
        _derniereMeteo = meteo;
        _controller.SetMeteo(meteo);
        _vue.SetMeteo(meteo);
    }

    // ── 2b) Nouveau constructeur : RESTAURATION depuis une sauvegarde ───
    public GestionPotager(
        Terrain[,] plateauChargé,
        int        grainesInitiales,
        int        semaineInitiale,
        Saison     saisonInitiale,
        Meteo      meteoInitial
    )
    {
        // On réutilise exactement le plateau chargé
        _plateau        = plateauChargé;

        // Monnaie restaurée
        _graines        = new Graines(grainesInitiales);

        // Vue & controller sur ce plateau
        _vue            = new VuePotager(_plateau, _graines);
        _controller     = new GestionPlateau(_plateau, _vue, _graines);
        _vue.SetController(_controller);

        // Mode urgence (inchangé)
        _urgence = new ModeUrgenceManager(
            new NettoyagePotagerGame(_graines),
            new OrageGame(_controller, _vue, _graines)
        );

        // État restauré
        _saisonActuelle   = saisonInitiale;
        _semaineActuelle  = semaineInitiale;
        _simulationEnCours= true;

        // Injecte directement la météo restaurée
        _derniereMeteo    = meteoInitial;
        _controller.SetMeteo(meteoInitial);
        _vue.SetMeteo(meteoInitial);
    }

    /// Permet d’injecter la météo après construction si nécessaire
    public void SetInitialMeteo(Meteo meteo)
    {
        _derniereMeteo = meteo;
        _controller.SetMeteo(meteo);
        _vue.SetMeteo(meteo);
    }

    // ── 3) API pour la sauvegarde / inspection ────────────────────────
    public int       GetSemaine()     => _semaineActuelle;
    public void      SetSemaine(int s)=> _semaineActuelle = s;

    public Graines  GetGraines()      => _graines;
    public void      SetGraines(int n)=> _graines = new Graines(initial: n);

    public Terrain[,] GetPlateau()    => _plateau;
    public int        GetLargeur()    => _plateau.GetLength(1);
    public int        GetHauteur()    => _plateau.GetLength(0);

    public Plante    CreerPlanteParNom(string type)
    {
        return type switch
        {
            "Soja"          => new Soja(_graines),
            "Maïs"          => new Mais(_graines),
            "Canne à sucre" => new CanneASucre(_graines),
            "Café"          => new Cafe(_graines),
            "Cactus"        => new Cactus(_graines),
            "Coton"         => new Coton(_graines),
            _               => null
        };
    }

    // ── 4) Boucle de jeu ───────────────────────────────────────────────
    public void LancerSimulation()
    {
        // _derniereMeteo est déjà initialisée en constructeur
        while (_simulationEnCours)
        {
            // 1) Affiche tout
            _vue.AfficherPlateau();

            // 2) Lecture des touches
            _controller.GererInteractionUtilisateur(
                out bool avancerSemaine,
                out bool quitterSimulation
            );
            if (quitterSimulation) break;

            if (avancerSemaine)
            {
                // 3) Mode urgence aléatoire
                if (_rng.NextDouble() < _urgenceProba)
                {
                    Thread.Sleep(1000);
                    _urgence.LancerUrgence();
                    continue;  // même semaine
                }

                // 4) Mise à jour des plantes avec la météo courante
                _controller.MettreAJourPotager(_derniereMeteo);

                // 5) Incrément de la semaine + rotation saisonnière
                _semaineActuelle++;
                if ((_semaineActuelle - 1) % 26 == 0)
                {
                    _saisonActuelle = _saisonActuelle is SaisonPluvieuse
                        ? (Saison)new SaisonSeche()
                        : new SaisonPluvieuse();
                }

                // 6) Génération et stockage de la nouvelle météo
                _derniereMeteo = Meteo.GenererPourSaison(
                    _saisonActuelle,
                    _semaineActuelle
                );
                _controller.SetMeteo(_derniereMeteo);
                _vue.SetMeteo(_derniereMeteo);
            }
        }

        Console.WriteLine("Simulation arrêtée par l'utilisateur.");
    }

    public void ArreterSimulation()
        => _simulationEnCours = false;
}
