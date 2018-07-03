using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics;
using SpaceCadetAlif.Source.Engine.Managers;
using System;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    /*
     * The abstract parent class of both Actors and Props.
     * Has basic sprite and health information.
     */

    abstract class DrawnObject : GameObject
    {
        public DrawLayer DrawLayer { get; protected set; } // The layer that the object's sprite is drawn on.
        public bool Mirrored { get; protected set; }       // Whether or not to mirror the object when drawn.
        public List<Sprite> Sprites { get; }               // The sprites to be drawn.
        private int mMaxHealth;                            // The max health of the object. Defaults to 1.
        private int mHealth;                               // The current health of the object. Defaults to 1.

        // EventHandlers
        // Called when a change in health causes the GameObject to die.
        public virtual event EventHandler DeathListener;
        public virtual void OnDeath(object sender, EventArgs e) { DeathListener?.Invoke(this, e); }

        // Called when an AnimatedSprite finishes its animation.
        public event EventHandler AnimationCompleteListener;
        public virtual void OnAnimationComplete(object sender, EventArgs e) { AnimationCompleteListener?.Invoke(this, e); }

        // Called when this object is interacted with.
        public virtual event EventHandler InteractListener;
        public virtual void OnInteract(object sender, EventArgs e) { InteractListener?.Invoke(this, e); }

        
        protected DrawnObject(List<Sprite> sprites, List<Rectangle> collisionBoxes, Vector2 position, Vector2 gravity, int health = 1)
            : base(collisionBoxes, position, gravity)
        {
            Sprites = sprites;
            Mirrored = false;
            mMaxHealth = health;
            mHealth = health;
        }

        // Called once per game loop.
        public override void Update()
        {
            foreach (Sprite sprite in Sprites)
            {
                sprite.Update();
            }
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

        public override void OnCreate()
        {
            WorldManager.ToDraw.Add(this);
            base.OnCreate();
        }

        public override void OnDelete()
        {
            WorldManager.ToDraw.Remove(this);
            base.OnDelete();
        }
    }
}
