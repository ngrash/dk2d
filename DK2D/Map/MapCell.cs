using DK2D.Rooms;
using DK2D.Terrains;

using SFML.Graphics;
using SFML.Window;

namespace DK2D.Map
{
    internal class MapCell
    {
        public MapCell(Map map, int x, int y)
        {
            Map = map;
            X = x;
            Y = y;
            Position = new Vector2i(X, Y);
        }

        public Map Map { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Vector2i Position { get; private set; }

        public Terrain Terrain { get; set; }

        public Room Room { get; set; }

        public Color? OverlayColor { get; set; }

        public bool IsOverlayFadeEnabled { get; set; }

        public float OverlayFade { get; set; }

        public float OverlayFadeDuration { get; set; }

        public bool IsClaimed
        {
            get
            {
                return Terrain is ClaimedPath;
            }
        }

        public void Highlight(Color color)
        {
            OverlayColor = color;
            OverlayFade = 0;
            OverlayFadeDuration = 1;
            IsOverlayFadeEnabled = true;
        }

        public void Update(float secondsElapsed)
        {
            if (IsOverlayFadeEnabled)
            {
                OverlayFade += (1f / OverlayFadeDuration) * secondsElapsed;
                if (OverlayFade >= 1)
                {
                    IsOverlayFadeEnabled = false;
                    OverlayColor = null;
                }
            }
        }
    }
}
