using DK2D.Map;
using DK2D.Terrains;

namespace DK2D.Actions
{
    internal class ClaimPath : LaborGameAction
    {
        public ClaimPath()
        {
            Duration = 0.5f;
        }

        protected override void Completed(Objects.Creatures.Imp imp, Game game)
        {
            MapCell cell = game.Map.MapCoordsToCell(imp.Position);
            cell.Terrain = new ClaimedPath();
        }
    }
}
