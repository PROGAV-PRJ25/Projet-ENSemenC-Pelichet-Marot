public class CanneASucre : Plante
{
    public CanneASucre(Graines graines)
        : base(
            prixGraines :        14,
            nomPlante: "Canne à sucre",
            acronyme: "Cs",
            espacePris: 3,
            terrainIdeal: new TerrainArgileux(),
            saisonCompatible: new List<Saison> { new SaisonPluvieuse() },
            vitesseDeshydratation: 4f,
            temperatureMinimale: 20f,
            temperatureMaximale: 38f,
            vitesseCroissance: 0.10f,
            hauteurMaximale: 2.0f,
            graines : graines
        )
    {
        HydratationCritique = 60f;
        LuminositeIdeale = 4;
        RendementBase = 18;
    }

    protected override void Pousser(float tauxSatisfaction)
    {
        // Bonus de croissance tant que la plante est loin de sa taille max
        float facteur = 1f + 0.3f * (1 - HauteurActuelle / HauteurMaximale);
        base.Pousser(tauxSatisfaction * facteur);
    }
}