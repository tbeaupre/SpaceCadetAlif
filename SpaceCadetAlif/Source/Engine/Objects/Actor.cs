using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Actor : DrawnObject
    {
        public const float DEFAULT_TOPSPEED = 1;
        // Called when an input is processed.
        public event InputEventHandler InputListener;
        public delegate void InputEventHandler(InputEventArgs e);
        public virtual void OnInput(InputEventArgs e) { InputListener?.Invoke(e); }
        public float TopSpeed { get; set; }

        //There may be a problem here, might need a parameter for topSpeed but Not gonna try to fix it now cause there's bigger shrimps to fry
        public Actor(List<Sprite> sprites,
            List<Rectangle> collisionBoxes,
            Vector2 position,
            float gravityY = PhysicsManager.DEFAULT_GRAVITY_Y,
            float gravityX = PhysicsManager.DEFAULT_GRAVITY_X)
            : base(sprites, collisionBoxes, position, new Vector2(gravityX,gravityY))
        {
            TopSpeed = DEFAULT_TOPSPEED;
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
