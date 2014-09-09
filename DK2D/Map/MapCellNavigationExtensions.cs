using System.Collections.Generic;

namespace DK2D.Map
{
    static class MapCellNavigationExtensions
    {
        public static IEnumerable<MapCell> Adjacents(this MapCell cell)
        {
            var map = cell.Map;
            return new[] { map[cell.X, cell.Y - 1], map[cell.X + 1, cell.Y], map[cell.X, cell.Y + 1], map[cell.X - 1, cell.Y] };
        }
    }
}
