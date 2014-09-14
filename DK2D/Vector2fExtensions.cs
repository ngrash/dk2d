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

        public static float DistanceTo(this Vector2f baseVector, Vector2f vector)
        {
            float x1 = baseVector.X;
            float y1 = baseVector.Y;
            float x2 = vector.X;
            float y2 = vector.Y;

            return (float)Math.Sqrt(Math.Pow(Math.Abs(x1 - x2), 2) + Math.Pow(Math.Abs(y1 - y2), 2));
        }
    }

    static class Vector2iExtensions
    {
        public static float DistanceTo(this Vector2i baseVector, Vector2i vector)
        {
            int x1 = baseVector.X;
            int y1 = baseVector.Y;
            int x2 = vector.X;
            int y2 = vector.Y;

            return (float)Math.Sqrt(Math.Pow(Math.Abs(x1 - x2), 2) + Math.Pow(Math.Abs(y1 - y2), 2));
        }

        public static bool IsSameAs(this Vector2i baseVector, Vector2i vector)
        {
            return baseVector.X == vector.X 
                && baseVector.Y == vector.Y;
        }
    }
}
