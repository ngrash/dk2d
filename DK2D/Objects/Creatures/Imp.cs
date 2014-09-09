using System.Collections.Generic;
using System.Linq;

using DK2D.Actions;
using DK2D.Map;
using DK2D.Terrains;
using SFML.Graphics;
using SFML.Window;

namespace DK2D.Objects.Creatures
{
    class Imp : Creature
    {
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
                MapCell currentCell = game.Map.Get(cellIndex);

                IEnumerable<GameAction> possibleActions = ScanEnvironment(cellIndex, game.Map);

                GameAction nearestAction = possibleActions.OrderBy(action => action.Cell.DistanceTo(currentCell)).FirstOrDefault();
                if (nearestAction != null)
                {
                    nearestAction.Cell.Choosen = true;
                }
            }
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

            cell.Visible = true;

            if (cell.Terrain is DirtPath)
            {
                cell.Considered = true;

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
