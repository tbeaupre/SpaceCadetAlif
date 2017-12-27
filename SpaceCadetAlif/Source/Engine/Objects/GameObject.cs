using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Physics;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    abstract class GameObject
    {
        public Body Body { get; } // The GameObject's physics body.

        // Called by the PhysicsManager when this object is part of a collision.
        public event CollisionEventHandler CollisionListener;
        public delegate void CollisionEventHandler(CollisionEventArgs e);
        public virtual void OnCollision(CollisionEventArgs e) { CollisionListener?.Invoke(e); }

        public GameObject(List<Rectangle> collisionBoxes, Vector2 position)
        {
            Body = new Body(collisionBoxes, position);
            OnCreate();
        }

        public virtual void Update() { }

        public virtual void OnCreate()
        {
            WorldManager.ToUpdate.Add(this);
        }

        public virtual void OnDelete()
        {
            WorldManager.ToUpdate.Remove(this);
        }
    }
}
