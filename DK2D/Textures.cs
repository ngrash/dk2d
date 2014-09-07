using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Window;

namespace DK2D
{
    static class Textures
    {
        enum Size
        {
            Room,
            Widget
        }

        private static readonly List<RenderTexture> RenderTextures = new List<RenderTexture>();

        public static readonly Texture Imp = TextureFrom(new CircleShape {FillColor = Colors.Imp, Radius = 5}, Size.Widget);

        public static void Release()
        {
            foreach (RenderTexture renderTexture in RenderTextures)
            {
                renderTexture.Dispose();
            }

            RenderTextures.Clear();
        }

        private static Texture TextureFrom(Shape shape, Size size)
        {
            Vector2u textureSize = GetSizeVector(size);

            var renderTexture = new RenderTexture(textureSize.X, textureSize.Y);

            renderTexture.Draw(shape);

            RenderTextures.Add(renderTexture);
            return renderTexture.Texture;
        }

        private static Vector2u GetSizeVector(Size size)
        {
            switch (size)
            {
                case Size.Room:
                    return new Vector2u(Game.CellWidth, Game.CellHeight);
                case Size.Widget:
                    return new Vector2u(Game.CellHeight / 3, Game.CellWidth / 3);
                default:
                    throw new ArgumentOutOfRangeException("size");
            }
        }
    }
}
