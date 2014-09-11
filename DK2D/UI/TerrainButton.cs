using DK2D.Terrains;

namespace DK2D.UI
{
    internal class TerrainButton : Button
    {
        public TerrainButton(Terrain terrainPrototype)
        {
            Color = terrainPrototype.Color;
        }
    }
}
