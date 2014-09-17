using System.Collections.Generic;
using System.Linq;

using DK2D.Actions;
using DK2D.Map;
using DK2D.Terrains;

using SFML.Window;

namespace DK2D.Objects.Creatures
{
    internal class Imp : Creature
    {
        public const int ScanRadius = 5;

        public Imp(Game game)
            : base(game)
        {
            Color = Colors.CreatureImp;
            BoundingRadius = 5;
            Speed = 80;
        }

        protected override void Idle()
        {
            FindSomethingToDo();
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
                nearestAction.Creature = this;
                nearestAction.Cell.Highlight(Colors.OverlayRed);

                MoveTo(nearestAction);
            }
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

                    if (selectedAdjacent.Terrain is GoldSeam)
                    {
                        yield return new MineGold { Cell = cell, Target = selectedAdjacent };
                    }
                    else
                    {
                        yield return new Dig { Cell = cell, Target = selectedAdjacent };
                    }
                }
            }
            else
            {
                bool hasClaimedAdjacents = cell.Adjacents().Any(adjacent => adjacent.IsClaimed);
                if (hasClaimedAdjacents)
                {
                    if (cell.Terrain is DirtPath)
                    {
                        ClaimPath existingJob = cell.Actions.OfType<ClaimPath>().SingleOrDefault();
                        if (existingJob == null)
                        {
                            cell.Highlight(Colors.OverlayGreen);

                            var possibleAction = new ClaimPath { Cell = cell };

                            yield return possibleAction;
                        }
                        else
                        {
                            if (!existingJob.IsDone && existingJob.Creature == null)
                            {
                                yield return existingJob;
                            }
                        }
                    }
                }
            }
        }
    }
}
