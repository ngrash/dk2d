using SFML.Graphics;

using DrawingColor = System.Drawing.Color;

namespace DK2D
{
    static class Colors
    {
        public static readonly Color Earth = ColorFrom(DrawingColor.SaddleBrown);

        public static readonly Color DirtPath = ColorFrom(DrawingColor.RosyBrown);

        public static readonly Color ClaimedPath = ColorFrom(DrawingColor.LightGray);

        private static Color ColorFrom(DrawingColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
