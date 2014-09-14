using System;
using System.Collections.Generic;
using System.Linq;

using DK2D.Actions;
using DK2D.Map;
using DK2D.Terrains;
using SFML.Graphics;
using SFML.Window;

namespace DK2D.Objects.Creatures
{
    internal class Imp : Creature
    {
        public const int ScanRadius = 5;

        private const float Speed = 80;

        private GameAction _currentAction;

        public Imp(Game game)
            : base(game)
        {
            Sprite = new Sprite(Textures.Imp);
        }

        public override void Update(float secondsElapsed)
        {
            bool isAtDestination = Path.Count == 0;
            if (!isAtDestination)
            {
                Vector2f nextPoint = Path[0];
                bool reachedNextPoint = MoveTowardsPoint(nextPoint, secondsElapsed);
                if (reachedNextPoint)
                {
                    Path.RemoveAt(0);
                }
            }
            else if (_currentAction != null)
            {
                _currentAction.Perform(secondsElapsed);
                if (_currentAction.IsDone)
                {
                    _currentAction = null;
                }
            }
            else
            {
                FindSomethingToDo();
            }
        }

        private void FindSomethingToDo()
        {
            Vector2i cellIndex = Game.Map.MapCoordsToCellIndex(Position);
            MapCell currentCell = Game.Map.Get(cellIndex);

            IEnumerable<GameAction> possibleActions = ScanEnvironment(cellIndex, Game.Map);

            GameAction nearestAction =
                possibleActions.OrderBy(action => action.Cell.DistanceTo(currentCell))
                               .ThenByDescending(action => action.Cell.Neighbors().Count(adjacent => adjacent.IsClaimed))
                               .FirstOrDefault();
            if (nearestAction != null)
            {
                _currentAction = nearestAction;

                nearestAction.Cell.Highlight(Colors.OverlayRed);

                MoveTo(nearestAction.Cell.Position);
            }
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

        private IEnumerable<GameAction> ScanEnvironment(Vector2i cellIndex, Map.Map map)
        {
            var possibleActions = new List<GameAction>();

            int cx = cellIndex.X;
            int cz = cellIndex.Y;
            const int Cr = ScanRadius;

            for (int z = cz - Cr; z <= cz + Cr; z++)
            {
                for (int x = cx - Cr; x <= cx + Cr; x++)
                {
                    bool isInCircle = ((x - cx) * (x - cx)) + ((z - cz) * (z - cz)) < Cr * Cr;
                    if (isInCircle)
                    {
                        MapCell mapCell = map[x, z];
                        if (mapCell != null)
                        {
                            IEnumerable<GameAction> possibleActionsOnCell = GetActions(mapCell);
                            possibleActions.AddRange(possibleActionsOnCell);
                        }
                    }
                }
            }

            return possibleActions;
        }

        private IEnumerable<GameAction> GetActions(MapCell cell)
        {
            if (cell.IsClaimed)
            {
                foreach (MapCell selectedAdjacent in cell.Adjacents().Where(adjacent => adjacent.IsSelected && adjacent.IsPenetrable))
                {
                    selectedAdjacent.Highlight(Colors.OverlayGreen);

                    yield return new Dig { Cell = cell, Target = selectedAdjacent, Creature = this };
                }
            }
            else
            {
                bool hasClaimedAdjacents = cell.Adjacents().Any(adjacent => adjacent.IsClaimed);
                if (hasClaimedAdjacents)
                {
                    if (cell.Terrain is DirtPath)
                    {
                        cell.Highlight(Colors.OverlayGreen);

                        var possibleAction = new ClaimPath
                        {
                            Creature = this,
                            Cell = cell
                        };

                        yield return possibleAction;
                    }
                }
            }
        }
    }
}
