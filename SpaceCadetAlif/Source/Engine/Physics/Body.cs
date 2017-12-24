using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Physics
{
    /*
     * Contains information about a GameObject to be used by the PhysicsManager.
     */
    class Body
    {
        public List<Rectangle> CollisionBoxes { get; }   // The collision boxes associated with this GameObject.
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        public CollisionType CollisionType { get; set; } // Determines how collisions with other objects are handled.

        public Body(List<Rectangle> collisionBoxes, Vector2 position, CollisionType collisionType = CollisionType.SOLID)
        {
            CollisionBoxes = collisionBoxes;
            Position = position;
            Velocity = Vector2.Zero;
            Acceleration = Vector2.Zero;
            CollisionType = collisionType;
        }
    }
}
