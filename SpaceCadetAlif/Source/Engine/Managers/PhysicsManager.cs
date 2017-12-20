

using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
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
            UpdateAllMotion(objectList);
        }

        private static void HandleAllSimpleCollisions(List<GameObject> objectList)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                for (int j = i + 1; j < objectList.Count; j++)
                {
                    HandleSimpleCollision(objectList[i], objectList[j]);
                }
            }
        }

        // Resets body A's position back, velocity of A and B to 0
        private static void HandleSimpleCollision(GameObject A, GameObject B)
        {
            if (A.Body.CollisionType == CollisionType.GHOST || B.Body.CollisionType == CollisionType.GHOST)
                return;

            // Relative velocity is the diff between A and B. Useful if both bodies are moving.
            Vector2 relativeVelocity = A.Body.Velocity - B.Body.Velocity;

            // Either X or Y of velocity vector will pass over its entire dimension of the intersectRectangle. 
            // That is max between the two. xPriority means x is max, if false y is max.
            bool xPriority = (relativeVelocity.X > relativeVelocity.Y);

            foreach (Rectangle aRect in A.Body.CollisionBoxes)
            {
                foreach (Rectangle bRect in B.Body.CollisionBoxes)
                {
                    Rectangle intersect = Rectangle.Intersect(aRect, bRect);
                    if (!intersect.IsEmpty) //collision occured!
                    {
                        // May update in the future, but for now set A and B Velocity to 0 and move A back to the closest position 
                        // in the opposite direction of relativeVelocity
                        
                        A.Body.Velocity = Vector2.Zero;
                        B.Body.Velocity = Vector2.Zero;

                        Vector2 error = new Vector2(); // How far back the object should be moved to stop collision.
                        if (xPriority)
                        {
                            error.X = Math.Sign(relativeVelocity.X) * intersect.Width;
                            error.Y = relativeVelocity.Y * (error.X / relativeVelocity.X);
                        }
                        else
                        {
                            error.Y = Math.Sign(relativeVelocity.Y) * intersect.Height;
                            error.X = relativeVelocity.X * (error.Y / relativeVelocity.Y);
                        }
                        // Here's where we ignore physics completely and just set A back to the last available pixel
                        A.Body.Position -= error;
                    }
                }
            }
        }

        private static void UpdateAllMotion(List<GameObject> objectList)
        {
            for (int i = 0; i < objectList.Count; i++)
            {
                UpdateMotion(objectList[i].Body);
            }
        }

        private static void UpdateMotion(Body body)
        {
            body.Position += body.Velocity;
            body.Velocity += body.Acceleration;
        }

    }
}
