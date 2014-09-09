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
                    this[x, y] = new MapCell(this, x, y);
                }
            }
        }

        public MapCell Get(Vector2i index)
        {
            return this[index.X, index.Y];
        }

        public MapCell MapCoordsToCell(Vector2f coords)
        {
            Vector2i index = MapCoordsToCellIndex(coords);

            return this[index.X, index.Y];
        }

        public Vector2i MapCoordsToCellIndex(Vector2f coords)
        {
            var x = (int)coords.X / _cellWidth;
            var y = (int)coords.Y / _cellHeight;

            return new Vector2i(x, y);
        }

        public Vector2f MapCellIndexToCoords(Vector2i cellIndex)
        {
            var x = cellIndex.X*_cellWidth;
            var y = cellIndex.Y*_cellHeight;

            return new Vector2f(x, y);
        }

        public Vector2f MapCellIndexToCenterCoords(Vector2i cellIndex)
        {
            Vector2f coords = MapCellIndexToCoords(cellIndex);
            return new Vector2f(coords.X + (_cellWidth / 2f), coords.Y + (_cellHeight / 2f));
        }
    }
}
