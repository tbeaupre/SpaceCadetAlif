using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Graphics;
using System;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    /*
     * The abstract parent class of both Actors and Props.
     * Has basic physics, sprite, and health information.
     */

    abstract class GameObject
    {
        public Body Body { get; } // The GameObject's physics body.
        public DrawLayer DrawLayer { get; protected set; } // The layer that the GameObject's sprite is drawn on.
        public Sprite Sprite { get; }   // The sprite to be drawn.
        private int mMaxHealth;   // The max health of the object. Defaults to 1.
        private int mHealth;      // The current health of the object. Defaults to 1.

        // EventHandlers
        // Called when an input is processed.
        public event InputEventHandler KeyPressListener;
        public delegate void InputEventHandler(InputEventArgs e);
        public virtual void OnKeyPress(InputEventArgs e) { KeyPressListener?.Invoke(e); }

        // Called when a change in health causes the GameObject to die.
        public event EventHandler DeathListener;
        public virtual void OnDeath(object sender, EventArgs e) { DeathListener?.Invoke(this, e); }

        // Called by the PhysicsManager when this object is part of a collision.
        public event CollisionEventHandler CollisionListener;
        public delegate void CollisionEventHandler(CollisionEventArgs e);
        public virtual void OnCollision(CollisionEventArgs e) { CollisionListener?.Invoke(e); }

        // Called when an AnimatedSprite finishes its animation.
        public event EventHandler AnimationCompleteListener;
        public virtual void OnAnimationComplete(object sender, EventArgs e) { AnimationCompleteListener?.Invoke(this, e); }


        protected GameObject(Sprite sprite, List<Rectangle> collisionBoxes, Vector2 position, int health = 1)
        {
            Body = new Body(collisionBoxes, position);
            Sprite = sprite;
            mMaxHealth = health;
            mHealth = health;
        }

        // Called once per game loop.
        public void Update()
        {
            Sprite.Update();
        }

        // Handles increasing or decreasing current life.
        public void ChangeLife(object source, int delta)
        {
            mHealth += delta;

            if (mHealth > mMaxHealth)
            {
                mHealth = mMaxHealth;
            }
            else if (mHealth <= 0)
            {
                OnDeath(source, EventArgs.Empty);
            }
        }
    }
}
