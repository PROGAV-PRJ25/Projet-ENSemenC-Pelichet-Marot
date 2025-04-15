Console.WriteLine("💧 TEST HYDRATATION (Vitesse=2f) 🌱\n");
var soja = new Soja();
Console.WriteLine($"DÉSHYDRATATION DE BASE: {soja.VitesseDeshydratation}%/jour");
Console.WriteLine($"PLAGE IDÉALE: {soja.TemperatureMinimale}°C à {soja.TemperatureMaximale}°C\n");
// Initialisation à 100%
soja.Arroser(100);
// Simulation sur 20 jours
for (int jour = 1; jour <= 20; jour++)
{
    float temperature = jour <= 10 ? 25f : 40f; // 10j normaux + 10j canicule

    Console.WriteLine($"\n--- JOUR {jour} ({temperature}°C) ---");
    soja.Update(temperature, 1f);

    Console.WriteLine($"Hydratation: {soja.Hydratation:F1}%");
    Console.WriteLine($"Stress: {(soja.EstEnStressThermique ? "🔴 OUI" : "🟢 NON")}");
    Console.WriteLine($"Jours hors limite: {soja.JoursHorsLimiteTemperature}/10");

    if (soja.Hydratation <= 0)
    {
        Console.WriteLine("💀 MORT PAR DÉSHYDRATATION");
        break;
    }
    if (soja.JoursHorsLimiteTemperature >= 10)
    {
        Console.WriteLine("🔥 MORT PAR TEMPÉRATURE");
        break;
    }
}