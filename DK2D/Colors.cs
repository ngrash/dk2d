using SFML.Graphics;

using DrawingColor = System.Drawing.Color;

namespace DK2D
{
    static class Colors
    {
        public static readonly Color OverlayRed = ColorFrom(DrawingColor.FromArgb(100, 255, 0, 0));
        public static readonly Color OverlayGreen = ColorFrom(DrawingColor.FromArgb(100, 0, 255, 0));
        public static readonly Color OverlayBlue = ColorFrom(DrawingColor.FromArgb(100, 0, 0, 255));

        public static readonly Color DebugPath = ColorFrom(DrawingColor.OrangeRed);

        public static readonly Color Earth = ColorFrom(DrawingColor.SaddleBrown);
        public static readonly Color DirtPath = ColorFrom(DrawingColor.RosyBrown);
        public static readonly Color ClaimedPath = ColorFrom(DrawingColor.LightGray);
        public static readonly Color Imp = ColorFrom(DrawingColor.Brown);

        private static Color ColorFrom(DrawingColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
