public class Cafe : Plante
{
    public Cafe()
        : base(
            nomPlante: "Café",
            acronyme: "Cf",
            espacePris: 2,
            terrainIdeal: new TerrainArgileux(),
            saisonCompatible: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 3.5f,
            temperatureMinimale: 15f,
            temperatureMaximale: 25f,
            vitesseCroissance: 0.08f,
            hauteurMaximale: 0.8f
        )
    {
        HydratationCritique = 65f;
        LuminositeIdeale = 70f;
    }

    protected override void Pousser(float tauxSatisfaction)
    {
        // Café préfère une croissance douce et régulière
        base.Pousser(tauxSatisfaction * 0.8f);
    }
}