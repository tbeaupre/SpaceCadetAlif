
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
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                WorldManager.ToUpdate[i].Body.UpdateVelocity();
                WorldManager.ToUpdate[i].Body.UpdatePosition();

            }
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                for (int j = i + 1; j < WorldManager.ToUpdate.Count; j++)
                {
                    HandleCollision(WorldManager.ToUpdate[i], WorldManager.ToUpdate[j]);
                }

                HandleEnvironmentCollision(WorldManager.ToUpdate[i], currentRoom);

            }
        }



        // Resets body A's position back, velocity of A and B to 0
        private static void HandleCollision(GameObject A, GameObject B)
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
                float maxXOffset = 0, maxYOffset = 0;
                // loop through all collision boxes in B
                foreach (Rectangle bRect in B.Body.CollisionBoxes)
                {
                    // offset the rectangle to the body's location
                    bRect.Offset(B.Body.Position.X, B.Body.Position.Y);
                    if (Rectangle.Intersect(bRect, span).IsEmpty)
                    {
                        continue;
                    }
                    if (!Rectangle.Intersect(bRect, projection).IsEmpty || PhysicsUtilities.WithinPath(aRect, projection, bRect))
                    {
                        if (A.Body.CollisionType == CollisionType.SOLID)
                        {
                            OffsetAndDirectionData offset = calculateOffset(bRect, projection, relativeVel);
                            if (Math.Abs(offset.OffsetVector.X) > Math.Abs(maxXOffset))
                            {
                                maxXOffset = offset.OffsetVector.X;
                            }
                            if (Math.Abs(offset.OffsetVector.Y) > Math.Abs(maxYOffset))
                            {
                                maxXOffset = offset.OffsetVector.Y;
                            }


                            dPair.add(offset.Direction);

                            CollisionEventArgs collision = new CollisionEventArgs(A, B, dPair);
                            A.OnCollision(collision);
                            B.OnCollision(collision);
                        }
                    }
                }

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

        private static void HandleEnvironmentCollision(GameObject obj, Room currentRoom)
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

            foreach (Rectangle rect in obj.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                rect.Offset(obj.Body.Position.X, obj.Body.Position.Y);
                // copy the rectangle at its projected destination.
                Rectangle projection;
                int nextX = 0, nextY = 0 ;
                if(velocity.Y >= 1 && velocity.X >=1)
                {
                    nextX = (int)velocity.X;
                    nextY = (int)velocity.Y;
                }
                else
                {
                    Vector2 tangent = velocity / Math.Max(velocity.X, velocity.Y); // largest dimension of this vector is 1
                    nextX = (int)tangent.X;
                    nextY = (int)tangent.Y;
                }

                projection = new Rectangle(rect.X + nextX, rect.Y + nextY, rect.Width, rect.Height);

                Rectangle objSpan = Rectangle.Union(rect, projection); // span of projection hitbox
                Rectangle roomSpan = currentRoom.GetCollision().Bounds;// outline of the room

                bool collided = false;


                //to prevent index out of bounds exception
                int top = Math.Max(objSpan.Top, roomSpan.Top);
                int bot = Math.Min(objSpan.Bottom, roomSpan.Bottom);
                int left = Math.Max(objSpan.Left, roomSpan.Left);
                int right = Math.Min(objSpan.Right, roomSpan.Right);

                DirectionPair dPair = new DirectionPair();
                float maxXOffset = 0, maxYOffset = 0;

                for (int j = top; j < bot; j++)
                {
                    for (int i = left; i < right; i++)
                    {
                        Color currentColor = cList[i + j * currentRoom.GetCollision().Width];
                        if (currentColor.A != 0) // alpha != 0
                        {
                            Rectangle pixel = new Rectangle(i, j, 1, 1);

                            if (rect.Contains(new Point(i, j)) || projection.Contains(new Point(i, j)) || PhysicsUtilities.WithinPath(rect, projection, pixel))
                            {
                                collided = true;
                                if (obj.Body.CollisionType == CollisionType.SOLID)
                                {
                                    OffsetAndDirectionData offset = calculateOffset(pixel, projection, velocity);
                                    if (Math.Abs(offset.OffsetVector.X) > Math.Abs(maxXOffset))
                                    {
                                        maxXOffset = offset.OffsetVector.X;
                                    }
                                    if (Math.Abs(offset.OffsetVector.Y) > Math.Abs(maxYOffset))
                                    {
                                        maxYOffset = offset.OffsetVector.Y;
                                    }
                                    dPair.add(offset.Direction);
                                }
                            }
                        }
                    }
                }
                if (collided)
                {
                    if (obj.Body.CollisionType == CollisionType.SOLID)
                    {
                        obj.Body.Position -= new Vector2(maxXOffset, maxYOffset);
                        SetVelocity(dPair, obj.Body);
                    }
                    CollisionEventArgs collisionEventArgs = new CollisionEventArgs(obj, currentRoom, dPair);
                    obj.OnCollision(collisionEventArgs);
                }
            }
        }

        private static void SetVelocity(DirectionPair dPair, Body body)
        {
            if (dPair.contains(Direction.LEFT) || dPair.contains(Direction.RIGHT))
            {
                body.Velocity = new Vector2(0, body.Velocity.Y);
            }
            if (dPair.contains(Direction.DOWN) || dPair.contains(Direction.UP))
            {
                body.Velocity = new Vector2(body.Velocity.X, 0);
            }
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

                if (Math.Abs(xFromXOff) <= 1)
                {
                    yFromXOff = 0;
                }
                else
                {
                    yFromXOff = (relativeVel.Y / relativeVel.X) * (xFromXOff);
                }

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

                if (Math.Abs(yFromYOff) <= 1)
                {
                    xFromYOff = 0;
                }
                else
                {
                    xFromYOff = (relativeVel.X / relativeVel.Y) * (yFromYOff);
                }
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


    }
}