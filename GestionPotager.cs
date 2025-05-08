using System;
public class GestionPotager
{
    private readonly GestionPlateau _controller;
    private readonly VuePotager     _vue;
    private Saison                  _saisonActuelle;
    private int                     _jourActuel;
    private bool                    _simulationEnCours;
    private Graines graines;

    public GestionPotager(int largeur, int hauteur)
    {
        // 1) Génération des terrains
        var plateau = GenerateurBiome.GenererPlateau(largeur, hauteur);
        graines = new Graines();

        // 2) Création du controller et de la vue
        _controller = new GestionPlateau(plateau);
        _vue        = new VuePotager(plateau, _controller, graines);
        _controller.SetVue(_vue);

        // 3) État initial
        _saisonActuelle    = new SaisonPluvieuse();
        _jourActuel        = 1;
        _simulationEnCours = true;
    }

    public void LancerSimulation()
{
    while (_simulationEnCours)
    {
        // 1) Génération de la météo
        var meteo = Meteo.GenererPourSaison(_saisonActuelle, _jourActuel);

        // 2) On informe à la fois le controller et la vue
        _controller.SetMeteo(meteo);
        _vue.SetMeteo(meteo);   

        // 3) Mise à jour des plantes
        _controller.MettreAJourPotager(meteo);

        // 4) Affichage du potager
        _vue.AfficherPlateau();

        // 5) Gestion des entrées
        _controller.GererInteractionUtilisateur(
            out bool avancerJour,
            out bool quitterSimulation
        );

        if (quitterSimulation) break;
        if (avancerJour)     _jourActuel++;
    }
    Console.WriteLine("Simulation arrêtée par l'utilisateur.");
}

    public void ArreterSimulation() => _simulationEnCours = false;
}