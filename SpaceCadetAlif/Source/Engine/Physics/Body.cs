using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    /*
     * Contains information about a GameObject to be used by the PhysicsManager.
     */
    class Body
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public CollisionType CollisionType { get; set; } // Determines how collisions with other objects are handled.
        public float Mass { get; set; }
        public Vector2 Gravity { get; set; }
        public List<Rectangle> CollisionBoxes { get; set; }  // The collision boxes associated with this GameObject
        public Body(List<Rectangle> collisionBoxes, Vector2 position, Vector2 gravity, CollisionType collisionType = CollisionType.SOLID)
        {
            CollisionBoxes = collisionBoxes;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            CollisionType = collisionType;
            Gravity = gravity;
        }

        public void UpdateVelocity()
        {
            Velocity += Acceleration + Gravity;
        }

        public void UpdatePosition()
        {
            Position += Velocity;
        }
    }
}