public class Compost : Plante
{
    private int pourcentageRemplissage;

    public int Remplissage => pourcentageRemplissage;

    public Compost(Graines graines)
        : base(
            graines,
            prixGraines: 30,
            nomPlante: "Compost",
            acronyme: "#",
            espacePris: 1,
            terrainIdeal: new TerrainArgileux(), // peu importe ici
            saisonCompatible: new List<Saison>(),
            vitesseDeshydratation: 0f,
            temperatureMinimale: 0f,
            temperatureMaximale: 100f,
            vitesseCroissance: 0f,
            hauteurMaximale: 100f,
            esperanceDeVieSemaines: 9999
        )
    {
        pourcentageRemplissage = 0;
    }

    public void AjouterRemplissage()
    {
        pourcentageRemplissage = Math.Min(100, pourcentageRemplissage + 25);
        if (pourcentageRemplissage == 100)
        {
            _graines.Ajouter(15);
            Console.WriteLine("\n♻️ Votre compost est plein ! +15 graines récupérées.");
            Thread.Sleep(2000);
            pourcentageRemplissage = 0;
        }
    }

    public override void Update(
        float tempsEcouleEnSemaines,
        float temperatureSemaine,
        bool espaceRespecte,
        float coeffAbsorptionEau,
        int luminositeSemaine,
        Saison saisonActuelle,
        Terrain terrainActuel)
    {
        // Ne change pas, pas besoin de croissance ni de mort
    }

    public override float EvaluerConditions(bool espaceRespecte, Saison saisonActuelle, Terrain terrainActuel)
    {
        return 0f; // Le compost est toujours content
    }

    public override void Tuer()
    {
        // Ne meurt jamais
    }
    public void SetRemplissage(int valeur)
    {
        pourcentageRemplissage = Math.Clamp(valeur, 0, 100);
    }

}