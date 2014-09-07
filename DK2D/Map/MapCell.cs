using System.Collections.Generic;
using DK2D.Rooms;
using DK2D.Terrains;
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

        #region Navigation

        public MapCell North
        {
            get { return Map[X, Y - 1]; }
        }

        public MapCell NorthEast
        {
            get { return Map[X + 1, Y - 1]; }
        }

        public MapCell East
        {
            get { return Map[X + 1, Y]; }
        }

        public MapCell SouthEast
        {
            get { return Map[X + 1, Y + 1]; }
        }

        public MapCell South
        {
            get { return Map[X, Y + 1]; }
        }

        public MapCell SouthWest
        {
            get { return Map[X - 1, Y + 1]; }
        }

        public MapCell West
        {
            get { return Map[X - 1, Y]; }
        }

        public MapCell NorthWest
        {
            get { return Map[X - 1, Y - 1]; }
        }

        public IEnumerable<MapCell> Neighbors
        {
            get
            {
                if (North != null) yield return North;
                if (NorthEast != null) yield return NorthEast;
                if (East != null) yield return East;
                if (SouthEast != null) yield return SouthEast;
                if (South != null) yield return South;
                if (SouthWest != null) yield return SouthWest;
                if (West != null) yield return West;
                if (NorthWest != null) yield return NorthWest;
            }
        }

        #endregion

        public Terrain Terrain { get; set; }

        public Room Room { get; set; }

        public bool Choosen { get; set; }

        public bool Considered { get; set; }
    }
}
