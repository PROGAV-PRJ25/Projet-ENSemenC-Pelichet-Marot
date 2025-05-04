public class Mais : Plante
{
    public Mais()
        : base(
            nomPlante:            "Maïs",
            acronyme:             "Ma",
            espacePris:           2,
            terrainIdeal:         new TerrainClassiqueTerreux(),
            saisonCompatible:     new List<Saison>{ new SaisonPluvieuse() },
            vitesseDeshydratation:3f,
            temperatureMinimale:  18f,
            temperatureMaximale:  32f,
            vitesseCroissance:    0.15f,
            hauteurMaximale:      1.5f
        )
    {
        HydratationCritique = 40f;
        LuminositeIdeale    = 80f;
    }
}
