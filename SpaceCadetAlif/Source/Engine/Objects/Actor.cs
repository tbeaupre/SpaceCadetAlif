using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Utilities;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Actor : DrawnObject
    {
        public const float DEFAULT_TOPSPEED = 0.1f;
        // Called when an input is processed.
        public event InputEventHandler InputListener;
        public delegate void InputEventHandler(InputEventArgs e);
        public virtual void OnInput(InputEventArgs e) { InputListener?.Invoke(e); }

        
        public float TopSpeed { get; set; }

        public Actor(
            Sprite sprite, List<Rectangle> collisionBoxes, 
            Vector2 position, float topSpeed = DEFAULT_TOPSPEED, 
            float gravityY = PhysicsUtilities.DEFAULT_GRAVITY_Y, 
            float gravityX = PhysicsUtilities.DEFAULT_GRAVITY_X) 
            : base(sprite, collisionBoxes, position, new Vector2(gravityX,gravityY))
        {
            TopSpeed = topSpeed;
        }

        public override void OnCreate()
        {
            InputManager.RegisteredActors.Add(this);
            base.OnCreate();
        }

        public override void OnDelete()
        {
            InputManager.RegisteredActors.Remove(this);
            base.OnDelete();
        }
    }
}
