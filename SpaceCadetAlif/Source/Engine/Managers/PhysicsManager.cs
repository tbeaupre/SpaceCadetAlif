﻿
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
        public const float DEFAULT_GRAVITY_Y = 0.005f;
        public const float DEFAULT_GRAVITY_X = 0.00f;

        public const float DEFAULT_FRICTION_COEFFICIENT = 0.1f;
        public const float DEFAULT_FRICTION_THRESHOLD = 0.1f;

        public static void Update(Room currentRoom)
        {
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                WorldManager.ToUpdate[i].Body.UpdateVelocity();
            }
            for (int i = 0; i < WorldManager.ToUpdate.Count - 1; i++)
            {
                for (int j = i + 1; j < WorldManager.ToUpdate.Count; j++)
                {
                    HandleObjectCollision(WorldManager.ToUpdate[i], WorldManager.ToUpdate[j]);
                }
            }
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                HandleEnvironmentCollision(WorldManager.ToUpdate[i], currentRoom);
                WorldManager.ToUpdate[i].Body.UpdatePosition();
            }
        }

        // Resets body A's position back, velocity of A and B to 0
        private static void HandleObjectCollision(GameObject A, GameObject B)
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
                
                DirectionPair dPair = new DirectionPair();
                Rectangle collidingHitboxA = Rectangle.Empty;
                Rectangle collidingHitboxB = Rectangle.Empty;
         
                bool collided = false;

                relativeVel = A.Body.Velocity - B.Body.Velocity;
                int xVel = (int)relativeVel.X;
                int yVel = (int)relativeVel.Y;

                //handle the issue of rounding float velocities for use with int rectangles
                if (relativeVel.X < 0) xVel -= 1;
                if (relativeVel.Y < 0) yVel -= 1;
                if (relativeVel.X > 0) xVel += 1;
                if (relativeVel.Y > 0) yVel += 1;

                // copy the rectangle at its projected destination.
                Rectangle projection = new Rectangle(aRect.X + xVel, aRect.Y + yVel, aRect.Width, aRect.Height);
                Rectangle span = Rectangle.Union(aRect, projection);

                // loop through all collision boxes in B to find the colliding hitboxes
                foreach (Rectangle bRect in B.Body.CollisionBoxes)
                {
                   

                    // offset the rectangle to the body's location
                    bRect.Offset(B.Body.Position.X, B.Body.Position.Y);
                    if (!bRect.Intersects(span))
                    {
                        continue;
                    }
                    if (projection.Intersects(bRect) || PhysicsUtilities.WithinPath(aRect, projection, bRect))
                    {
                        //Resulting collision velocity
                        A.Body.Velocity = NextVelocity(A.Body.Velocity, aRect, bRect);
                        float aRectDist = Vector2.Distance(aRect.Center.ToVector2(), bRect.Center.ToVector2());
                        float collidingRectDist = Vector2.Distance(collidingHitboxA.Center.ToVector2(), collidingHitboxB.Center.ToVector2());

                        if (!collided || aRectDist < collidingRectDist)
                        {
                            collidingHitboxB = bRect;
                            collidingHitboxA = aRect;
                            collided = true;
                        }
                    }
                }
                if (collided)
                {
                    Direction leadingEdge = GetCollisionDirection(collidingHitboxA, collidingHitboxB, relativeVel);
                    A.Body.Position += SnapToEdge(collidingHitboxA, collidingHitboxB, relativeVel, leadingEdge);
                    A.Body.Velocity = Vector2.Zero;
                    A.Body.Acceleration = Vector2.Zero;
                    B.Body.Velocity = Vector2.Zero;
                    B.Body.Acceleration = Vector2.Zero;
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
        private static void HandleEnvironmentCollision(GameObject obj, Room currentRoom)
        {
            if (obj.Body.CollisionType == CollisionType.GHOST || obj.Body.Velocity.Length() == 0)
            {
                return;
            }

            Vector2 velocity = obj.Body.Velocity;
            float relativeSlope = PhysicsUtilities.SlopeFromVector(velocity);
            bool collided = false;
            DirectionPair collisionDirectionPair = new DirectionPair();
            Rectangle closestPixel = Rectangle.Empty;
            Rectangle collidingRect = Rectangle.Empty;

            Vector2 relativeVel = obj.Body.Velocity;

            int xVel = (int)relativeVel.X;
            int yVel = (int)relativeVel.Y;

            //handle the issue of rounding float velocities for use with int rectangles
            if (relativeVel.X < 0) xVel -= 1;
            if (relativeVel.Y < 0) yVel -= 1;
            if (relativeVel.X > 0) xVel += 1;
            if (relativeVel.Y > 0) yVel += 1;

            foreach (Rectangle rect in obj.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                rect.Offset(obj.Body.Position.X, obj.Body.Position.Y);
                // copy the rectangle at its projected destination.
                Rectangle projection;
                projection = new Rectangle(rect.X + xVel, rect.Y + yVel, rect.Width, rect.Height);
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
                        Color currentColor = currentRoom.ColorData[i + j * currentRoom.GetCollision().Width];
                        if (currentColor.A != 0) // alpha != 0
                        {
                            Rectangle currentPixel = new Rectangle(i, j, 1, 1);

                            if (projection.Intersects(currentPixel) || PhysicsUtilities.WithinPath(rect, projection, currentPixel))
                            {
                                obj.Body.Velocity = NextVelocity(obj.Body.Velocity, rect, currentPixel);
                                //TODO Figure out how to handle X and Y seperately

                                if (!collided || Vector2.Distance(rect.Center.ToVector2(), currentPixel.Center.ToVector2()) < Vector2.Distance(collidingRect.Center.ToVector2(), closestPixel.Center.ToVector2()))
                                {
                                    closestPixel = currentPixel;
                                    collidingRect = rect;
                                    collided = true;
                                }
                            }
                            if (collided)
                            {
                                Direction leadingEdge = GetCollisionDirection(collidingRect, closestPixel, relativeVel);
                                obj.Body.Position += SnapToEdge(collidingRect, closestPixel, relativeVel, leadingEdge);
                                obj.Body.Velocity = Vector2.Zero;
                                obj.Body.Acceleration = Vector2.Zero;
                            }
                        }
                    }
                }
            }
            Console.WriteLine(velocity + " " + collided + " " + obj.Body.Position);

            if (collided)
            {
                Direction leadingEdge = GetCollisionDirection(collidingRect, closestPixel, velocity);
                obj.Body.Position += SnapToEdge(collidingRect, closestPixel, velocity, leadingEdge);
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
                CollisionEventArgs collisionEventArgs = new CollisionEventArgs(obj, currentRoom, collisionDirectionPair);
                obj.OnCollision(collisionEventArgs);
            }
        }


        private static Vector2 SnapToEdge(Rectangle collider, Rectangle stationaryRect, Vector2 velocity, Direction leadingEdge)
        {
            float xOffset = 0, yOffset = 0;
            switch (leadingEdge)
            {
                case Direction.NONE:
                    break;
                case Direction.UP:
                    yOffset = stationaryRect.Bottom - collider.Top;
                    xOffset = (velocity.X / velocity.Y) * yOffset;
                    break;
                case Direction.DOWN:
                    yOffset = stationaryRect.Top - collider.Bottom;
                    xOffset = (velocity.X / velocity.Y) * yOffset;
                    break;
                case Direction.LEFT:
                    xOffset = stationaryRect.Right - collider.Left;
                    yOffset = (velocity.Y / velocity.X) * xOffset;
                    break;
                case Direction.RIGHT:
                    xOffset = stationaryRect.Left - collider.Right;
                    yOffset = (velocity.Y / velocity.X) * xOffset;
                    break;
            }
            return new Vector2(xOffset, yOffset);
        }

        private static Vector2 NextVelocity(Vector2 velocity, Rectangle rectA, Rectangle rectB)
        {
            float X = velocity.X;
            float Y = velocity.Y;

            if ((PhysicsUtilities.above(rectA, rectB) || PhysicsUtilities.below(rectA, rectB)) && !(PhysicsUtilities.leftOf(rectA, rectB) || PhysicsUtilities.rightOf(rectA, rectB)))
            {
                Y = 0;
            }
            if ((PhysicsUtilities.leftOf(rectA, rectB) || PhysicsUtilities.rightOf(rectA, rectB)) && !(PhysicsUtilities.above(rectA, rectB) || PhysicsUtilities.below(rectA, rectB)))
            {
                X = 0;
            }
            return new Vector2(X, Y);
        }

        private static Direction GetCollisionDirection(Rectangle collidingRect, Rectangle stationaryRect, Vector2 velocity)
        {
            
            Vector2 collidingCorner = collidingRect.Center.ToVector2();
            Vector2 receivingCorner = stationaryRect.Center.ToVector2();

            DirectionPair velPair = PhysicsUtilities.GetDirectionPair(velocity);
            if (velPair.X == Direction.NONE || velPair.Y == Direction.NONE)
            {
                return PhysicsUtilities.GetDirectionFromVector(velocity);
            }

            if (velPair.Y == Direction.DOWN)
            {
                collidingCorner.Y = collidingRect.Bottom;
                receivingCorner.Y = stationaryRect.Top;
            }
            else
            {
                collidingCorner.Y = collidingRect.Top;
                receivingCorner.Y = stationaryRect.Bottom;
            }

            if (velPair.X == Direction.RIGHT)
            {
                collidingCorner.X = collidingRect.Right;
                receivingCorner.X = stationaryRect.Left;
            }
            else
            {
                collidingCorner.X = collidingRect.Left;
                receivingCorner.X = stationaryRect.Right;
            }


            //Check angles. If the angle of collision is less than the angle between the corners then it was a Y dir collision
            Vector2 referenceVector = Vector2.Zero;
            Direction A = Direction.NONE;
            Direction B = Direction.NONE;
            //SO CLOSE! Just need to use a different method than get Touching edge that makes sure the edge is directly next to colliding. I'
            switch (PhysicsUtilities.GetRelativePositionDirection(collidingRect, stationaryRect))
            {
                case Direction.LEFT:
                    referenceVector = collidingCorner + new Vector2(-1, 0);
                    A = velPair.X;
                    B = velPair.Y;
                    break;
                case Direction.DOWN:
                    referenceVector = collidingCorner + new Vector2(0, 1);
                    A = velPair.Y;
                    B = velPair.X;
                    break;
                case Direction.RIGHT:
                    referenceVector = collidingCorner + new Vector2(1, 0);
                    A = velPair.X;
                    B = velPair.Y;
                    break;
                case Direction.UP:
                    referenceVector = collidingCorner + new Vector2(0, -1);
                    A = velPair.Y;
                    B = velPair.X;
                    break;
            }

            float collisionAngle = PhysicsUtilities.GetAngle(referenceVector, collidingCorner, collidingCorner + velocity);
            float angleBetweenCorners = PhysicsUtilities.GetAngle(referenceVector, collidingCorner, receivingCorner);

            if (collisionAngle < angleBetweenCorners)
            {
                return A;
            }

            return B;
        }


    }
}