
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Utilities;
using System;
using System.Collections.Generic;
using SpaceCadetAlif.Source.Engine.Physics;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    /*
     * This class updates position, velocity, and acceleration of bodies passed to it.
     * Also handles Collision Detection between hitboxes and environment.
     */

    static class PhysicsManager
    {
        public const float DEFAULT_GRAVITY_Y = 0.05f;
        public const float DEFAULT_GRAVITY_X = 0.00f;

        public const float DEFAULT_FRICTION_COEFFICIENT = 0.1f;
        public const float DEFAULT_FRICTION_THRESHOLD = 0.1f;

        public static void Update(Room currentRoom)
        {
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                WorldManager.ToUpdate[i].Body.UpdateVelocity();
            }
<<<<<<< HEAD

            for (int i = 0; i < WorldManager.ToUpdate.Count - 1; i++)
=======
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
>>>>>>> parent of 4fa4635... slow pokin
            {
                for (int j = i + 1; j < WorldManager.ToUpdate.Count; j++)
                {
                    UpdateObjectToObject(WorldManager.ToUpdate[i], WorldManager.ToUpdate[j]);
                }
<<<<<<< HEAD
            }

            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                UpdateObjectToEnvironment(WorldManager.ToUpdate[i], currentRoom);
=======

                HandleEnvironmentCollision(WorldManager.ToUpdate[i], currentRoom);
>>>>>>> parent of 4fa4635... slow pokin
                WorldManager.ToUpdate[i].Body.UpdatePosition();
            }
        }



        // Resets body A's position back, velocity of A and B to 0
        private static void UpdateObjectToObject(GameObject A, GameObject B)
        {
            Vector2 relativeVel = A.Body.Velocity - B.Body.Velocity;
            if (A.Body.CollisionType == CollisionType.GHOST || B.Body.CollisionType == CollisionType.GHOST || relativeVel.Length() == 0)
            {
                return;
            }
            foreach (Rectangle aRect in A.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                aRect.Offset(A.Body.Position.X, A.Body.Position.Y);
                // copy the rectangle at its projected destination.
                Rectangle projection = new Rectangle(aRect.X + (int)relativeVel.X, aRect.Y + (int)relativeVel.Y, aRect.Width, aRect.Height);
                Rectangle span = Rectangle.Union(aRect, projection);
                DirectionPair dPair = new DirectionPair();
                Vector2 offset = Vector2.Zero;
                Vector2 minDist = new Vector2(float.MaxValue, float.MaxValue);
                Vector2 newVelocity = relativeVel;
                bool collided = false;
                // loop through all collision boxes in B
                foreach (Rectangle bRect in B.Body.CollisionBoxes)
                {
                    // offset the rectangle to the body's location
                    bRect.Offset(B.Body.Position.X, B.Body.Position.Y);
                    if (Rectangle.Intersect(bRect, span).IsEmpty)
                    {
                        continue;
                    }
                    if (projection.Contains(bRect) || PhysicsUtilities.WithinPath(aRect, projection, bRect))
                    {
                        HandleCollision(aRect, bRect, relativeVel, ref newVelocity, ref offset, ref minDist);
                    }
                }
                if (collided)
                {
                    CollisionEventArgs collisionEventArgs = new CollisionEventArgs(A, B, dPair);
                    A.OnCollision(collisionEventArgs);
                }
            }
        }


        /* current issues
         * 1. closest pixel determined by center of collider to center of collidee, but that may not be an adequate assumption. 
         * fix is either to calculate point of contact (expensive) or to keep collision boxes as a set of squares (cheaper, but annoying) 
         * 
         * 2. snap to edge does not currently take into account snapping through an object on a different axis. Not sure how to fix this.
         * perhaps a check after the fact to see if rect contains any environment pixels, but thats pricey too.
         */
        private static void UpdateObjectToEnvironment(GameObject obj, Room currentRoom)
        {
            if (obj.Body.CollisionType == CollisionType.GHOST || obj.Body.Velocity.Length() == 0)
            {
                return;
            }

            Vector2 velocity = obj.Body.Velocity;
            float relativeSlope = PhysicsUtilities.SlopeFromVector(velocity);

            //for some reason, we need to store the data from Texture2D.getCollision() as a *1D* array of colors
            Color[] cList = new Color[currentRoom.GetCollision().Width * currentRoom.GetCollision().Height];
            currentRoom.GetCollision().GetData(cList);
            DirectionPair collisionDirectionPair = PhysicsUtilities.GetDirectionPair(velocity);
            bool collided = false;
            Vector2 offset = Vector2.Zero;
            Vector2 minDist = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 newVelocity = velocity;

            foreach (Rectangle rect in obj.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                rect.Offset(obj.Body.Position.X, obj.Body.Position.Y);
                // copy the rectangle at its projected destination.
                int xVelOffset = (int)Math.Ceiling(velocity.X), yVelOffset = (int)Math.Ceiling(velocity.Y);

                Rectangle projection = new Rectangle(rect.X + xVelOffset, rect.Y + yVelOffset, rect.Width, rect.Height);
                Rectangle objSpan = Rectangle.Union(rect, projection); // span of projection hitbox
                Rectangle roomSpan = currentRoom.GetCollision().Bounds;// outline of the room

                //to prevent index out of bounds exception
                int top = Math.Max(objSpan.Top, roomSpan.Top);
                int bot = Math.Min(objSpan.Bottom, roomSpan.Bottom);
                int left = Math.Max(objSpan.Left, roomSpan.Left);
                int right = Math.Min(objSpan.Right, roomSpan.Right);

                for (int j = top; j < bot; j++)
                {
                    for (int i = left; i < right; i++)
                    {
                        Color currentColor = cList[i + j * currentRoom.GetCollision().Width];
                        if (currentColor.A != 0) // alpha != 0
                        {
                            Rectangle currentPixel = new Rectangle(i, j, 1, 1);

                            if (projection.Contains(currentPixel) || PhysicsUtilities.WithinPath(rect, projection, currentPixel))
                            {

                                HandleCollision(rect, currentPixel, velocity, ref newVelocity, ref offset, ref minDist);
                                collided = true;
                            }
                        }
                    }
                }
            }
            Console.WriteLine(velocity + " " + collided + " " + obj.Body.Position);

            if (collided)
            {
                /*
                if (leadingEdge == PhysicsUtilities.GetDirectionFromVector(obj.Body.Gravity))
                {
                    if (obj.Body.Velocity.Length() < DEFAULT_FRICTION_THRESHOLD)
                    {
                        obj.Body.Velocity = Vector2.Zero;
                        obj.Body.Acceleration = Vector2.Zero;
                    }
                    else
                    {
                        obj.Body.Acceleration = -obj.Body.Velocity * DEFAULT_FRICTION_COEFFICIENT;
                    }
                }
                */
                obj.Body.Position += offset;
                obj.Body.Velocity = newVelocity;
                CollisionEventArgs collisionEventArgs = new CollisionEventArgs(obj, currentRoom, collisionDirectionPair);
                obj.OnCollision(collisionEventArgs);
            }
        }

        private static void HandleCollision(Rectangle A, Rectangle B, Vector2 velocity, ref Vector2 newVelocity, ref Vector2 offset, ref Vector2 minDist)
        {
            Vector2 CriticalCornerA = PhysicsUtilities.GetCriticalCornerFromVector(A, velocity);
            Vector2 CriticalCornerB = PhysicsUtilities.GetCriticalCornerFromVector(B, -velocity);
            Vector2 dist = new Vector2(Math.Abs(CriticalCornerA.X - CriticalCornerB.X), Math.Abs(CriticalCornerA.Y - CriticalCornerB.Y));

            if (dist.LengthSquared() > minDist.LengthSquared())
            {
                return;
            }


            Direction leadingEdge = getCollisionDirection(A, B, CriticalCornerA, CriticalCornerB, velocity);
            SetPosAndVel(A, B, velocity, ref newVelocity, ref offset, leadingEdge);
            minDist = velocity - offset;
        }

        private static Direction getCollisionDirection(Rectangle rectA, Rectangle rectB, Vector2 criticalCornerA, Vector2 criticalCornerB, Vector2 velocity)
        {
            
            if (velocity.X == 0 || velocity.Y == 0)
            {
                return PhysicsUtilities.GetDirectionFromVector(velocity);
            }
            DirectionPair velPair = PhysicsUtilities.GetDirectionPair(velocity);

            //Check angles. If the angle of collision is less than the angle between the corners then it was a Y dir collision
            Vector2 referenceVector = Vector2.Zero;
            Direction A = Direction.NONE;
            Direction B = Direction.NONE;
            //SO CLOSE! Just need to use a different method than get Touching edge that makes sure the edge is directly next to colliding. I'
            switch (PhysicsUtilities.GetRelativePositionDirection(rectA, rectB))
            {
                case Direction.LEFT:
                    referenceVector = criticalCornerA + new Vector2(-1, 0);
                    A = velPair.X;
                    B = velPair.Y;
                    break;
                case Direction.DOWN:
                    referenceVector = criticalCornerA + new Vector2(0, 1);
                    A = velPair.Y;
                    B = velPair.X;
                    break;
                case Direction.RIGHT:
                    referenceVector = criticalCornerA + new Vector2(1, 0);
                    A = velPair.X;
                    B = velPair.Y;
                    break;
                case Direction.UP:
                    referenceVector = criticalCornerA + new Vector2(0, -1);
                    A = velPair.Y;
                    B = velPair.X;
                    break;
            }

            float collisionAngle = PhysicsUtilities.GetAngle(referenceVector, criticalCornerA, criticalCornerA + velocity);
            float angleBetweenCorners = PhysicsUtilities.GetAngle(referenceVector, criticalCornerA, criticalCornerB);

            if (collisionAngle < angleBetweenCorners)
            {
                return A;
            }
            return B;
        }




        private static void SetPosAndVel(Rectangle A, Rectangle B, Vector2 velocity, ref Vector2 newVelocity, ref Vector2 offset, Direction leadingEdge)
        {
            switch (leadingEdge)
            {
                case Direction.NONE:
                    break;
                case Direction.UP:
                    offset.Y = B.Bottom - A.Top;
                    offset.X = (velocity.X / velocity.Y) * offset.Y;
                    newVelocity.Y = 0;
                    break;
                case Direction.DOWN:
                    offset.Y = B.Top - A.Bottom;
                    offset.X = (velocity.X / velocity.Y) * offset.Y;
                    newVelocity.Y = 0;
                    break;
                case Direction.LEFT:
                    offset.X = B.Right - A.Left -1;
                    offset.Y = (velocity.Y / velocity.X) * offset.X;
                    newVelocity.X = 0;
                    break;
                case Direction.RIGHT:
                    offset.X = B.Left - A.Right;
                    offset.Y = (velocity.Y / velocity.X) * offset.X;
                    newVelocity.X = 0;
                    break;
            }
        }
    }
}