using DK2D.Map;

using SFML.Graphics;
using SFML.Window;

namespace DK2D.UI
{
    internal class Button
    {
        public Color Color { get; set; }

        public Vector2f Position { get; set; }

        public Vector2f Size { get; set; }

        public bool IsActive { get; set; }

        public virtual void CellClicked(MapCell cell)
        {
        }
    }
}
