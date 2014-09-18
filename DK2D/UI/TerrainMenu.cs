using DK2D.Terrains;

namespace DK2D.UI
{
    internal class TerrainMenu : Menu
    {
        public TerrainMenu()
        {
            Add(new Earth());
            Add(new DirtPath());
            Add(new ClaimedPath());
            Add(new GoldSeam());
            Add(new Water());
        }

        private void Add(Terrain terrain)
        {
            Add(new TerrainButton(terrain));
        }
    }
}
