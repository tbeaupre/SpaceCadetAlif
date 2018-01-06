using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Physics;
using System;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Utilities
{
    static class PhysicsUtilities
    {
        public const float DEFAULT_GRAVITY_Y = 0.005f;
        public const float DEFAULT_GRAVITY_X = 0;
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
            return (-v.Y / v.X);
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
            return new Body(body.CollisionBoxes, body.Position + body.Velocity, body.Gravity, body.CollisionType);
        }

        public static bool WithinPath(Rectangle start, Rectangle finish, Rectangle check)
        {

            //critical points are the starting points of rays which have a direction and magnitude of the velocity. 
            //represent upper and lower bounds of the total collision path.
            Vector2 A; 
            Vector2 B; 
            Vector2 C;
            Vector2 D;
            Vector2 E;
            Vector2 F;


            float relativeSlope = PhysicsUtilities.SlopeFromVector(TopLeft(finish) - TopLeft(start));
            if (relativeSlope == float.NaN || relativeSlope >= 0)
            {
                A = PhysicsUtilities.TopLeft(start);
                B = PhysicsUtilities.BottomRight(start);
                C = PhysicsUtilities.TopLeft(finish);
                D = PhysicsUtilities.BottomRight(finish);
                E = PhysicsUtilities.TopLeft(check);
                F = PhysicsUtilities.BottomRight(check);
            }
            else
            {
                A = PhysicsUtilities.TopRight(start);
                B = PhysicsUtilities.BottomLeft(start);
                C = PhysicsUtilities.TopRight(finish);
                D = PhysicsUtilities.BottomLeft(finish);
                E = PhysicsUtilities.TopRight(check);
                F = PhysicsUtilities.BottomLeft(check);
            }

            float angleBAE = GetAngle(B, A, E);
            float angleBAF = GetAngle(B, A, F);
            float angleABE = GetAngle(A, B, E);
            float angleABF = GetAngle(A, B, F);
            float angleBAC = GetAngle(B, A, C);
            float angleABD = GetAngle(A, B, D);

            if (angleBAC <= angleBAF)
            {
                if (angleBAC > angleBAE)
                {
                    return true;
                }
            }
            else
            {
                if (angleABD > angleABF)
                {
                    return true;
                }
            }
            return false;
        }
    }
}