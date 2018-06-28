
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
        public static void Update()
        {
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

        }

        // Correct objects that are clipping into one another.
        private static void ClipCorrection()
        {
            foreach (GameObject obj in listofgameobjects)
            {
                foreach (GameObject otherobj in listofgameobjects)
                {
                    if (obj != otherobj)
                    {
                        if (clipping)
                        {
                            change velocities;
                            clip correct;
                        }
                    }
                }

                change velocity based on forces;
            }
        }
    }
}