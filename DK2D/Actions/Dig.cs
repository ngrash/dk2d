using DK2D.Map;
using DK2D.Objects.Creatures;
using DK2D.Terrains;

namespace DK2D.Actions
{
    class Dig : LaborGameAction
    {
        public Dig()
        {
            Duration = 0.5f;
        }

        public MapCell Target { get; set; }

        protected override void Completed(Imp imp, Game game)
        {
            Target.Terrain = new DirtPath();
            Target.IsSelected = false;

            imp.MoveTo(Target.Position, game);
        }
    }
}
