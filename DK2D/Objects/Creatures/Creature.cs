using System.Collections.Generic;
using System.Linq;

using DK2D.Map;
using DK2D.Pathfinding;

using SFML.Window;

namespace DK2D.Objects.Creatures
{
    internal abstract class Creature : GameObject
    {
        private readonly List<Vector2f> _path = new List<Vector2f>();

        protected Creature(Game game)
            : base(game)
        {
        }

        public List<Vector2f> Path
        {
            get
            {
                return _path;
            }
        }

        public void MoveTo(Vector2i cellIndex)
        {
            Vector2i currentCellIndex = Game.Map.MapCoordsToCellIndex(Position);
            MapCell currentCell = Game.Map.Get(currentCellIndex);

            var star = new AStar(i => Game.Map.Get(i).IsPassable, i => Game.Map.Get(i).Adjacents().Select(cell => cell.Position));
            List<Vector2i> path = star.FindPath(currentCell.Position, cellIndex);

            // We already stand on the first cell
            path.RemoveAt(0);

            Path.Clear();
            foreach (Vector2i cell in path)
            {
                Vector2f coords = Game.Map.MapCellIndexToCenterCoords(cell);
                Path.Add(coords);
            }
        }
    }
}
