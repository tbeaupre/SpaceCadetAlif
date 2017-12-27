

using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Utilities;

using System;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    /*
     * This class updates position, velocity, and acceleration of bodies passed to it.
     * Also handles Collision Detection between hitboxes and environment.
     */

    static class PhysicsManager
    {
        private static Objects.Environment environment;

        // Sets the current environment
        public static void InitEnvironment(Objects.Environment e)
        {
            environment = e;
        }

        public static void Update(List<GameObject> objectList)
        {
            HandleAllSimpleCollisions(objectList);
        }

        private static void HandleAllSimpleCollisions(List<GameObject> objectList)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                UpdateMotion(objectList[i].Body);
                bool collided = false;
                for (int j = i + 1; j < objectList.Count; j++)
                {
                    if (HandleComplexCollision(objectList[i], objectList[j]))
                    {
                        collided = true;
                    }
                }
                if (collided)
                {
                    objectList[i].Body.Velocity = Vector2.Zero;
                }
            }
        }

        // Resets body A's position back, velocity of A and B to 0
        private static bool HandleComplexCollision(GameObject A, GameObject B)
        {
            Vector2 relativeVel = A.Body.Velocity - B.Body.Velocity;
            if (A.Body.CollisionType == CollisionType.GHOST || B.Body.CollisionType == CollisionType.GHOST || relativeVel.Length() == 0)
            {
                return false;
            }

            
            float relativeSlope = PhysicsUtilities.SlopeFromVector(relativeVel);

            foreach (Rectangle aRect in A.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                aRect.Offset(A.Body.Position.X, A.Body.Position.Y);
                // copy the rectangle at its projected destination.
                Rectangle projection = new Rectangle(aRect.X + (int)relativeVel.X, aRect.Y + (int)relativeVel.Y, aRect.Width, aRect.Height);
                //critical points are the starting points of rays which have a direction and magnitude of the velocity. represent upper and lower bounds of the total collision box.
                Vector2 upperCriticalPoint; // start point of upper projection ray
                Vector2 lowerCriticalPoint; // start point of lower projection ray
                // set maximum and minimum of projection rays
                if (relativeSlope >= 0)
                {
                    upperCriticalPoint = PhysicsUtilities.TopLeft(aRect);
                    lowerCriticalPoint = PhysicsUtilities.BottomRight(aRect);
                }
                else
                {
                    upperCriticalPoint = PhysicsUtilities.TopRight(aRect);
                    lowerCriticalPoint = PhysicsUtilities.BottomLeft(aRect);
                }
                Rectangle span = new Rectangle(
                Math.Min(projection.Location.X, aRect.Location.X),
                Math.Min(projection.Location.Y, aRect.Location.Y),
                Math.Abs(projection.Location.X - aRect.Location.X),
                Math.Abs(projection.Location.Y - aRect.Location.Y));

                Vector2 offset = Vector2.Zero;

                // loop through all collision boxes in B
                foreach (Rectangle bRect in B.Body.CollisionBoxes)
                {
                    if (Rectangle.Intersect(bRect, span).IsEmpty)
                    {
                        continue;
                    }

                    if (!Rectangle.Intersect(bRect, projection).IsEmpty)
                    {
                        Vector2 localOffset = calculateOffset(bRect, projection, relativeVel);
                        if (localOffset.LengthSquared() > offset.LengthSquared()) {
                            offset = localOffset;
                            continue;
                        }
                    }
                    // A is upper left of
                    float angleABC, angleABD, angleBAC, angleBAD;


                }
            }
            return false;
        }

        private static Vector2 calculateOffset(Rectangle stationaryRect, Rectangle movingRect, Vector2 relativeVel)
        {
            float xOff, yOff;
            bool rightWardX = relativeVel.X > 0;
            bool upWardY = relativeVel.Y < 0;

            float relativeSlope = PhysicsUtilities.SlopeFromVector(relativeVel);
            bool xPriority = false;
            if(!float.IsNaN(relativeSlope)) //if velcocity is not Y direction only
            {
                xPriority = Math.Abs(relativeSlope) <= 1;
            }

            if (xPriority)
            {
                if (rightWardX)
                {
                    xOff = movingRect.Right - stationaryRect.Left;
                }
                else
                {
                    xOff = movingRect.Left - stationaryRect.Right;
                }
                yOff = (relativeVel.Y / relativeVel.X) * (xOff);
            }
            else
            {
                if (upWardY)
                {
                    yOff = movingRect.Top - stationaryRect.Bottom;
                }
                else
                {
                    yOff = movingRect.Bottom - stationaryRect.Top;
                }
                xOff = (relativeVel.X / relativeVel.Y) * (yOff);
            }

            return new Vector2(xOff, yOff);
        }

        private static void UpdateMotion(Body body)
        {
            body.Position += body.Velocity;
            body.Velocity += body.Acceleration;
        }
    }
}

