using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceCadetAlif.Source.Engine.Objects
{
    class Actor : GameObject
    {
        public Actor(Sprite sprite, List<Rectangle> collisionBoxes, Vector2 position) : base(sprite, collisionBoxes, position)
        {
        }
    }
}
