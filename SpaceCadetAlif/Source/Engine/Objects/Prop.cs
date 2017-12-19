using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Prop : GameObject
    {
        public Prop(Sprite sprite, List<Rectangle> collisionBoxes, Vector2 position) : base(sprite, collisionBoxes, position)
        {
        }
    }
}
