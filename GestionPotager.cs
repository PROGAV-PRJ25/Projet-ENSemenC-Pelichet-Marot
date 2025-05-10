public class GestionPotager
{
    private readonly GestionPlateau _controller;
    private readonly VuePotager     _vue;
    private readonly Graines        _graines;
    private Saison                   _saisonActuelle;
    private int                      _semaineActuelle;
    private bool                     _simulationEnCours;

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

        // 5) On relie la vue au controller
        _vue.SetController(_controller);

        // 6) État initial
        _saisonActuelle    = new SaisonPluvieuse();
        _semaineActuelle   = 1;
        _simulationEnCours = true;
    }

    public void LancerSimulation()
    {
        while (_simulationEnCours)
        {
            // a) Génération de la météo pour la semaine en cours
            var meteo = Meteo.GenererPourSaison(_saisonActuelle, _semaineActuelle);

            // b) On informe le contrôleur et la vue
            _controller.SetMeteo(meteo);
            _vue.SetMeteo(meteo);

            // c) Mise à jour des plantes
            _controller.MettreAJourPotager(meteo);

            // d) Affichage du potager
            _vue.AfficherPlateau();

            // e) Gestion des entrées utilisateur
            _controller.GererInteractionUtilisateur(
                out bool avancerSemaine,
                out bool quitterSimulation
            );

            if (quitterSimulation)
                break;
            if (avancerSemaine)
            {
                // On passe à la semaine suivante
                _semaineActuelle++;

                // Toutes les 26 semaines, on change de saison
                // Semaine 1 → Pluvieuse, 27 → Sèche, 53 → Pluvieuse, etc.
                if ((_semaineActuelle - 1) % 26 == 0)
                {
                    _saisonActuelle = _saisonActuelle is SaisonPluvieuse
                        ? (Saison)new SaisonSeche()
                        : new SaisonPluvieuse();
                }
            }
        }

        Console.WriteLine("Simulation arrêtée par l'utilisateur.");
    }

    public void ArreterSimulation() => _simulationEnCours = false;
}
