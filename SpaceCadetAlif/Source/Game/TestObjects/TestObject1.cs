using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics.Sprites;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Events;
using System;


namespace SpaceCadetAlif.Source.Engine.TestObjects
{
    class TestObject1 : Prop
    {
        public TestObject1(Vector2 position)
            : base(new ManualSprite(ResourceManager.LoadSpriteData("Prop/Consumable/Battery", 4), 0),
                  new List<Rectangle>() { new Rectangle(0, 0, 5, 8) },
                  position, false, false, false)
        {
        }

      
    }
}
