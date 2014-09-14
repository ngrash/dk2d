using System;
using System.Collections.Generic;
using System.IO;

using SFML.Graphics;
using SFML.Window;

namespace DK2D
{
    internal static class Textures
    {
        private static readonly List<RenderTexture> RenderTextures = new List<RenderTexture>();
        private static readonly List<Texture> AssetTextures = new List<Texture>();

        public static readonly Texture Imp = TextureFrom(new CircleShape {FillColor = Colors.CreatureImp, Radius = 5}, Size.Widget);

        public static readonly Texture TerrainWater = TextureFrom("Terrain/Water");

        public static readonly Texture TerrainDirtPath = TextureFrom("Terrain/DirtPath");

        public static readonly Texture TerrainEarth = TextureFrom("Terrain/Earth");

        public static readonly Texture TerrainClaimedPath = TextureFrom("Terrain/ClaimedPath");

        private enum Size
        {
            Room,
            Widget
        }

        public static void Release()
        {
            foreach (Texture texture in AssetTextures)
            {
                texture.Dispose();
            }

            AssetTextures.Clear();

            foreach (RenderTexture renderTexture in RenderTextures)
            {
                renderTexture.Dispose();
            }

            RenderTextures.Clear();
        }

        private static Texture TextureFrom(string asset)
        {
            string filePath = Path.Combine("Assets", asset) + ".png";
            var texture = new Texture(filePath);
            AssetTextures.Add(texture);
            return texture;
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
