using SFML.Graphics;
using SFML.Window;

namespace DK2D.Objects
{
    class GameObject
    {
        public Vector2f Position { get; set; }

        public Sprite Sprite { get; set; }

        public virtual void Update(float secondsElapsed, Game game)
        {

        }
    }
}
