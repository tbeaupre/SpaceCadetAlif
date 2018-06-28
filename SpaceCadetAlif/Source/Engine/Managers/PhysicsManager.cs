
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
        private static Dictionary<GameObject, Vector2> impactResultants = new Dictionary<GameObject, Vector2>();

        public static void Update()
        {
            impactResultants.Clear();

            // Update positions based on velocity
            UpdatePositions();

            // Check for clipping
            ClipCorrection();

            // Change velocities of clipped objects

            // Clip correction using snap to edge

            // Change velocity based on forces
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
                            CorrectClipping(obj, otherobj);
                        }
                    }
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

        }

        // Corrects clipping issues by snapping to edge.
        private static void CorrectClipping(GameObject obj1, GameObject obj2)
        {

        }

        // Update velocities based on impact resultants and gravity.
        private static void UpdateVelocity(GameObject obj1)
        {

        }
    }
}