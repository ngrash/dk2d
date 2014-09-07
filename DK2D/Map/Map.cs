using SFML.Window;

namespace DK2D.Map
{
    class Map : Grid<MapCell>
    {
        private readonly int _cellWidth;
        private readonly int _cellHeight;

        public Map(int cellWidth, int cellHeight, int mapWidth, int mapHeight)
            : base(mapWidth, mapHeight)
        {
            _cellWidth = cellWidth;
            _cellHeight = cellHeight;

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    this[x, y] = new MapCell();
                }
            }
        }

        public MapCell MapCoordsToCell(Vector2f coords)
        {
            var x = (int)coords.X / _cellWidth;
            var y = (int)coords.Y / _cellHeight;

            return this[x, y];
        }
    }
}
