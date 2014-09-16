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

        public Vector2f Size { get; protected set; }

        public List<Button> Buttons { get; private set; }

        public Button HitTest(Vector2f screenCoords)
        {
            if (screenCoords.X > Position.X 
             && screenCoords.X < Position.X + Size.X
             && screenCoords.Y > Position.Y
             && screenCoords.Y < Position.Y + Size.Y)
            {
                Vector2f menuCoords = screenCoords - Position;

                foreach (Button button in Buttons)
                {
                    if (menuCoords.X > button.Position.X && menuCoords.X < button.Position.X + button.Size.X
                        && menuCoords.Y > button.Position.Y && menuCoords.Y < button.Position.Y + button.Size.Y)
                    {
                        return button;
                    }
                }
            }

            return null;
        }
    }
}
