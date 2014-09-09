using DK2D.Rooms;
using DK2D.Terrains;

using SFML.Graphics;
using SFML.Window;

namespace DK2D.Map
{
    class MapCell
    {
        public MapCell(Map map, int x, int y)
        {
            Map = map;
            X = x;
            Y = y;
            Position = new Vector2i(X, Y);
        }

        public Map Map { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Vector2i Position { get; private set; }

        public Terrain Terrain { get; set; }

        public Room Room { get; set; }

        public Color? OverlayColor { get; set; }

        public bool IsClaimed
        {
            get
            {
                return Terrain is ClaimedPath;
            }
        }
    }
}
