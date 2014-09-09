using System;

using SFML.Window;

namespace DK2D
{
    // https://code.google.com/p/monoxna/source/browse/trunk/src/Microsoft.Xna.Framework/Vector2.cs?r=348
    static class Vector2fExtensions
    {
        public static Vector2f Normalize(this Vector2f vector)
        {
            float factor = 1f / (float)Math.Sqrt((double)(vector.X * vector.X + vector.Y * vector.Y));
            return new Vector2f(vector.X * factor, vector.Y * factor);
        }

        public static float Dot(this Vector2f baseVector, Vector2f vector)
        {
            return baseVector.X * vector.X + baseVector.Y * vector.Y;
        }
    }
}
