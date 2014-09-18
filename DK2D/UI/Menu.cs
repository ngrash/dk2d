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

        protected void Add(Button button)
        {
            float x = 9;
            float y = 9;

            if (Buttons.Count > 0)
            {
                Button last = Buttons[Buttons.Count - 1];
                x = last.Position.X + last.Size.X + 9f;
                y = last.Position.Y;
            }

            button.Position = new Vector2f(x, y);
            button.Size = new Vector2f(32, 32);

            Buttons.Add(button);

            // Resize menu
            Size = new Vector2f(button.Position.X + button.Size.X + 9, button.Size.Y + 9 + 9);
        }
    }
}
