
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

        public const float DEFAULT_GRAVITY_Y = 0.01f;
        public const float DEFAULT_GRAVITY_X = 0.0f;

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
                            CollisionEventArgs collision = new CollisionEventArgs(A, B, dPair);
                            A.OnCollision(collision);
                            B.OnCollision(collision);
                        }
                    }
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

            //for some reason, we need to store the data from Texture2D.getCollision() as a *1D* array of colors
            Color[] cList = new Color[currentRoom.GetCollision().Width * currentRoom.GetCollision().Height];
            currentRoom.GetCollision().GetData(cList);
            bool collided = false;
            DirectionPair collisionDirectionPair = new DirectionPair();
            Rectangle closestPixel = Rectangle.Empty;
            Rectangle collidingRect = Rectangle.Empty;

            foreach (Rectangle rect in obj.Body.CollisionBoxes)
            {
                // offset the rectangle to the body's location
                rect.Offset(obj.Body.Position.X, obj.Body.Position.Y);
                // copy the rectangle at its projected destination.
                Rectangle projection;

                projection = new Rectangle(rect.X + (int)Math.Ceiling(velocity.X), rect.Y + (int)Math.Ceiling(velocity.Y), rect.Width, rect.Height);
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
                                obj.Body.Velocity = NextVelocity(obj.Body, rect, currentPixel);
                                if (!collided || Vector2.Distance(rect.Center.ToVector2(), currentPixel.Center.ToVector2()) < Vector2.Distance(rect.Center.ToVector2(), closestPixel.Center.ToVector2()))
                                {
                                    closestPixel = currentPixel;
                                    collidingRect = rect;
                                    collided = true;
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine(collided + " " + obj.Body.Velocity + " " + obj.Body.Position);
            if (collided)
            {
                obj.Body.Velocity += SnapToEdge(collidingRect, closestPixel, velocity);
                CollisionEventArgs collisionEventArgs = new CollisionEventArgs(obj, currentRoom, collisionDirectionPair);
                obj.OnCollision(collisionEventArgs);
            }
        }

        private static Vector2 SnapToEdge(Rectangle collider, Rectangle stationaryRect, Vector2 velocity)
        {
            Direction leadingEdge = PhysicsUtilities.GetDirectionFromVector(stationaryRect.Center.ToVector2() - collider.Center.ToVector2());
            float xOffset = 0, yOffset = 0;
            switch (leadingEdge)
            {
                case Direction.NONE:
                    break;
                case Direction.UP:
                    if (velocity.Y <= 0) break;
                    yOffset = stationaryRect.Bottom - collider.Top + 1;
                    xOffset = (velocity.X / velocity.Y) * yOffset;
                    break;
                case Direction.DOWN:
                    if (velocity.Y >= 0) break;
                    yOffset = stationaryRect.Top - collider.Bottom - 1;
                    xOffset = (velocity.X / velocity.Y) * yOffset;
                    break;
                case Direction.LEFT:
                    if (velocity.X <= 0) break;
                    xOffset = stationaryRect.Right - collider.Left + 1;
                    yOffset = (velocity.Y / velocity.X) * xOffset;
                    break;
                case Direction.RIGHT:
                    if (velocity.X >= 0) break;
                    xOffset = stationaryRect.Left - collider.Right - 1;
                    yOffset = (velocity.Y / velocity.X) * xOffset;
                    break;
            }
            return new Vector2(xOffset, yOffset);
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
    }
}