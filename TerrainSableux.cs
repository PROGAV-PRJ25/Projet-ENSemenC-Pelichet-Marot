public class TerrainSableux : Terrain
{
    public TerrainSableux()
        : base("Sableux", fertilite: 0.4f, coeffAbsorptionEau: 0.3f, couleur:  ConsoleColor.Yellow) { } //L'eau est drainée rapidement dans le sable
}