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
        private static readonly int[] STAND_ANIM = { 1 };
        private static readonly int[] DASH_ANIM = { 2 };
        private static readonly int[] RUN_ANIM = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

        public Armory Armory { get; }
        private AnimatedSprite mBodySprite;
        private ManualSprite mGunSprite;


        public Cadet(Vector2 position)
            : base(
                  new List<Sprite> {
                    new AnimatedSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Spaceman", 13, 2), false, true, STAND_ANIM, 0),
                    new ManualSprite(ResourceManager.LoadSpriteData("Actor/Ally/Player/Guns/Guns", 5, 2)) },
                  new List<Rectangle>() { new Rectangle(0, 3, 11, 15) },
                  position)
        {
            Armory = new Armory();
            mBodySprite = (AnimatedSprite)Sprites[0];
            mGunSprite = (ManualSprite)Sprites[1];

            InputListener += _OnInput;
        }

        private void _OnInput(InputEventArgs e)
        {
            switch (e.Input)
            {
                case Public.Input.Left:
                    RunLeft(e.Value);
                    break;
                case Public.Input.Right:
                    RunRight(e.Value);
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

        // Resets the velocity in the X direction to 0 and changes to the standing animation.
        private void ResetHorizontalVelocity()
        {
            Body.Acceleration = new Vector2(0, Body.Acceleration.Y);
            mBodySprite.SetAnimation(STAND_ANIM, false);
        }

        private void RunLeft(float input)
        {
            Run(input, -1);
            Mirrored = true;
        }

        private void RunRight(float input)
        {
            Run(input, 1);
            Mirrored = false;
        }

        private void Run(float input, int xVel)
        {
            if (input == 0.5f)
            {
                mBodySprite.SetAnimation(DASH_ANIM, false);
                Body.Acceleration = new Vector2(xVel * 0.2f, Body.Acceleration.Y);
            }
            else if (input == 1)
            {
                mBodySprite.SetAnimation(RUN_ANIM, true);
            }
            else
            {
                ResetHorizontalVelocity();
            }
        }

        private void NextGun()
        {
            mGunSprite.SetFrame(Armory.NextGun(), mGunSprite.CurrentY);
        }

        private void ShootGun()
        {
            new Bullet(Body.Position, Armory.GetCurrentGun(), mBodySprite.CurrentY == 1, Mirrored);
        }
    }
}
