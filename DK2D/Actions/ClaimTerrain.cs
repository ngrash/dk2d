using DK2D.Terrains;

namespace DK2D.Actions
{
    internal class ClaimPath : LaborGameAction
    {
        public ClaimPath()
        {
            Duration = 0.5f;
        }

        protected override void Completed(Objects.Creatures.Imp imp)
        {
            Cell.Terrain = new ClaimedPath();
        }
    }
}
