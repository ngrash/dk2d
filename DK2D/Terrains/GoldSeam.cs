namespace DK2D.Terrains
{
    internal class GoldSeam : Terrain
    {
        public GoldSeam()
        {
            Color = Colors.TerrainGold;
            Quantity = 250;
        }

        public int Quantity { get; set; }
    }
}