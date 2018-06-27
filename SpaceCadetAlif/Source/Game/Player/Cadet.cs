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

        public Armory Armory { get; }
        private LoopingSprite mBodySprite;
        private ManualSprite mGunSprite;


        public Cadet(Vector2 position)
            : base(
                  new List<Sprite> {
                    new LoopingSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Spaceman", 13, 2), 0),
                    new ManualSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Guns/Guns", 5, 2)) },
                  new List<Rectangle>() { new Rectangle(0, 3, 11, 15) },
                  position)
        {
            Armory = new Armory();
            mBodySprite = (LoopingSprite)Sprites[0];
            mGunSprite = (ManualSprite)Sprites[1];

            InputListener += _OnInput;
        }

        private void _OnInput(InputEventArgs e)
        {
            switch (e.Input)
            {
                case Public.Input.Left:
                    if (e.Value > 0)
                    {
                        if (Body.Velocity.X > -TopSpeed)
                        {
                            Body.Acceleration = new Vector2(-0.2f, 0);
                        }
                        else
                        {
                            Body.Acceleration = Vector2.Zero;
                        }
                    }
                    else
                    {
                        Body.Acceleration = Vector2.Zero;
                    }

                    break;
                case Public.Input.Right:
                    if (e.Value > 0)
                    {
                        if (Body.Velocity.X < TopSpeed)
                        {
                            Body.Acceleration = new Vector2(0.2f, 0);
                        }
                        else
                        {
                            Body.Acceleration = Vector2.Zero;
                        }
                    }
                    else
                    {
                        Body.Acceleration = Vector2.Zero;
                    }

                    break;

                case Public.Input.Jump:
                    if (e.Value > 0)
                    {
                        Body.Acceleration = new Vector2(0, -Body.Gravity.Y * 2);
                    }
                    else
                    {
                        Body.Acceleration = Vector2.Zero;
                    }
                    break;
                case Public.Input.Up:
                    if (e.Value == 0)
                    {
                        mBodySprite.CurrentY = 0;
                        mGunSprite.CurrentY = 0;
                    }
                    else
                    {
                        mBodySprite.CurrentY = 1;
                        mGunSprite.CurrentY = 1;
                    }
                    break;
                case Public.Input.Attack:
                    if (e.Value == 0.5f)
                    {
                        ShootGun();
                    }
                    break;
                case Public.Input.ChangeWeapons:
                    if (e.Value == 0.5f)
                    {
                        NextGun();
                    }
                    break;
            }
        }

        private void NextGun()
        {
            mGunSprite.SetFrame(Armory.NextGun(), mGunSprite.CurrentY);
        }

        private void ShootGun()
        {
            new Bullet(Body.Position, Armory.GetCurrentGun(), mBodySprite.CurrentY == 1);
        }
    }
}
