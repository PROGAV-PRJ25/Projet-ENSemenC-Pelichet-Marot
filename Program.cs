var soja = new Soja();
float[] temperatures = new float[]
        {
            25f, // OK
            38f, // Trop chaud
            5f,  // Trop froid
            32f, // OK
            40f, // Trop chaud
            12f, // OK
        };

float joursSimules = 0;
float pasDeTemps = 1f; 

Console.WriteLine($"🌱 Début de la simulation pour {soja.NomPlante}\n");

foreach (float temp in temperatures)
{
    joursSimules += pasDeTemps;

    Console.WriteLine($"\n📅 Jour {joursSimules} — Température : {temp}°C");

    soja.EffetTemperature(temp, pasDeTemps);
    soja.Update(pasDeTemps);

    Console.WriteLine($"💧 Hydratation actuelle : {soja.Hydratation:F1}%");

    if (soja.ASoif)
    {
        Console.WriteLine($"🚿 {soja.NomPlante} a soif ! Tentative d'arrosage...");
        soja.Arroser(50); // Quantité arbitraire
    }

    soja.VerifierSante();

    if (soja.Hydratation <= 0)
    {
        Console.WriteLine("💀 Fin de la simulation : plante morte.");
        break;
    }

    soja.Pousser();

    Thread.Sleep(1000); // Juste pour l'effet de tempo, tu peux l'enlever si tu veux
}

Console.WriteLine("\n✅ Simulation terminée.");