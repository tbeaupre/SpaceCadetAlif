using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Utilities;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Actor : DrawnObject
    {
        public const float DEFAULT_TOPSPEED = 1;
        // Called when an input is processed.
        public event InputEventHandler InputListener;
        public delegate void InputEventHandler(InputEventArgs e);
        public virtual void OnInput(InputEventArgs e) { InputListener?.Invoke(e); }
<<<<<<< HEAD

        
        public float TopSpeed { get; set; }

        public Actor(
            Sprite sprite, List<Rectangle> collisionBoxes, 
            Vector2 position, float topSpeed = DEFAULT_TOPSPEED, 
            float gravityY = PhysicsManager.DEFAULT_GRAVITY_Y, 
            float gravityX = PhysicsManager.DEFAULT_GRAVITY_X) 
            : base(sprite, collisionBoxes, position, new Vector2(gravityX,gravityY))
=======
        
        public Actor(List<Sprite> sprites,
            List<Rectangle> collisionBoxes,
            Vector2 position,
            float gravityY = PhysicsUtilities.DEFAULT_GRAVITY_Y,
            float gravityX = PhysicsUtilities.DEFAULT_GRAVITY_X)
            : base(sprites, collisionBoxes, position, new Vector2(gravityX,gravityY))
>>>>>>> dev-game
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
