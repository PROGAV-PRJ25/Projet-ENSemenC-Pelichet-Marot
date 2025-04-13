Console.WriteLine("=== TEST POTAGER ===");

// 1. Création d'un plant de soja
var soja = new Soja();
Console.WriteLine($"Nouveau plant créé : {soja.NomPlante}");
Console.WriteLine($"Hydratation initiale : {soja.Hydratation}%");
Console.WriteLine();

// 2. Test d'arrosage normal
Console.WriteLine("-> Arrosage avec 80 unités d'eau :");
soja.Arroser(80);
Console.WriteLine($"Hydratation : {soja.Hydratation}%");
soja.VerifierSante();
soja.Pousser();
Console.WriteLine();

// 3. Simulation du temps qui passe (2 jours)
Console.WriteLine("-> 2 jours sans arrosage :");
soja.Update(2); // 2 jours * 25% de perte/jour = 50%
Console.WriteLine($"Hydratation : {soja.Hydratation}%");
soja.VerifierSante(); // Doit afficher l'avertissement
Console.WriteLine();

// 4. Test de sécheresse mortelle
Console.WriteLine("-> 4 jours supplémentaires sans eau :");
soja.Update(4); // 4j * 25% = 100% de perte
Console.WriteLine($"Hydratation : {soja.Hydratation}%");
soja.VerifierSante(); // Doit afficher la mort
soja.Pousser();
Console.WriteLine();

// 5. Test de résurrection (optionnel)
Console.WriteLine("-> Arrosage post-mortem :");
soja.Arroser(200);
Console.WriteLine($"Hydratation : {soja.Hydratation}%"); // Doit rester à 0%
soja.VerifierSante();
