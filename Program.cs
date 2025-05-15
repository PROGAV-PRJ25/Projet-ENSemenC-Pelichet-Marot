using System;
using System.Collections.Generic;



const int largeur = 10, hauteur = 10;

while (true)
{
    // 1) Menu de départ
    var choix = StartMenu.Show();
    if (choix == MenuOption.Quitter)
        return;

    GestionPotager sim = null;

    switch (choix)
    {
        case MenuOption.NouvellePartie:
            sim = new GestionPotager(largeur, hauteur);
            break;

        case MenuOption.ChargerSauvegarde:
            sim = MenuChargerSauvegarde(largeur, hauteur);
            if (sim == null)
                continue;  // retour au menu principal
            break;

        case MenuOption.ReglesDuJeu:
            AfficherRegles();
            continue;
    }

    // 2) Lancer la simulation si elle existe
    sim.LancerSimulation();

    // 3) À la sortie, on force la question de sauvegarde
    MenuSauvegarder(sim);
}


// Affiche et gère l'écran des règles
static void AfficherRegles()
{
    Console.Clear();
    Console.WriteLine("=== RÈGLES DU POTAGER BRESIL ===\n");
    Console.WriteLine("1) Planter coûte des graines.");
    Console.WriteLine("2) Chaque semaine, vos plantes ont 7 conditions.");
    Console.WriteLine("3) ≥3 échecs → ORANGE ; ≥4 → ROUGE (mort).");
    Console.WriteLine("4) Actions : arroser, récolter, désherber, …");
    Console.WriteLine("5) Mode urgence aléatoire (mini-jeux).");
    Console.WriteLine("\nAppuyez sur une touche pour revenir.");
    Console.ReadKey(true);
}

// Menu pour charger ou supprimer des slots
static GestionPotager MenuChargerSauvegarde(int largeur, int hauteur)
{
    while (true)
    {
        var slots = SaveManager.ListerSlots();
        Console.Clear();
        if (slots.Count == 0)
        {
            Console.WriteLine("⚠️  Aucune sauvegarde trouvée.");
            Console.WriteLine("Appuyez sur une touche pour revenir.");
            Console.ReadKey(true);
            return null;
        }

        Console.WriteLine("=== Charger une sauvegarde ===\n");
        for (int i = 0; i < slots.Count; i++)
            Console.WriteLine($"  {i + 1}. {slots[i]}");
        Console.WriteLine("\n  D<num> : Supprimer le slot #num (ex: D2)");
        Console.WriteLine("  0 : Retour au menu principal");
        Console.Write("\nVotre choix : ");

        string input = Console.ReadLine()?.Trim() ?? "";
        if (input == "0")
            return null;

        // Supprimer un slot
        if ((input.StartsWith("D", StringComparison.OrdinalIgnoreCase) ||
             input.StartsWith("d", StringComparison.OrdinalIgnoreCase))
            && int.TryParse(input.Substring(1), out int delIdx)
            && delIdx >= 1 && delIdx <= slots.Count)
        {
            var slotToDelete = slots[delIdx - 1];
            System.IO.File.Delete(System.IO.Path.Combine("Saves", slotToDelete + ".json"));
            Console.WriteLine($"Slot « {slotToDelete} » supprimé.");
            Console.WriteLine("Appuyez sur une touche pour recharger la liste.");
            Console.ReadKey(true);
            continue;
        }

        // Charger un slot
        if (int.TryParse(input, out int idx)
            && idx >= 1 && idx <= slots.Count)
        {
            var data = SaveManager.Charger(slots[idx - 1]);
            if (data == null)
            {
                Console.WriteLine("Erreur de lecture. Appuyez sur une touche.");
                Console.ReadKey(true);
                return null;
            }

            // Reconstruire la simulation
            var sim = new GestionPotager(largeur, hauteur);
            sim.SetSemaine(data.Semaine);
            sim.SetGraines(data.Graines);

            var plateau = sim.GetPlateau();
            foreach (var cell in data.Plantes)
            {
                var p = sim.CreerPlanteParNom(cell.TypePlante);
                if (p != null)
                {
                    p.HauteurActuelle = cell.HauteurActuelle;
                    p.HydratationActuelle = cell.HydratationActuelle;
                    plateau[cell.Y, cell.X].Plante = p;
                }
            }
            return sim;
        }

        // Choix invalide
        Console.WriteLine("Choix invalide, réessayez.");
        System.Threading.Thread.Sleep(1000);
    }
}

// Menu pour sauvegarder après la simulation
static void MenuSauvegarder(GestionPotager sim)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Sauvegarder la partie ? (S/N)");
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.N)
            return; // on revient au menu principal
        if (key == ConsoleKey.S)
            break;
    }

    // Choix du slot
    var existing = SaveManager.ListerSlots();
    Console.Clear();
    Console.WriteLine("=== Sauvegarder la partie ===\n");
    for (int i = 0; i < existing.Count; i++)
        Console.WriteLine($"  {i + 1}. {existing[i]}");
    Console.WriteLine("  N. Nouveau slot");
    Console.Write("\nVotre choix (numéro ou N) : ");
    string input = Console.ReadLine()?.Trim() ?? "";
    string slotName;

    if (input.Equals("N", StringComparison.OrdinalIgnoreCase))
    {
        Console.Write("Nom du nouveau slot : ");
        slotName = Console.ReadLine()?.Trim() ?? "Slot";
    }
    else if (int.TryParse(input, out int idx)
             && idx >= 1 && idx <= existing.Count)
    {
        slotName = existing[idx - 1];
    }
    else
    {
        Console.WriteLine("Choix invalide, sauvegarde annulée.");
        Console.ReadKey(true);
        return;
    }

    // Construction de la donnée
    var sd = new SaveData
    {
        Semaine = sim.GetSemaine(),
        Graines = sim.GetGraines().Nombre
    };
    var plat = sim.GetPlateau();
    for (int y = 0; y < sim.GetHauteur(); y++)
        for (int x = 0; x < sim.GetLargeur(); x++)
        {
            var p = plat[y, x].Plante;
            if (p != null)
            {
                sd.Plantes.Add(new PlantCell
                {
                    X = x,
                    Y = y,
                    TypePlante = p.NomPlante,
                    HauteurActuelle = p.HauteurActuelle,
                    HydratationActuelle = p.HydratationActuelle
                });
            }
        }

    SaveManager.Sauvegarder(slotName, sd);
    Console.WriteLine($"✅ Partie sauvegardée sous « {slotName} » !");
    Console.WriteLine("Appuyez sur une touche pour revenir au menu principal.");
    Console.ReadKey(true);
}
