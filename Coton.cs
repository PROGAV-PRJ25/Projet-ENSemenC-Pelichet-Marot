public class Coton : Plante
    {
        public Coton()
            : base(
                nomPlante:            "Coton",
                acronyme:             "Co",
                espacePris:           2,
                terrainIdeal:         new TerrainClassiqueTerreux(),
                saisonCompatible:     new List<Saison>{ new SaisonPluvieuse() },
                vitesseDeshydratation:2.5f,
                temperatureMinimale:  18f,
                temperatureMaximale:  30f,
                vitesseCroissance:    0.12f,
                hauteurMaximale:      1.0f
            )
        {
            HydratationCritique = 35f;
            LuminositeIdeale    = 75f;
        }

        protected override void Pousser(float tauxSatisfaction)
        {
            // Croissance standard, plus un petit boost si conditions parfaites
            float bonus = tauxSatisfaction >= 1f ? 1.2f : 1f;
            base.Pousser(tauxSatisfaction * bonus);
        }
    }