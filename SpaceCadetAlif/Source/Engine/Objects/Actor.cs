using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine.Managers;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Actor : DrawnObject
    {
        // Called when an input is processed.
        public event InputEventHandler InputListener;
        public delegate void InputEventHandler(InputEventArgs e);
        public virtual void OnInput(InputEventArgs e) { InputListener?.Invoke(e); }

        public Actor(Sprite sprite, List<Rectangle> collisionBoxes, Vector2 position) : base(sprite, collisionBoxes, position)
        {
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
