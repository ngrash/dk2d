using System;
using System.Collections.Generic;

namespace DK2D.Map
{
    static class MapCellNavigationExtensions
    {
        public static IEnumerable<MapCell> Neighbors(this MapCell cell)
        {
            foreach (MapCell adjacent in Adjacents(cell))
            {
                yield return adjacent;
            }

            var map = cell.Map;

            MapCell northEast = map[cell.X + 1, cell.Y - 1];
            if (northEast != null)
            {
                yield return northEast;
            }

            MapCell southEast = map[cell.X + 1, cell.Y + 1];
            if (southEast != null)
            {
                yield return southEast;
            }

            MapCell southWest = map[cell.X - 1, cell.Y + 1];
            if (southWest != null)
            {
                yield return southWest;
            }

            MapCell northWest = map[cell.X - 1, cell.Y - 1];
            if (northWest != null)
            {
                yield return northWest;
            }
        }

        public static IEnumerable<MapCell> Adjacents(this MapCell cell)
        {
            var map = cell.Map;

            MapCell north = map[cell.X, cell.Y - 1];
            if (north != null)
            {
                yield return north;
            }

            MapCell east = map[cell.X + 1, cell.Y];
            if (east != null)
            {
                yield return east;
            }

            MapCell south = map[cell.X, cell.Y + 1];
            if (south != null)
            {
                yield return south;
            }

            MapCell west = map[cell.X - 1, cell.Y];
            if (west != null)
            {
                yield return west;
            }
        }

        public static double DistanceTo(this MapCell baseCell, MapCell cell)
        {
            int x1 = baseCell.X;
            int y1 = baseCell.Y;
            int x2 = cell.X;
            int y2 = cell.Y;

            return Math.Sqrt(Math.Pow(Math.Abs(x1 - x2), 2) + Math.Pow(Math.Abs(y1 - y2), 2));
        }
    }
}
