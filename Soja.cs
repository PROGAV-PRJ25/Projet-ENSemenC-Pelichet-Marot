public class Soja : Plante
{
    public Soja(Graines graines)
        : base(
            prixGraines :        10,
            nomPlante:           "Soja",
            acronyme:            "So",
            espacePris:          2,
            terrainIdeal:        new TerrainClassiqueTerreux(),
            saisonCompatible:    new List<Saison>{ new SaisonPluvieuse() },
            vitesseDeshydratation: 2f,
            temperatureMinimale: 10f,
            temperatureMaximale: 35f,
            vitesseCroissance:   0.1f,     // ex. 0.1 unité / jour
            hauteurMaximale:     1.0f,
            graines : graines
        )
    { 
        RendementBase = 12;
    }
    
    // Optionnel : on peut affiner le pattern de croissance pour le soja
    protected override void Pousser(float tauxSatisfaction)
    {
        // Par exemple, le soja pousse plus vite en début de cycle
        float facteur = 1f + 0.5f * (1 - HauteurActuelle / HauteurMaximale);
        base.Pousser(tauxSatisfaction * facteur);
    }
}
