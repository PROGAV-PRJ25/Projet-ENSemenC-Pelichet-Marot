using System;
using System.Threading;

public class GestionPotager
{
    private readonly GestionPlateau _controller;
    private readonly VuePotager _vue;
    private readonly Graines _graines;
    private readonly ModeUrgenceManager _urgence;       // ← nouvel ajout
    private Saison _saisonActuelle;
    private int _semaineActuelle;
    private bool _simulationEnCours;
    private readonly Random _rng = new Random();
    private readonly double _urgenceProba = 0.1; // 10% par défaut

    public GestionPotager(int largeur, int hauteur)
    {
        // 1) Génération du plateau
        var plateau = GenerateurBiome.GenererPlateau(largeur, hauteur);

        // 2) Porte-monnaie de graines
        _graines = new Graines(initial: 200);

        // 3) Vue sans controller
        _vue = new VuePotager(plateau, _graines);

        // 4) Controller avec vue + graines
        _controller = new GestionPlateau(plateau, _vue, _graines);

        // 5) Liaison circulaire vue→controller
        _vue.SetController(_controller);

        // 6) Mode urgence manager
        _urgence = new ModeUrgenceManager(
            new NettoyagePotagerGame(_graines),
            new OrageGame(_controller, _vue, _graines)
        );

        // 7) État initial
        _saisonActuelle = new SaisonPluvieuse();
        _semaineActuelle = 1;
        _simulationEnCours = true;
    }

    public void LancerSimulation()
    {
        while (_simulationEnCours)
        {
            // a) Météo de la semaine courante
            var meteo = Meteo.GenererPourSaison(_saisonActuelle, _semaineActuelle);

            // b) Mise à jour du contrôleur & de la vue
            _controller.SetMeteo(meteo);
            _vue.SetMeteo(meteo);

            // c) Affichage
            _vue.AfficherPlateau();

            // d) Entrées utilisateur
            _controller.GererInteractionUtilisateur(
                out bool avancerSemaine,
                out bool quitterSimulation
            );
            if (quitterSimulation)
                break;

            if (avancerSemaine)
            {
                // 1) Mode urgence aléatoire…
                if (_rng.NextDouble() < _urgenceProba)
                {
                    Thread.Sleep(1000);
                    _urgence.LancerUrgence();
                    continue; // on reste sur la même semaine
                }

                // 2) Quand on confirme la semaine, là *seulement* on met à jour les plantes
                _controller.MettreAJourPotager(meteo);

                // 3) On incrémente la semaine et change la saison si besoin
                _semaineActuelle++;
                if ((_semaineActuelle - 1) % 26 == 0)
                    _saisonActuelle = _saisonActuelle is SaisonPluvieuse
                        ? new SaisonSeche()
                        : new SaisonPluvieuse();
            }
        }

        Console.WriteLine("Simulation arrêtée par l'utilisateur.");
    }

    public void ArreterSimulation() => _simulationEnCours = false;
}
