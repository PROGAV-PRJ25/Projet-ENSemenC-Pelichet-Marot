while (true)
{
    var choix = StartMenu.Show();
    switch (choix)
    {
        case MenuOption.NouvellePartie:
            var gestion = new GestionPotager(10, 10);
            gestion.LancerSimulation();
            break;

        case MenuOption.ChargerSauvegarde:
            Console.Clear();
            Console.WriteLine(">> Fonctionnalité de chargement à implémenter.");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu.");
            Console.ReadKey(true);
            break;

        case MenuOption.ReglesDuJeu:
            Console.Clear();
            Console.WriteLine(">> Voici les règles du jeu :");
            Console.WriteLine("1) Planter…\n2) Arroser…\n3) Récolter…");
            Console.WriteLine("\nAppuyez sur une touche pour revenir au menu.");
            Console.ReadKey(true);
            break;

        case MenuOption.Quitter:
            return;
    }
}