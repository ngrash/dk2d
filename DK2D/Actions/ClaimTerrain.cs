using DK2D.Map;
using DK2D.Objects.Creatures;
using DK2D.Terrains;

namespace DK2D.Actions
{
    internal class ClaimPath : GameAction
    {
        private const float Duration = 0.5f;

        public float PercentCompleted { get; private set; }

        public override void Perform(float secondsElapsed, Imp imp, Game game)
        {
            PercentCompleted += (100 / Duration) * secondsElapsed;

            if (PercentCompleted >= 100)
            {
                IsDone = true;

                MapCell cell = game.Map.MapCoordsToCell(imp.Position);
                cell.Terrain = new ClaimedPath();
            }
        }
    }
}
