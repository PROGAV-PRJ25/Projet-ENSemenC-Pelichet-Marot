public class ModeUrgenceManager
{
    private readonly IMiniJeu[] _jeux;
    private readonly Random     _rng = new();

    public ModeUrgenceManager(params IMiniJeu[] jeux)
    {
        _jeux = jeux;
    }

    public void LancerUrgence()
    {
        Console.Clear();
        Console.WriteLine("!!! MODE URGENCE !!!\n");
        Thread.Sleep(1000);

        // Choix aléatoire du jeu puis lancement
        var jeu = _jeux[_rng.Next(_jeux.Length)];
        jeu.Run();
    }
}
