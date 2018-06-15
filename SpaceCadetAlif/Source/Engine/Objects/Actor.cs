using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Utilities;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Actor : DrawnObject
    {
        // Called when an input is processed.
        public event InputEventHandler InputListener;
        public delegate void InputEventHandler(InputEventArgs e);
        public virtual void OnInput(InputEventArgs e) { InputListener?.Invoke(e); }
        
        public Actor(List<Sprite> sprites,
            List<Rectangle> collisionBoxes,
            Vector2 position,
            float gravityY = PhysicsUtilities.DEFAULT_GRAVITY_Y,
            float gravityX = PhysicsUtilities.DEFAULT_GRAVITY_X)
            : base(sprites, collisionBoxes, position, new Vector2(gravityX,gravityY))
        {
        }

        public override void OnCreate()
        {
            InputManager.RegisterActor(this);
            base.OnCreate();
        }

        public override void OnDelete()
        {
            InputManager.UnregisterActor(this);
            base.OnDelete();
        }
    }
}
