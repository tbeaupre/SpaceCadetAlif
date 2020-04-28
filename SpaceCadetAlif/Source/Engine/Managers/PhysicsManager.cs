using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    /*
     * This class updates position, velocity, and acceleration of bodies passed to it.
     * Also handles Collision Detection between CollisionBoxes and environment.
     */

    static class PhysicsManager
    {
        internal const float DEFAULT_GRAVITY_Y = 0.005f;
        internal const float DEFAULT_GRAVITY_X = 0.0f;
        private static Dictionary<GameObject, Vector2> impactResultants = new Dictionary<GameObject, Vector2>(); // list of objects and their new velocities

        public static void Update()
        {
            impactResultants.Clear();

            // Check for collisions and handle them
            UpdateCollisions();

            // Update velocities given by impactResutants for next iteration
            UpdateVelocities();

            // Update positions based on velocity.
            UpdatePositions();

        }

        // Update object positions based on their current velocity.
        private static void UpdatePositions()
        {
            foreach (GameObject obj in WorldManager.ToUpdate)
            {
                obj.Body.UpdatePosition();
            }
        }

        // Corrects clipping and adds velocities to impactResultants upon collision.
        private static void UpdateCollisions()
        {
            foreach (GameObject obj1 in WorldManager.ToUpdate)
            {
                MapCollision(obj1);
                foreach (GameObject obj2 in WorldManager.ToUpdate)
                {
                    if (obj1 != obj2)
                    {
                        if (obj1.Body.CollisionBoxesAbsolute.Any(c1 => obj2.Body.CollisionBoxesAbsolute.Any(c2 => c1.Intersects(c2))))
                        {
                            obj1.OnCollision(new CollisionEventArgs(obj1, obj2));
                            if (obj1.Body.CollisionType == Physics.CollisionType.SOLID && obj2.Body.CollisionType == Physics.CollisionType.SOLID)
                            {
                                ChangeImpactVelocity(obj1, obj2);
                                CorrectClipping(obj1, obj2);
                            }
                        }
                    }
                }
            }
        }




        // Adds impact velocities to the impactResultants dictionary.
        private static void ChangeImpactVelocity(GameObject obj1, GameObject obj2)
        {
            Vector2 u1 = obj1.Body.Velocity;
            float m1 = obj1.Body.Mass;
            float frictionCoefficient = Math.Max(obj1.Body.Friction, obj2.Body.Friction);
            ChangeImpactVelocity(u1, m1, obj2, frictionCoefficient);
        }

        // Stripped down version of ChangeImpactVelocity so that the world can effect the objects.
        private static void ChangeImpactVelocity(Vector2 u1, float m1, GameObject obj2, float frictionCoefficient)
        {
            Vector2 u2 = obj2.Body.Velocity;
            float m2 = obj2.Body.Mass;
            float totalMass = (m1 + m2);
            Vector2 v2 = ((u2 * (m2 - m1)) + (2 * m1 * u1)) / totalMass * frictionCoefficient;
            if (float.IsNaN(v2.X)) v2 = Vector2.Zero;
            AddImpact(obj2, v2);
        }

        // Corrects clipping issues by snapping to edge.
        private static void CorrectClipping(GameObject obj1, GameObject obj2)
        {
            // First determines which object is moving faster to determine who is snapping to who(m).
            GameObject fasterObj, slowerObj;
            if (obj1.Body.Velocity.Length() >= obj2.Body.Velocity.Length())
            {
                fasterObj = obj1;
                slowerObj = obj2;
            }
            else
            {
                fasterObj = obj2;
                slowerObj = obj1;
            }

            // relative velocity between the two
            Vector2 relativeVel = obj1.Body.Velocity - obj2.Body.Velocity;

            foreach (Rectangle rectA in fasterObj.Body.CollisionBoxesAbsolute)
            {
                foreach (Rectangle rectB in slowerObj.Body.CollisionBoxesAbsolute)
                {
                    if (rectA.Intersects(rectB))
                    {
                        // offset the faster object!
                        fasterObj.Body.Position += CalculateOffset(relativeVel, rectA, rectB);
                    }
                }
            }
        }

        // uses a binary search to determine and return the vector to offset a body by for use in clip correction
        private static Vector2 CalculateOffset(Vector2 relativeVel, Rectangle rectA, Rectangle rectB)
        {
            Vector2 offset = Vector2.Zero;
            Point origin = rectA.Location;
            float binarySearchFactor = 0.5f;

            while (true)
            {
                rectA.Location = origin + offset.ToPoint();
                if (rectA.Intersects(rectB))
                {
                    if (offset.Y <= 1 && offset.X <= 1) return offset;
                    offset -= binarySearchFactor * relativeVel; // push the rect back the direction it came by a factor of binarySearchFactor
                }
                else
                {
                    if (offset.Y <= 1 && offset.X <= 1) return offset; // feeling strange about this return statement. Might be nice to exit loop with a more robust toucing check.
                    offset += binarySearchFactor * relativeVel; // push the rect forward in the same direction
                }
                binarySearchFactor *= 0.5f; // redeuce BSF by half every iteration
            }
        }

        // Helper function for adding impacts to the dictionary.
        private static void AddImpact(GameObject obj, Vector2 vel)
        {
            if (impactResultants.ContainsKey(obj))
            {
                impactResultants[obj] = vel;
            }
            else
            {
                impactResultants.Add(obj, vel);
            }
        }

        // Handles map collisions by correcting clipping and adding velocities to impactResultants
        private static void MapCollision(GameObject obj)
        {
            var collisionZone = CollidingRects(obj).OrderBy(o => o.Left);
            if (collisionZone.Count() == 0)
            {
                return;
            }
            var size = new Point(collisionZone.Max(o => o.Left) - collisionZone.Min(o => o.Left), collisionZone.Max(o => o.Top) - collisionZone.Min(o => o.Top));
            var fullSurface = new Rectangle(collisionZone.First().Location, size);
          
        }

        private static IEnumerable<Rectangle> CollidingRects(GameObject obj) {
            foreach (Rectangle rect in obj.Body.CollisionBoxesAbsolute)
            {
                Rectangle roomSpan = WorldManager.CurrentRoom.GetCollision().Bounds;// outline of the room

                //to prevent index out of bounds exception
                int top = Math.Max(rect.Top, roomSpan.Top);
                int bot = Math.Min(rect.Bottom, roomSpan.Bottom);
                int left = Math.Max(rect.Left, roomSpan.Left);
                int right = Math.Min(rect.Right, roomSpan.Right);

                for (int j = top; j < bot; j++)
                {
                    for (int i = left; i < right; i++)
                    {
                        Color currentColor = WorldManager.CurrentRoom.ColorData[i + j * WorldManager.CurrentRoom.GetCollision().Width];
                        if (currentColor.A != 0) // alpha != 0
                        {
                            Rectangle currentPixel = new Rectangle(i, j, 1, 1);
                            if (currentPixel.Intersects(rect))
                            {
                                yield return currentPixel;
                            }
                        }
                    }
                }
            }
        }


        // Changes velocities of bodies based on the resultant table and adds accelerations to bodies
        private static void UpdateVelocities()
        {
            foreach (GameObject obj in impactResultants.Keys)
            {
                obj.Body.Velocity = impactResultants[obj];
            }

            foreach(GameObject obj in WorldManager.ToUpdate)
            {
               if (impactResultants.Keys.Contains(obj)) continue;
               obj.Body.UpdateVelocity(); // Adds any accelerations within object instance
            }
        }
    }
}