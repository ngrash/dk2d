using System.Collections.Generic;

using SFML.Window;

namespace DK2D.UI
{
    internal class Menu
    {
        public Menu()
        {
            Buttons = new List<Button>();
        }

        public Vector2f Position { get; set; }

        public Vector2f Size { get; set; }

        public List<Button> Buttons { get; private set; }
    }
}
