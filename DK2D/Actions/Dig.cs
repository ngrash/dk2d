using DK2D.Map;
using DK2D.Terrains;

namespace DK2D.Actions
{
    internal class Dig : LaborGameAction
    {
        public Dig()
        {
            Duration = 0.5f;
        }

        public MapCell Target { get; set; }

        protected override void Completed()
        {
            Target.Terrain = new DirtPath();
            Target.IsSelected = false;

            Creature.MoveTo(Target);
        }
    }
}
