public class Cafe : Plante
{
    public Cafe(Graines graines)
        : base(
            prixGraines :        16,
            nomPlante: "Café",
            acronyme: "Cf",
            espacePris: 2,
            terrainIdeal: new TerrainArgileux(),
            saisonCompatible: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 3.5f,
            temperatureMinimale: 15f,
            temperatureMaximale: 25f,
            vitesseCroissance: 0.08f,
            hauteurMaximale: 0.8f,
            graines : graines
        )
    {
        HydratationCritique = 65f;
        LuminositeIdeale = 2;
        RendementBase = 20;
    }

    protected override void Pousser(float tauxSatisfaction)
    {
        // Café préfère une croissance douce et régulière
        base.Pousser(tauxSatisfaction * 0.8f);
    }
}