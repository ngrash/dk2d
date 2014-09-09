using System.Collections.Generic;
using DK2D.Rooms;
using DK2D.Terrains;
using SFML.Window;

namespace DK2D.Map
{
    class MapCell
    {
        private bool _considered;

        private bool _visible;

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

        public bool Choosen { get; set; }

        public bool Considered
        {
            get
            {
                return _considered;
            }

            set
            {
                _considered = value;
                Choosen = false;
            }
        }

        public bool IsClaimed
        {
            get
            {
                return Terrain is ClaimedPath;
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
                Considered = false;
            }
        }
    }
}
