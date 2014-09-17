using System;
using System.Collections.Generic;
using System.Linq;

using DK2D.Actions;
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

        public float Speed { get; set; }

        public int Gold { get; set; }

        public GameAction CurrentAction { get; private set; }

        public override void Update(float secondsElapsed)
        {
            bool isAtDestination = Path.Count == 0;
            if (isAtDestination)
            {
                if (CurrentAction != null)
                {
                    CurrentAction.Perform(secondsElapsed);
                    if (CurrentAction.IsDone)
                    {
                        CurrentAction.Cell = null;
                        CurrentAction.Creature = null;
                        CurrentAction = null;
                    }
                }
                else
                {
                    Idle();
                }
            }
            else
            {
                Vector2f nextPoint = Path[0];
                bool reachedNextPoint = MoveTowardsPoint(nextPoint, secondsElapsed);
                if (reachedNextPoint)
                {
                    Path.RemoveAt(0);
                }
            }

            base.Update(secondsElapsed);
        }

        public void MoveTo(GameAction action)
        {
            if (CurrentAction != null)
            {
                CurrentAction.Cell = null;
                CurrentAction.Creature = null;
                CurrentAction = null;
            }

            MoveTo(action.Cell);
            CurrentAction = action;
        }

        public void MoveTo(MapCell target)
        {
            MapCell current = Cell;

            var star = new AStar(i => Game.Map.Get(i).IsPassable, i => Game.Map.Get(i).Adjacents().Select(cell => cell.Position));
            List<Vector2i> path = star.FindPath(current.Position, target.Position);

            // We already stand on the first cell
            path.RemoveAt(0);

            Path.Clear();
            foreach (Vector2i cell in path)
            {
                Vector2f coords = Game.Map.MapCellIndexToCenterCoords(cell);
                Path.Add(coords);
            }
        }

        protected virtual void Idle()
        {
        }

        private bool MoveTowardsPoint(Vector2f goal, float elapsed)
        {
            const float Epsilon = 0.1f;

            if (Math.Abs(goal.X - Position.X) < Epsilon && Math.Abs(goal.Y - Position.Y) < Epsilon)
            {
                return true;
            }

            // http://gamedev.stackexchange.com/a/28337
            Vector2f direction = (goal - Position).Normalize();
            Position += direction * (Speed * elapsed);

            if (Math.Abs(direction.Dot((goal - Position).Normalize()) + 1) < Epsilon)
            {
                Position = goal;
            }

            return Math.Abs(Position.X - goal.X) < Epsilon && Math.Abs(Position.Y - goal.Y) < Epsilon;
        }
    }
}
