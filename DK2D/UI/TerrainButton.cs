using System;

using DK2D.Terrains;

namespace DK2D.UI
{
    internal class TerrainButton : Button
    {
        private readonly Terrain _terrainPrototype;

        public TerrainButton(Terrain terrainPrototype)
        {
            _terrainPrototype = terrainPrototype;
            Color = terrainPrototype.Color;
        }

        public override void CellClicked(Map.MapCell cell)
        {
            cell.Terrain = (Terrain)Activator.CreateInstance(_terrainPrototype.GetType());
        }
    }
}
