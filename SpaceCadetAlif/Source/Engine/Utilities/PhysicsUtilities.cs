using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceCadetAlif.Source.Engine.Utilities
{
    static class PhysicsUtilities
    {
        /// <returns>Corners as set of Vector2 in order: TL,TR,BL,BR</returns>
        public static List<Vector2> Corners(Rectangle r)
        {
            return new List<Vector2> { TopLeft(r), TopRight(r), BottomLeft(r), BottomRight(r) };
        }

        public static Vector2 TopLeft(Rectangle r)
        {
            return new Vector2(r.Left, r.Top);
        }

        public static Vector2 BottomLeft(Rectangle r)
        {
            return new Vector2(r.Left, r.Bottom);
        }

        public static Vector2 TopRight(Rectangle r)
        {
            return new Vector2(r.Right, r.Top);
        }

        public static Vector2 BottomRight(Rectangle r)
        {
            return new Vector2(r.Right, r.Bottom);
        }

        public static float SlopeFromVector(Vector2 v)
        {
            if (v.X == 0)
            {
                if (v.Y != 0) return float.NaN;
                return 0;
            }
            return v.Y / v.X;
        }

        public static float GetAngle(Vector2 vec1, Vector2 vec2, Vector2 vec3)
        {
            float lenghtA = (float)Math.Sqrt(Math.Pow(vec2.X - vec1.X, 2) + Math.Pow(vec2.Y - vec1.Y, 2));
            float lenghtB = (float)Math.Sqrt(Math.Pow(vec3.X - vec2.X, 2) + Math.Pow(vec3.Y - vec2.Y, 2));
            float lenghtC = (float)Math.Sqrt(Math.Pow(vec3.X - vec1.X, 2) + Math.Pow(vec3.Y - vec1.Y, 2));

            float calc = ((lenghtA * lenghtA) + (lenghtB * lenghtB) - (lenghtC * lenghtC)) / (2 * lenghtA * lenghtB);

            return (float)(Math.Acos(calc) * (180.0 / Math.PI));
        }

        public static Body GetProjection(Body body)
        {
            return new Body(body.CollisionBoxes, body.Position + body.Velocity, body.CollisionType);
        }
    }
}