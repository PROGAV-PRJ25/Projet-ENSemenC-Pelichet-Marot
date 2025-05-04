public class CanneASucre : Plante
{
    public CanneASucre()
        : base(
            nomPlante: "Canne à sucre",
            acronyme: "Cs",
            espacePris: 3,
            terrainIdeal: new TerrainArgileux(),
            saisonCompatible: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 4f,
            temperatureMinimale: 20f,
            temperatureMaximale: 38f,
            vitesseCroissance: 0.10f,
            hauteurMaximale: 2.0f
        )
    {
        HydratationCritique = 60f;
        LuminositeIdeale = 85f;
    }

    protected override void Pousser(float tauxSatisfaction)
    {
        // Bonus de croissance tant que la plante est loin de sa taille max
        float facteur = 1f + 0.3f * (1 - HauteurActuelle / HauteurMaximale);
        base.Pousser(tauxSatisfaction * facteur);
    }
}