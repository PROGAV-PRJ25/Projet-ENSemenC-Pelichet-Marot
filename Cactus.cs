public class Cactus : Plante
{
    public Cactus(Graines graines)
        : base(
            prixGraines: 20,
            nomPlante: "Cactus",
            acronyme: "Ca",
            espacePris: 1,
            terrainIdeal: new TerrainSableux(),
            saisonCompatible: new List<Saison> { new SaisonSeche() },
            vitesseDeshydratation: 1f,
            temperatureMinimale: 18f,
            temperatureMaximale: 40f,
            vitesseCroissance: 0.02f,
            hauteurMaximale: 0.5f,
            graines: graines,
            esperanceDeVieSemaines: 500
        )
    {
        HydratationCritique = 10f;
        LuminositeIdeale = 5;
        RendementBase = 40;
        EstVivace = true;
    }

    protected override void Pousser(float tauxSatisfaction)
    {
        // Très lente au début, accélère une fois un certain seuil atteint
        float facteur = (HauteurActuelle < 0.2f * HauteurMaximale)
            ? 0.5f
            : 1.5f;
        base.Pousser(tauxSatisfaction * facteur);
    }
}