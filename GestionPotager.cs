using System;
using System.Threading;



    public class GestionPotager
    {
        // 1) Champs internes
        private readonly Terrain[,] _plateau;
        private readonly GestionPlateau _controller;
        private readonly VuePotager _vue;
        private Graines _graines;
        private readonly ModeUrgenceManager _urgence;
        private Saison _saisonActuelle;
        private int _semaineActuelle;
        private bool _simulationEnCours;
        private readonly Random _rng = new Random();
        private readonly double _urgenceProba = 0.1;

        // 2) Constructeur
        public GestionPotager(int largeur, int hauteur)
        {
            // Plateau
            _plateau = GenerateurBiome.GenererPlateau(largeur, hauteur);

            // Argent (graines)
            _graines = new Graines(initial: 200);

            // Vue + Controller
            _vue = new VuePotager(_plateau, _graines);
            _controller = new GestionPlateau(_plateau, _vue, _graines);
            _vue.SetController(_controller);

            // Mode urgence
            _urgence = new ModeUrgenceManager(
                new NettoyagePotagerGame(_graines),
                new OrageGame(_controller, _vue, _graines)
            );

            // État initial
            _saisonActuelle  = new SaisonPluvieuse();
            _semaineActuelle = 1;
            _simulationEnCours = true;
        }

        // —————————————————————————————————————————————————————————————
        // 3) API publique pour sauvegarde / rechargement / inspection
        public int GetSemaine()              => _semaineActuelle;
        public void SetSemaine(int s)        => _semaineActuelle = s;

        public Graines GetGraines()         => _graines;
        public void SetGraines(int n)        => _graines = new Graines(initial: n);

        public Terrain[,] GetPlateau()       => _plateau;
        public int GetLargeur()              => _plateau.GetLength(1);
        public int GetHauteur()              => _plateau.GetLength(0);

        public Plante CreerPlanteParNom(string type)
        {
            // Adapte les noms exactement à ceux retournés par PlantCell.TypePlante
            return type switch
            {
                "Soja"           => new Soja(_graines),
                "Maïs"           => new Mais(_graines),
                "Canne à sucre"  => new CanneASucre(_graines),
                "Café"           => new Cafe(_graines),
                "Cactus"         => new Cactus(_graines),
                "Coton"          => new Coton(_graines),
                _                => null
            };
        }
        // —————————————————————————————————————————————————————————————

        // 4) Boucle principale inchangée (juste placée après l’API)
        public void LancerSimulation()
        {
            // météo initiale
            var meteo = Meteo.GenererPourSaison(_saisonActuelle, _semaineActuelle);
            _controller.SetMeteo(meteo);
            _vue.SetMeteo(meteo);

            while (_simulationEnCours)
            {
                _vue.AfficherPlateau();
                _controller.GererInteractionUtilisateur(
                    out bool avancerSemaine,
                    out bool quitterSimulation
                );
                if (quitterSimulation) break;

                if (avancerSemaine)
                {
                    // urgence ?
                    if (_rng.NextDouble() < _urgenceProba)
                    {
                        Thread.Sleep(1000);
                        _urgence.LancerUrgence();
                        continue;
                    }

                    // mise à jour plantes
                    _controller.MettreAJourPotager(meteo);

                    // semaine++ & changement de saison
                    _semaineActuelle++;
                    if ((_semaineActuelle - 1) % 26 == 0)
                        _saisonActuelle = _saisonActuelle is SaisonPluvieuse
                            ? (Saison)new SaisonSeche()
                            : new SaisonPluvieuse();

                    // nouvelle météo
                    meteo = Meteo.GenererPourSaison(_saisonActuelle, _semaineActuelle);
                    _controller.SetMeteo(meteo);
                    _vue.SetMeteo(meteo);
                }
            }

            Console.WriteLine("Simulation arrêtée par l'utilisateur.");
        }

        public void ArreterSimulation() => _simulationEnCours = false;
    }

