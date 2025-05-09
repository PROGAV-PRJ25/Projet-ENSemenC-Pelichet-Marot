public class Mais : Plante
{
    public Mais(Graines graines)
        : base(
            prixGraines :        12,
            nomPlante:            "Maïs",
            acronyme:             "Ma",
            espacePris:           2,
            terrainIdeal:         new TerrainClassiqueTerreux(),
            saisonCompatible:     new List<Saison>{ new SaisonPluvieuse() },
            vitesseDeshydratation:3f,
            temperatureMinimale:  18f,
            temperatureMaximale:  32f,
            vitesseCroissance:    0.15f,
            hauteurMaximale:      1.5f,
            graines : graines
        )
    {
        HydratationCritique = 40f;
        LuminositeIdeale    = 4;
        RendementBase = 10;
    }
}
