﻿using System;
using System.Collections.Generic;
using System.Linq;

using DK2D.Actions;
using DK2D.Map;
using DK2D.Pathfinding;
using DK2D.Terrains;
using SFML.Graphics;
using SFML.Window;

namespace DK2D.Objects.Creatures
{
    internal class Imp : Creature
    {
        private const float Speed = 40;

        private readonly List<Vector2f> _path = new List<Vector2f>();

        private GameAction _currentAction;

        public Imp()
        {
            Sprite = new Sprite(Textures.Imp);
        }

        public List<Vector2f> Path
        {
            get
            {
                return _path;
            }
        }

        public override void Update(float secondsElapsed, Game game)
        {
            bool isAtDestination = _path.Count == 0;
            if (!isAtDestination)
            {
                Vector2f nextPoint = _path[0];
                bool reachedNextPoint = MoveTowardsPoint(nextPoint, secondsElapsed);
                if (reachedNextPoint)
                {
                    _path.RemoveAt(0);
                }
            }
            else if (_currentAction != null)
            {
                _currentAction.Perform(secondsElapsed, this, game);
                if (_currentAction.IsDone)
                {
                    _currentAction = null;
                }
            }
            else
            {
                FindSomethingToDo(game);
            }
        }

        private void FindSomethingToDo(Game game)
        {
            Vector2i cellIndex = game.Map.MapCoordsToCellIndex(Position);
            MapCell currentCell = game.Map.Get(cellIndex);

            IEnumerable<GameAction> possibleActions = ScanEnvironment(cellIndex, game.Map);

            GameAction nearestAction =
                possibleActions.OrderBy(action => action.Cell.DistanceTo(currentCell))
                               .ThenByDescending(action => action.Cell.Adjacents().Count(adjacent => adjacent.IsClaimed))
                               .FirstOrDefault();
            if (nearestAction != null)
            {
                _currentAction = nearestAction;

                nearestAction.Cell.Highlight(Colors.OverlayRed);

                var star = new AStar(i => !(game.Map.Get(i).Terrain is Earth), i => game.Map.Get(i).Adjacents().Select(cell => cell.Position));
                List<Vector2i> path = star.FindPath(currentCell.Position, _currentAction.Cell.Position);

                // We already stand on the first cell
                path.RemoveAt(0);

                Path.Clear();
                foreach (Vector2i cell in path)
                {
                    Vector2f coords = game.Map.MapCellIndexToCenterCoords(cell);
                    Path.Add(coords);
                }
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
            const int Cr = 5;

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
                            GameAction action = GetActionOrNull(mapCell);
                            if (action != null)
                            {
                                possibleActions.Add(action);
                            }
                        }
                    }
                }
            }

            return possibleActions;
        }

        private GameAction GetActionOrNull(MapCell cell)
        {
            bool hasClaimedNeighbor = cell.Adjacents().Any(adjacentCell => adjacentCell.IsClaimed);
            if (!hasClaimedNeighbor)
            {
                return null;
            }

            cell.Highlight(Colors.OverlayBlue);

            if (cell.Terrain is DirtPath)
            {
                cell.Highlight(Colors.OverlayGreen);

                Vector2f actionPosition = cell.Map.MapCellIndexToCenterCoords(new Vector2i(cell.X, cell.Y));
                var possibleAction = new ClaimPath
                {
                    Position = actionPosition,
                    Cell = cell
                };

                return possibleAction;
            }

            return null;
        }
    }
}
