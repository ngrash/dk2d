using DK2D.Map;
using DK2D.Objects.Creatures;

namespace DK2D.Actions
{
    internal class GameAction
    {
        public MapCell Cell { get; set; }

        public Creature Creature { get; set; }

        public bool IsDone { get; set; }

        public virtual void Perform(float secondsElapsed)
        {
            IsDone = true;
        }
    }
}
