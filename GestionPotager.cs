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
        // ——————— 0) Initialisation : météo SEMAINE 1 ———————
        var meteo = Meteo.GenererPourSaison(_saisonActuelle, _semaineActuelle);
        _controller.SetMeteo(meteo);
        _vue.SetMeteo(meteo);

        while (_simulationEnCours)
        {
            // ——————— 1) Affichage (plateau + météo de la semaine ACTIVE) ———————
            _vue.AfficherPlateau();

            // ——————— 2) Saisie utilisateur ———————
            _controller.GererInteractionUtilisateur(
                out bool avancerSemaine,
                out bool quitterSimulation
            );
            if (quitterSimulation)
                break;

            if (avancerSemaine)
            {
                // ——— 3) Mode urgence ? ———
                if (_rng.NextDouble() < _urgenceProba)
                {
                    Thread.Sleep(1000);
                    _urgence.LancerUrgence();
                    continue; // on reste sur la même semaine
                }

                // ——— 4) MAJ plantes pour la semaine ACTIVE ———
                _controller.MettreAJourPotager(meteo);

                // ——— 5) Incrément semaine et changement de saison ———
                _semaineActuelle++;
                if ((_semaineActuelle - 1) % 26 == 0)
                {
                    _saisonActuelle = _saisonActuelle is SaisonPluvieuse
                        ? (Saison)new SaisonSeche()
                        : new SaisonPluvieuse();
                }

                // ——— 6) Génére et assigne la météo pour la NOUVELLE semaine ———
                meteo = Meteo.GenererPourSaison(_saisonActuelle, _semaineActuelle);
                _controller.SetMeteo(meteo);
                _vue.SetMeteo(meteo);
            }
        }

        Console.WriteLine("Simulation arrêtée par l'utilisateur.");
    }





    public void ArreterSimulation() => _simulationEnCours = false;
}
