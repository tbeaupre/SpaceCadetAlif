
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Objects;
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
        internal const float DEFAULT_GRAVITY_Y = 0.05f;
        internal const float DEFAULT_GRAVITY_X = 0.00f;
        private static Dictionary<GameObject, Vector2> impactResultants = new Dictionary<GameObject, Vector2>(); // list of objects and their new velocities

        public static void Update(Room CurrentRoom)
        {
            impactResultants.Clear();

            // Update positions based on velocity.
            UpdatePositions();

            // Check for clipping and handle it.
            ClipCorrection();

            // Update velocities given by impactResutants for next iteration
            UpdateVelocities();

        }

        // Update object positions based on their current velocity.
        private static void UpdatePositions()
        {
            foreach (GameObject obj in WorldManager.ToUpdate)
            {
                obj.Body.Position += obj.Body.Velocity;
            }
        }

        // Correct objects that are clipping into one another.
        private static void ClipCorrection()
        {
            foreach (GameObject obj in WorldManager.ToUpdate)
            {
                foreach (GameObject otherobj in WorldManager.ToUpdate)
                {
                    if (obj != otherobj)
                    {
                        if (ClipCheck(obj, otherobj))
                        {
                            ChangeImpactVelocity(obj, otherobj);
                            //CorrectClipping(obj, otherobj);
                        }
                    }
                }
            }

            foreach (GameObject obj in WorldManager.ToUpdate)
            {
                // Handle map collisions last.
                if (MapCollision(obj))
                {
                    ChangeImpactVelocity(Vector2.Zero, float.MaxValue, obj);
                }

                UpdateVelocity(obj);
            }
        }

        private static bool ClipCheck(GameObject obj1, GameObject obj2)
        {
            foreach (Rectangle collisionBox1 in obj1.Body.CollisionBoxes)
            {
                foreach (Rectangle collisionBox2 in obj2.Body.CollisionBoxes)
                {
                    if (collisionBox1.Intersects(collisionBox2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // Adds impact velocities to the impactResultants dictionary.
        private static void ChangeImpactVelocity(GameObject obj1, GameObject obj2)
        {
            Vector2 u1 = obj1.Body.Velocity;
            float m1 = obj1.Body.Mass;

            ChangeImpactVelocity(u1, m1, obj2);
        }

        // Stripped down version of ChangeImpactVelocity so that the world can effect the objects.
        private static void ChangeImpactVelocity(Vector2 u1, float m1, GameObject obj2)
        {
            Vector2 u2 = obj2.Body.Velocity;
            float m2 = obj2.Body.Mass;

            Vector2 v2 = ((u2 * (m2 - m1)) + (2 * m1 * u1)) / (m1 + m2);
            AddImpact(obj2, v2);
        }

        // Corrects clipping issues by snapping to edge.
        private static void CorrectClipping(GameObject obj1, GameObject obj2)
        {
            // First determines which object is moving faster to determine who is snapping to whom.
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


        // Update velocities based on impact resultants and gravity.
        private static void UpdateVelocity(GameObject obj1)
        {
            if (impactResultants.ContainsKey(obj1))
            {
                obj1.Body.Velocity = impactResultants[obj1];
            }

            obj1.Body.UpdateVelocity(); // Adds gravity
        }

        // Helper function for adding impacts to the dictionary.
        private static void AddImpact(GameObject obj, Vector2 vel)
        {
            if (impactResultants.ContainsKey(obj))
            {
                impactResultants[obj] += vel;
            }
            else
            {
                impactResultants.Add(obj, vel);
            }
        }

        private static bool MapCollision(GameObject obj)
        {
            return false;
        }

        // Changes velocities of bodies based on the resultant table and adds accelerations to bodies
        private static void UpdateVelocities()
        {
            foreach (GameObject obj in impactResultants.Keys)
            {
                obj.Body.Velocity = impactResultants[obj];
            }
        }
    }
}