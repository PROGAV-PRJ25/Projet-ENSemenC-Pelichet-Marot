var saison = new SaisonSeche();
var meteo = saison.GenererMeteoDuJour();

var plante = new Soja();
plante.Update(meteo, tempsEcouleEnJours: 1f);

Console.WriteLine($"Météo: {meteo.Description}");
Console.WriteLine($"- Température: {meteo.Temperature}°C");
Console.WriteLine($"- Pluie: {meteo.QuantitePluie:P0}");
Console.WriteLine($"- Stress thermique: {plante.EstEnStressThermique}");