using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
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
        private Sprite mSprite;   // The sprite to be drawn.
        private int mMaxHealth;   // The max health of the object. Defaults to 1.
        private int mHealth;      // The current health of the object. Defaults to 1.

        // EventHandlers
        public event InputEventHandler KeyPressListener;
        public delegate void InputEventHandler(InputEventArgs e);
        public virtual void OnKeyPress(InputEventArgs e) { KeyPressListener?.Invoke(e); }

        public event DeathEventHandler DeathListener;
        public delegate void DeathEventHandler(DeathEventArgs e);
        public virtual void OnDeath(DeathEventArgs e) { DeathListener?.Invoke(e); }

        public event CollisionEventHandler CollisionListener;
        public delegate void CollisionEventHandler(CollisionEventArgs e);
        public virtual void OnCollision(CollisionEventArgs e) { CollisionListener?.Invoke(e); }


        protected GameObject(Sprite sprite, List<Rectangle> collisionBoxes, Vector2 position, int health = 1)
        {
            Body = new Body(collisionBoxes, position);
            mSprite = sprite;
            mMaxHealth = health;
            mHealth = health;
        }

        // Called once per game loop.
        public void Update()
        {
            mSprite.Update();
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
                OnDeath(new DeathEventArgs(source));
            }
        }
    }
}
