using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
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

        // Defines behavior for handling events. By default, reacts to no events.
        public virtual void OnEvent(Event e) { }

        // Defines behavior upon death.
        public virtual void OnDeath()
        {
            // Remove from WorldManager.
            // Play death animation.
        }

        // Handles increasing or decreasing current life.
        public void ChangeLife(int delta)
        {
            mHealth += delta;

            if (mHealth > mMaxHealth)
            {
                mHealth = mMaxHealth;
            }
            else if (mHealth <= 0)
            {
                OnDeath();
            }
        }
    }
}
