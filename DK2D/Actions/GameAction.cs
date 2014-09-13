using DK2D.Map;
using DK2D.Objects.Creatures;

using SFML.Window;

namespace DK2D.Actions
{
    class GameAction
    {
        public MapCell Cell { get; set; }

        public bool IsDone { get; set; }

        public virtual void Perform(float secondsElapsed, Imp imp, Game game)
        {
            IsDone = true;
        }
    }
}
