
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
        public static void Update(Room currentRoom)
        {
            bool[] collisionRecord = new bool[WorldManager.ToUpdate.Count];
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                UpdateMotion(WorldManager.ToUpdate[i].Body);
                for (int j = i + 1; j < WorldManager.ToUpdate.Count; j++)
                {
                    if (currentRoom != null && HandleEnvironmentCollision(WorldManager.ToUpdate[i], currentRoom))
                    {
                        if (WorldManager.ToUpdate[i].Body.CollisionType == CollisionType.SOLID && WorldManager.ToUpdate[j].Body.CollisionType == CollisionType.SOLID)
                        {
                            collisionRecord[i] = true;
                            collisionRecord[j] = true;
                        }
                    }
                }
                
                if (currentRoom != null && HandleEnvironmentCollision(WorldManager.ToUpdate[i], currentRoom)){
                    collisionRecord[i] = true;
                }
            }
            SetVelocities(collisionRecord);
        }

        private static void SetVelocities(bool[] collisionRecord)
        {
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                if (collisionRecord[i])
                {
                    //current fix. badass physics engine to go here. to be honest though I don't know why we handle it here. more versitile handling object by object.
                    WorldManager.ToUpdate[i].Body.Velocity = Vector2.Zero;
                }
            }
        }



        // Resets body A's position back, velocity of A and B to 0
        private static bool HandleCollision(GameObject A, GameObject B)
        {
            Vector2 relativeVel = A.Body.Velocity - B.Body.Velocity;
            if (A.Body.CollisionType == CollisionType.GHOST || B.Body.CollisionType == CollisionType.GHOST || relativeVel.Length() == 0)
            {
                return false;
            }
            bool collided = false;

            foreach (Rectangle aRect in A.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                aRect.Offset(A.Body.Position.X, A.Body.Position.Y);
                // copy the rectangle at its projected destination.
                Rectangle projection = new Rectangle(aRect.X + (int)relativeVel.X, aRect.Y + (int)relativeVel.Y, aRect.Width, aRect.Height);
                Rectangle span = Rectangle.Union(aRect, projection);
                OffsetAndDirectionData offset = new OffsetAndDirectionData();

                // loop through all collision boxes in B
                foreach (Rectangle bRect in B.Body.CollisionBoxes)
                {
                    // offset the rectangle to the body's location
                    bRect.Offset(B.Body.Position.X, B.Body.Position.Y);
                    if (Rectangle.Intersect(bRect, span).IsEmpty)
                    {
                        continue;
                    }
                    if (!Rectangle.Intersect(bRect, projection).IsEmpty)
                    {
                        collided = true;
                        if (A.Body.CollisionType == CollisionType.SOLID && B.Body.CollisionType == CollisionType.SOLID)
                        {
                            OffsetAndDirectionData localOffset = calculateOffset(bRect, projection, relativeVel);
                            if (localOffset.OffsetVector.LengthSquared() > offset.OffsetVector.LengthSquared())
                            {
                                offset = localOffset;
                                continue;
                            }
                        }
                    }

                    if (PhysicsUtilities.WithinPath(aRect, projection, bRect))
                    {
                        collided = true;
                        if (A.Body.CollisionType == CollisionType.SOLID && B.Body.CollisionType == CollisionType.SOLID)
                        {
                            OffsetAndDirectionData localOffset = calculateOffset(bRect, projection, relativeVel);
                            if (localOffset.OffsetVector.LengthSquared() > offset.OffsetVector.LengthSquared())
                            {
                                offset = localOffset;
                                continue;
                            }
                        }
                    }
                }
                // send collision event out with Direction
                if (collided)
                {
                    //shift A over by the offset if both are solid
                    if (A.Body.CollisionType == CollisionType.SOLID && B.Body.CollisionType == CollisionType.SOLID)
                    {
                        A.Body.Position -= offset.OffsetVector;
                    }
                    CollisionEventArgs collisionEventArgs = new CollisionEventArgs(A, B, offset.Direction);
                    A.OnCollision(collisionEventArgs);
                    B.OnCollision(collisionEventArgs);
                }
            }
            return collided;
        }


        private static OffsetAndDirectionData calculateOffset(Rectangle stationaryRect, Rectangle movingRect, Vector2 relativeVel)
        {
            float xFromXOff, yFromYOff, xFromYOff, yFromXOff;
            bool rightWardX = relativeVel.X > 0;
            bool upWardY = relativeVel.Y < 0;
            OffsetAndDirectionData offsetDataFromX = new OffsetAndDirectionData(), offsetDataFromY = new OffsetAndDirectionData();
            Direction directionFromX, directionFromY;
            float relativeSlope = PhysicsUtilities.SlopeFromVector(relativeVel);

            if (relativeVel.X != 0)
            {
                if (rightWardX)
                {
                    xFromXOff = movingRect.Right - stationaryRect.Left;
                    directionFromX = Direction.RIGHT;
                }
                else
                {
                    xFromXOff = movingRect.Left - stationaryRect.Right;
                    directionFromX = Direction.LEFT;
                }
                yFromXOff = (relativeVel.Y / relativeVel.X) * (xFromXOff);
                offsetDataFromX = new OffsetAndDirectionData(new Vector2(xFromXOff, yFromXOff), directionFromX);
            }
            if (relativeVel.Y != 0)
            {
                if (upWardY)
                {
                    yFromYOff = movingRect.Top - stationaryRect.Bottom;
                    directionFromY = Direction.UP;
                }
                else
                {
                    yFromYOff = movingRect.Bottom - stationaryRect.Top;
                    directionFromY = Direction.DOWN;
                }
                xFromYOff = (relativeVel.X / relativeVel.Y) * (yFromYOff);
                offsetDataFromY = new OffsetAndDirectionData(new Vector2(xFromYOff, yFromYOff), directionFromY);
            }
            // return the min offset
            if (offsetDataFromX.OffsetVector.LengthSquared() <= offsetDataFromY.OffsetVector.LengthSquared())
            {
                return offsetDataFromX;
            }
            else
            {
                return offsetDataFromY;
            }
        }

        // simple class for storing offset and direction of motion upon collision
        private class OffsetAndDirectionData
        {
            public Vector2 OffsetVector { get; set; }
            public Direction Direction { get; set; }
            public OffsetAndDirectionData(Vector2 offset, Direction dir)
            {
                OffsetVector = offset;
                Direction = dir;
            }
            //default for soft collisions
            public OffsetAndDirectionData()
            {
                OffsetVector = Vector2.Zero;
                Direction = Direction.NONE;
            }
        }

        private static bool HandleEnvironmentCollision(GameObject obj, Room currentRoom)
        {
            if (obj.Body.CollisionType == CollisionType.GHOST || obj.Body.Velocity.Length() == 0)
            {
                return false;
            }
            bool collided = false;

            Vector2 velocity = obj.Body.Velocity;
            float relativeSlope = PhysicsUtilities.SlopeFromVector(velocity);

            //for some reason, we need to store the data from Texture2D.getCollision() as a *1D* array of colors
            Color[] cList = new Color[currentRoom.GetCollision().Width * currentRoom.GetCollision().Height];
            currentRoom.GetCollision().GetData(cList);

            foreach (Rectangle rect in obj.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                rect.Offset(obj.Body.Position.X, obj.Body.Position.Y);
                // copy the rectangle at its projected destination.
                Rectangle projection = new Rectangle(rect.X + (int)velocity.X, rect.Y + (int)velocity.Y, rect.Width, rect.Height);
               
                Rectangle span = Rectangle.Union(rect, projection);
                OffsetAndDirectionData offset = new OffsetAndDirectionData();

                for (int j = span.Top; j < span.Bottom; j++)
                {
                    for (int i = span.Left; i < span.Right; i++)
                    {
                        Color currentColor = cList[i + j * currentRoom.GetCollision().Width];
                        if(currentColor.A != 0) // alpha != 0
                        {
                            Rectangle pixel = new Rectangle(i, j, 1, 1);
                            if (projection.Contains(new Point(i, j)))
                            {
                                collided = true;
                                OffsetAndDirectionData offsetData = calculateOffset(
                                    pixel, projection, velocity);
                            }
                            else
                            {
                                if (PhysicsUtilities.WithinPath(rect, projection, pixel))
                                {
                                    collided = true;
                                    if (obj.Body.CollisionType == CollisionType.SOLID)
                                    {
                                        OffsetAndDirectionData localOffset = calculateOffset(pixel, projection, velocity);
                                        if (localOffset.OffsetVector.LengthSquared() > offset.OffsetVector.LengthSquared())
                                        {
                                            offset = localOffset;
                                            continue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return collided;
        }

        private static void UpdateMotion(Body body)
        {
            body.Position += body.Velocity;
            body.Velocity += body.Acceleration + body.Gravity;
        }
    }
}