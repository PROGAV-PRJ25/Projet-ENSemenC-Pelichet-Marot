public class Cactus : Plante
{
    public Cactus()
        : base(
            nomPlante: "Cactus",
            espace: 3,
            terrain: new TerrainSableux(),
            saison: new List<Saison> { new SaisonSeche() },
            vitesseDeshydrataion: 5f
        )
    { }
    public override void VerifierSante()
    {
        throw new NotImplementedException();
    }
    public override void Pousser()
    {
        throw new NotImplementedException();
    }
}