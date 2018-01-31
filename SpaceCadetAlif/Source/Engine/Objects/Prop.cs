using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
using SpaceCadetAlif.Source.Engine.Utilities;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Prop : DrawnObject
    {
        public bool Interactable { get; } // Determines if a prop can be interacted with.
        public bool Destructible { get; } // Determines if a prop can be destroyed.
        public bool Movable { get; }      // Determines if a prop can be moved.
        
        // EventHandlers
        // Called when a change in health causes the GameObject to die.
        public override event EventHandler DeathListener;
        public override void OnDeath(object sender, EventArgs e) { if (Destructible) DeathListener.Invoke(this, e); }

        // Called when this object is interacted with.
        public override event EventHandler InteractListener;
        public override void OnInteract(object sender, EventArgs e) { if (Interactable) InteractListener?.Invoke(this, e); }
        
        public Prop(List<Sprite> sprites,
            List<Rectangle> collisionBoxes,
            Vector2 position,
            bool interact, bool destruct, bool move,
            float gravityY = PhysicsUtilities.DEFAULT_GRAVITY_Y,
            float gravityX = PhysicsUtilities.DEFAULT_GRAVITY_X)
            : base(sprites, collisionBoxes, position, new Vector2(gravityX,gravityY))
        {
            Interactable = interact;
            Destructible = destruct;
            Movable = move;

            if (!Movable)
            {
                Body.CollisionType = Physics.CollisionType.SOLID;
            }
        }
    }
}
