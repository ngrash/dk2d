using System;
using System.Collections.Generic;
using DK2D.Actions;
using DK2D.Map;
using DK2D.Terrains;
using SFML.Graphics;
using SFML.Window;

namespace DK2D.Objects.Creatures
{
    class Imp : Creature
    {
        private readonly Type[] _priorities = new[]
            {
                typeof (MineGold),
                typeof (ClaimPath)
            };

        private GameAction _targetAction;
        private GameAction _currentAction;

        public Imp()
        {
            Sprite = new Sprite(Textures.Imp);
        }

        public override void Update(float secondsElapsed, Game game)
        {
            if (_currentAction == null && _targetAction == null)
            {
                Vector2i cellIndex = game.Map.MapCoordsToCellIndex(Position);

                MapCell cell = game.Map[cellIndex.X, cellIndex.Y];

                GameAction action = FindAction(cell, 5);

                if (action != null)
                {
                    _targetAction = action;
                }
                else
                {
                    _currentAction = new WalkAround();
                }
            }
            else if (_currentAction == null && _targetAction != null)
            {
                // Walk to target
            }
            else
            {
                if (_currentAction is WalkAround)
                {
                    
                }
                else if (_currentAction is ClaimedPath)
                {
                    
                }
            }
        }

        private GameAction FindAction(MapCell current, int iterations)
        {
            var closed = new Queue<MapCell>();
            return FindAction(current, closed, iterations);
        }

        private GameAction FindAction(MapCell current, Queue<MapCell> closed, int iterations)
        {
            GameAction action = GetActionOrNull(current);
            if (action != null || iterations <= 0)
            {
                return action;
            }

            closed.Enqueue(current);

            if (!(current.Terrain is Earth))
            {
                foreach (MapCell neighbor in current.Neighbors)
                {
                    if (!closed.Contains(neighbor))
                    {
                        GameAction neighborAction = FindAction(neighbor, closed, iterations - 1);
                        if (neighborAction != null)
                        {
                            return neighborAction;
                        }
                    }
                }
            }

            return null;
        }

        private GameAction GetActionOrNull(MapCell cell)
        {
            cell.Considered = true;

            if (cell.Terrain is DirtPath)
            {
                cell.Choosen = true;

                Vector2f actionPosition = cell.Map.MapCellIndexToCenterCoords(new Vector2i(cell.X, cell.Y));
                return new ClaimPath
                {
                    Position = actionPosition,
                    Cell = cell
                };
            }

            return null;
        }
    }
}
