﻿using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics.Sprites;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Events;

namespace SpaceCadetAlif.Source.Game
{
    class Cadet : Actor
    {
        public Cadet(Vector2 position)
            : base(new LoopingSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Spaceman Body", 13), 0),
                  new List<Rectangle>() { new Rectangle(0, 0, 11, 15) },
                  position)
        {
            InputListener += _OnInput;
        }

        private void _OnInput(InputEventArgs e)
        {
            switch (e.Input)
            {
                case Public.Input.Left:
                    Body.Velocity = new Vector2(-1, 0);
                    break;
                case Public.Input.Right:
                    Body.Velocity = new Vector2(1, 0);
                    break;
            }
        }
    }
}
