using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Physics;
using System;
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

        public GameObject(List<Rectangle> collisionBoxes, Vector2 position, Vector2 gravity)
        {
            Body = new Body(collisionBoxes, position, gravity);
            OnCreate();
        }

        public virtual void Update() { }

        // Adds the GameObject to all relevant update lists. Called in the constructor.
        public virtual void OnCreate()
        {
            WorldManager.ToUpdate.Add(this);
        }

        // Removes the GameObject from all relevant update lists. Called by the WorldManager.
        public virtual void OnDelete()
        {
            WorldManager.ToUpdate.Remove(this);
        }
    }
}
