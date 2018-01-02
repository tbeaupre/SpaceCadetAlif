
﻿using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Utilities;
using System;
using System.Collections.Generic;
using SpaceCadetAlif.Source.Engine.Physics;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    /*
     * This class updates position, velocity, and acceleration of bodies passed to it.
     * Also handles Collision Detection between hitboxes and environment.
     */

    static class PhysicsManager
    {
        private static Room environment;

        // Sets the current environment
        public static void InitEnvironment(Room e)
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
            bool collided = false;

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

                OffsetAndDirectionData offset = new OffsetAndDirectionData();

                // loop through all collision boxes in B
                foreach (Rectangle bRect in B.Body.CollisionBoxes)
                {
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

                    Vector2 upperCriticalPointB;
                    Vector2 lowerCriticalPointB;

                    if (relativeSlope >= 0)
                    {
                        upperCriticalPointB = PhysicsUtilities.TopLeft(bRect);
                        lowerCriticalPointB = PhysicsUtilities.BottomRight(bRect);
                    }
                    else
                    {
                        lowerCriticalPointB = PhysicsUtilities.BottomLeft(bRect);
                        upperCriticalPointB = PhysicsUtilities.TopRight(bRect);
                    }

                    float angleFAB = PhysicsUtilities.GetAngle(upperCriticalPoint + relativeVel, upperCriticalPoint, lowerCriticalPoint);
                    float angleGBA = PhysicsUtilities.GetAngle(lowerCriticalPoint + relativeVel, lowerCriticalPoint, upperCriticalPoint);
                    float angleCAB = PhysicsUtilities.GetAngle(upperCriticalPointB, upperCriticalPoint, lowerCriticalPoint);
                    float angleDAB = PhysicsUtilities.GetAngle(lowerCriticalPointB, upperCriticalPoint, lowerCriticalPoint);
                    float angleCBA = PhysicsUtilities.GetAngle(upperCriticalPointB, lowerCriticalPoint, upperCriticalPoint);

                    if (angleFAB <= angleCAB)
                    {
                        if (angleFAB <= angleDAB)
                        {
                            continue;
                        }
                        else
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
                    else
                    {
                        if (angleGBA > angleCBA)
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
                }
                // send collision event out with Direction
                if (collided)
                {
                    //shift A over by the offset if both are solid
                    if (A.Body.CollisionType == CollisionType.SOLID && B.Body.CollisionType == CollisionType.SOLID)
                    {
                        A.Body.Position -= offset.OffsetVector;
                        //placeholder for velocity handling
                        A.Body.Velocity = Vector2.Zero;
                        B.Body.Velocity = Vector2.Zero;
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

        private static void UpdateMotion(Body body)
        {
            body.Position += body.Velocity;
            body.Velocity += body.Acceleration;
        }
    }
}

