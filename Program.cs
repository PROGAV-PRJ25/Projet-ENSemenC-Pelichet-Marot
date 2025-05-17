const int largeur = 10, hauteur = 10;

while (true)
{
    // 1) Affiche l'écran d'accueil
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
                continue; // retour au menu principal
            break;

        case MenuOption.ReglesDuJeu:
            AfficherRegles();
            continue;
    }

        if (sim != null)
    {
        sim.LancerSimulation();
        MenuSauvegarder(sim);
    }
}


// ── Affiche les règles et attend une touche ─────────────────────
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

// ── Menu pour charger ou supprimer des sauvegardes ───────────────
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
        Console.WriteLine("  0      : Retour au menu principal");
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
            string toDelete = slots[delIdx - 1];
            System.IO.File.Delete(System.IO.Path.Combine("Saves", toDelete + ".json"));
            Console.WriteLine($"Slot « {toDelete} » supprimé.");
            Console.WriteLine("Appuyez sur une touche pour actualiser la liste.");
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
            // 1) Recréer le plateau à l’identique
            var plateau = new Terrain[hauteur, largeur];
            foreach (var tcell in data.Terrains)
            {
                plateau[tcell.Y, tcell.X] =
                    GenerateurBiome.CreerTerrainParNom(tcell.TypeTerrain);
            }

            // 2) Construire GestionPotager avec ce plateau et la météo restaurée
            var meteoInit = new Meteo(
                pluie: data.MeteoPluie,
                luminosite: data.MeteoLuminosite,
                temperature: data.MeteoTemperature,
                intemperie: data.MeteoIntemperie,
                saison: data.MeteoSaison == "Pluvieuse"
                               ? (Saison)new SaisonPluvieuse()
                               : new SaisonSeche(),
                semaineActuelle: data.Semaine
            );
            var sim = new GestionPotager(
                plateauChargé: plateau,
                grainesInitiales: data.Graines,
                semaineInitiale: data.Semaine,
                saisonInitiale: meteoInit.SaisonActuelle,
                meteoInitial: meteoInit
            );

            // 3) Puis restaurer toutes les plantes (+ obstacles)
            foreach (var cell in data.Plantes)
            {
                var p = sim.CreerPlanteParNom(cell.TypePlante);
                if (p == null) continue;

                // Restorer l’état complet
                p.HauteurActuelle = cell.HauteurActuelle;
                p.HydratationActuelle = cell.HydratationActuelle;
                p.LuminositeActuelle = cell.LuminositeActuelle;
                p.TemperatureActuelle = cell.TemperatureActuelle;
                p.SemainesDepuisPlantation = cell.SemainesDepuisPlantation;
                p.SommeSatisfaction = cell.SommeSatisfaction;
                p.RendementBase = cell.RendementBase;
                p.PeutProduireFruits = cell.PeutProduireFruits;
                p.SemainesDepuisDerniereRecolte = cell.DerniereRecolte;

                // Restauration de l’équipement
                if (Enum.TryParse<Plante.Equipement>(cell.Equipement, out var eq))
                    p.Equiper(eq);

                if (cell.EstMorte) p.Tuer();

                // Placement
                plateau[cell.Y, cell.X].Plante = p;

                // Restauration compost
                if (p is Compost compost)
                {
                    compost.SetRemplissage(cell.RemplissageCompost);
                    sim.SetCompostActuel(compost);
                }


                // Obstacle
                if (!string.IsNullOrEmpty(cell.ObstacleNom) && p is not Compost)
                {
                    var obs = GenerateurObstacle.CreerParNom(cell.ObstacleNom);
                    if (obs != null) p.PlacerObstacle(obs);
                }

            }


            return sim;
        }

        // Choix invalide : on boucle
        Console.WriteLine("Choix invalide, réessayez…");
        System.Threading.Thread.Sleep(800);
    }
}

// ── Menu pour sauvegarder la partie en cours ─────────────────────
static void MenuSauvegarder(GestionPotager sim)
{
    // On force l'utilisateur à répondre S ou N
    while (true)
    {
        Console.Clear();
        Console.WriteLine("Sauvegarder la partie ? (S/N)");
        var key = Console.ReadKey(true).Key;
        if (key == ConsoleKey.N) return;
        if (key == ConsoleKey.S) break;
    }

    // Liste des slots existants
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

    // Délègue entièrement à SaveManager
    SaveManager.Sauvegarder(slotName, sim);

    Console.WriteLine($"✅ Partie sauvegardée sous « {slotName} » !");
    Console.WriteLine("Appuyez sur une touche pour revenir au menu principal.");
    Console.ReadKey(true);
}