
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

            }
            for (int i = 0; i < WorldManager.ToUpdate.Count; i++)
            {
                for (int j = i + 1; j < WorldManager.ToUpdate.Count; j++)
                {
                    HandleCollision(WorldManager.ToUpdate[i], WorldManager.ToUpdate[j]);
                }

                HandleEnvironmentCollision(WorldManager.ToUpdate[i], currentRoom);
                WorldManager.ToUpdate[i].Body.UpdatePosition();
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



                projection = new Rectangle(rect.X + (int)velocity.X, rect.Y + (int)velocity.Y, rect.Width, rect.Height);

                Rectangle objSpan = Rectangle.Union(rect, projection); // span of projection hitbox
                Rectangle roomSpan = currentRoom.GetCollision().Bounds;// outline of the room

                bool collided = false;


                //to prevent index out of bounds exception
                int top = Math.Max(objSpan.Top, roomSpan.Top);
                int bot = Math.Min(objSpan.Bottom, roomSpan.Bottom);
                int left = Math.Max(objSpan.Left, roomSpan.Left);
                int right = Math.Min(objSpan.Right, roomSpan.Right);

                DirectionPair dPair = new DirectionPair();
                Rectangle closestPixel = Rectangle.Empty;

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
                                obj.Body.Velocity  = NextVelocity(obj.Body, rect, currentPixel);
                                if (rect.Contains(currentPixel))
                                {
                                    //todo write a snap function and a collision direction function
                                }
                            }
                        }
                    }
                }
                if (collided)
                {
                    CollisionEventArgs collisionEventArgs = new CollisionEventArgs(obj, currentRoom, dPair);
                    obj.OnCollision(collisionEventArgs);
                }
            }
        }

        private static Vector2 NextVelocity(Body body, Rectangle rectA, Rectangle rectB)
        {

            float X = body.Velocity.X;
            float Y = body.Velocity.Y;
            bool left = rectA.Right <= (rectB.Left + 1);
            bool right = rectA.Left >= (rectB.Right - 1);
            bool above = rectA.Bottom <= (rectB.Top + 1);
            bool below = rectA.Top >= (rectB.Bottom - 1);

            if ((above || below) && !(left || right))
            {
                Y = 0;
            }
            if ((left || right) && !(above || below))
            {
                X = 0;
            }

            return new Vector2(X, Y);
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

                yFromXOff = 0;
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

                xFromYOff = 0;
                offsetDataFromY = new OffsetAndDirectionData(new Vector2(xFromYOff, yFromYOff), directionFromY);
            }



            // return the min offset
            if (offsetDataFromX.Direction == Direction.NONE)
            {
                return offsetDataFromY;
            }
            if (offsetDataFromY.Direction == Direction.NONE)
            {
                return offsetDataFromX;
            }
            if (offsetDataFromX.OffsetVector.Length() < offsetDataFromY.OffsetVector.Length())
            {
                return offsetDataFromX;
            }
            return offsetDataFromY;

        }


    }
}