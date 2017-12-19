

using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{

   
    /*
     * This class updates position, velocity, and acceleration of bodies passed to it.
     * Also handles Collision Detection between hitboxes and environment.
     */

    

    static class PhysicsManager
    {
        private static Environment environment;
        //Sets the current environment
        public static void InitEnvironment(Environment e)
        {
            environment = e;
        }

        //Resets body A's position back, velocity of A and B to 0
        private static void HandleSimpleCollision(Body A, Body B)
        {
            //Relative velocity is the diff between A and B. Useful if both bodies are moving
            Vector2 relativeVelocity = A.Velocity - B.Velocity;
            //Either X or Y of velocity vector will pass over its entire dimension of the intersectRectangle. 
            //That is max between the two. xPriority means x is max, if false y is max
            bool xPriority = (relativeVelocity.X > relativeVelocity.Y);
            foreach (Rectangle aRect in A.CollisionBoxes)
            {
                foreach (Rectangle bRect in B.CollisionBoxes)
                {
                    Rectangle intersect = Rectangle.Intersect(aRect, bRect);
                    if (!intersect.IsEmpty) //collision occured!
                    {
                        //may update in the future, but for now set A and B Velocity to 0 and move A back to the closest position 
                        //in the opposite direction of relativeVelocity
                        A.Velocity = Vector2.Zero;
                        B.Velocity = Vector2.Zero;
                        Vector2 error = new Vector2();
                        if (xPriority)
                        {
                            if (relativeVelocity.X > 0)
                            {
                                error.X = intersect.Width;
                            } else
                            {
                                error.X = -intersect.Width;
                            }
                            error.Y = relativeVelocity.Y * (error.X / relativeVelocity.X);
                        } else
                        {
                            if (relativeVelocity.Y > 0)
                            {
                                error.Y = intersect.Height;
                            }
                            else
                            {
                                error.Y = -intersect.Height;
                            }
                            error.X = relativeVelocity.X * (error.Y / relativeVelocity.Y);
                        }
                        //here's where we ignore physics completely and just set A back to the last available pixel
                        A.Position -= error; 
                    }
                }
            }
        }

        public static void SimpleCollisionUpdate(List<Body> bodyList)
        {
            for (int i = 0; i < bodyList.Count; i++)
            {
                for(int j = i + 1; j < bodyList.Count; j++)
                {
                    HandleSimpleCollision(bodyList[i], bodyList[j]);
                }
            }
        }
    }
}
