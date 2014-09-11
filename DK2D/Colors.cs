using SFML.Graphics;

using DrawingColor = System.Drawing.Color;

namespace DK2D
{
    static class Colors
    {
        public static readonly Color OverlayRed = ColorFrom(DrawingColor.FromArgb(100, 255, 0, 0));
        public static readonly Color OverlayGreen = ColorFrom(DrawingColor.FromArgb(100, 0, 255, 0));
        public static readonly Color OverlayBlue = ColorFrom(DrawingColor.FromArgb(100, 0, 0, 255));

        public static readonly Color MenuBackground = ColorFrom(DrawingColor.DimGray);
        public static readonly Color MenuOutline = ColorFrom(DrawingColor.DarkSlateGray);

        public static readonly Color DebugPath = ColorFrom(DrawingColor.OrangeRed);
        public static readonly Color DebugScanRadius = ColorFrom(DrawingColor.Firebrick);

        public static readonly Color TerrainEarth = ColorFrom(DrawingColor.SaddleBrown);
        public static readonly Color TerrainDirtPath = ColorFrom(DrawingColor.RosyBrown);
        public static readonly Color TerrainClaimedPath = ColorFrom(DrawingColor.LightGray);

        public static readonly Color CreatureImp = ColorFrom(DrawingColor.Brown);

        private static Color ColorFrom(DrawingColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
