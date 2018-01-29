using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceCadetAlif.Source.Engine.Graphics.Sprites;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Events;
using SpaceCadetAlif.Source.Engine;

namespace SpaceCadetAlif.Source.Game
{
    class Cadet : Actor
    {
        public Cadet(Vector2 position)
            : base(
                  new List<Sprite> { new LoopingSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Spaceman", 13, 2), 0) },
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
                case Public.Input.Up:
                    if (e.Value == 0)
                    {
                        Sprites[0].CurrentY = 0;
                    }
                    else
                    {
                        Sprites[0].CurrentY = 1;
                    }
                    break;
            }
        }
    }
}
