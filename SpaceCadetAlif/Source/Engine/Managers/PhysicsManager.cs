

using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.Utilities;

using System;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    /*
     * This class updates position, velocity, and acceleration of bodies passed to it.
     * Also handles Collision Detection between hitboxes and environment.
     */

    static class PhysicsManager
    {
        private static Objects.Environment environment;

        // Sets the current environment
        public static void InitEnvironment(Objects.Environment e)
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
                    if (HandleSimpleCollision(objectList[i], objectList[j]))
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
        private static bool HandleSimpleCollision(GameObject A, GameObject B)
        {

            Body aCurrent = A.Body;
            Body bCurrent = B.Body;

            if (aCurrent.CollisionType == CollisionType.GHOST || bCurrent.CollisionType == CollisionType.GHOST)
            {
                return false;
            }
            
            Body aProjection = PhysicsUtilities.GetProjection(aCurrent);
            Body bProjection = PhysicsUtilities.GetProjection(bCurrent);

            Vector2 relativeVel = aCurrent.Velocity - bCurrent.Velocity;
            float relativeSlope = PhysicsUtilities.SlopeFromVector(relativeVel);

            return false;

        }

        private static void UpdateMotion(Body body)
        {
            body.Position += body.Velocity;
            body.Velocity += body.Acceleration;
        }
    }
}

