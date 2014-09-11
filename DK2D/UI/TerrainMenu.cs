using System;

using DK2D.Terrains;

using SFML.Window;

namespace DK2D.UI
{
    internal class TerrainMenu : Menu
    {
        public TerrainMenu()
        {
            Add(new Earth());
            Add(new DirtPath());
            Add(new ClaimedPath());
        }

        public Button Click(Vector2f screenCoords)
        {
            throw new NotImplementedException();
        }

        private void Add(Terrain terrain)
        {
            float x = 9;
            float y = 9;

            if (Buttons.Count > 0)
            {
                Button last = Buttons[Buttons.Count - 1];
                x = last.Position.X + last.Size.X + 9f;
                y = last.Position.Y;
            }

            var button = new TerrainButton(terrain)
                {
                    Position = new Vector2f(x, y),
                    Size = new Vector2f(32, 32)
                };

            Buttons.Add(button);

            // Resize menu
            Size = new Vector2f(button.Position.X + button.Size.X + 9 , button.Size.Y + 9 + 9);
        }
    }
}
