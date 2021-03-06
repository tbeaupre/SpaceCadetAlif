﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Graphics;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Public;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class DrawManager
    {
        private static GraphicsDevice graphicsDevice; // The GraphicsDevice to be drawn to.
        private static RenderTarget2D lowRes;         // The RenderTarget for the low-res graphics. Necessary for smooth parallax.
        private static SpriteBatch spriteBatch;       // SpriteBatches allow many textures to be drawn with high efficiency.
        private static Vector2 screenOffset;          // Offsets everything to the center of the screen. Makes focusing on an object easier.
        private static Color debugColor = new Color(150, 50, 50, 140); // For collision box debugging. Press 'O' to turn it on

        // Initialize the DrawManager with the game's SpriteBatch.
        public static void Init(GraphicsDevice newGraphicsDevice, SpriteBatch newSpriteBatch)
        {
            graphicsDevice = newGraphicsDevice;
            spriteBatch = newSpriteBatch;
            lowRes = new RenderTarget2D(graphicsDevice, Screen.lowResWidth, Screen.lowResHeight);
            screenOffset = new Vector2(Screen.lowResWidth / 2, Screen.lowResHeight / 2);
        }

        // Called by the game loop to draw every texture and sprite.
        public static void Draw(Room room, List<DrawnObject> toDraw, bool drawDebugRectangles = false)
        {
            Vector2 focusOffset = -WorldManager.GetFocusOffset();

            graphicsDevice.SetRenderTarget(lowRes);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            graphicsDevice.Clear(new Color(0, 0, 0, 0));
              if (room != null)
              {
                  DrawTexture(room.GetForeground(), focusOffset + screenOffset, DrawLayer.Foreground);
              }
              foreach (DrawnObject obj in toDraw)
              {
                  Vector2 pos = obj.Body.Position + focusOffset + screenOffset;
                  bool mirrored = obj.Mirrored;
                  int masterWidth = obj.Sprites[0].Data.FrameWidth;
                  foreach (Sprite sprite in obj.Sprites)
                  {
                      DrawSprite(sprite, pos, obj.DrawLayer, mirrored, masterWidth);
                      if (drawDebugRectangles)
                      {
                          DrawDebugRectangles(pos, obj.Body.CollisonBoxesRelative);
                      }
                  }
              }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
              if (room != null)
              {
                  DrawBackground(room.GetBackground(), (focusOffset / room.ParallaxFactor) + screenOffset, Screen.screenSizeMultiplier);
              }
              spriteBatch.Draw(lowRes, new Rectangle(0, 0, Screen.screenWidth, Screen.screenHeight), Color.White);
            spriteBatch.End();
        }

        private static void DrawDebugRectangles(Vector2 pos, IEnumerable<Rectangle> collisionBoxes)
        {
            foreach(var box in collisionBoxes)
            {
                var texture = new Texture2D(graphicsDevice, box.Width, box.Height);
                Color[] data = new Color[box.Width * box.Height];
                for (int i = 0; i < data.Length; i++) data[i] = Color.White;
                texture.SetData(data);
                Rectangle dest = new Rectangle(pos.ToPoint().X + box.Location.X, pos.ToPoint().Y + box.Location.Y, box.Width, box.Height);
                spriteBatch.Draw(texture, dest, debugColor);
            }
        }

        // Draws the map background at a different size to enable parallax.
        private static void DrawBackground(Texture2D texture, Vector2 pos, int multiplier)
        {
            spriteBatch.Draw(texture,
                new Rectangle((int)(pos.X * multiplier), (int)(pos.Y * multiplier), texture.Bounds.Width * multiplier, texture.Bounds.Height * multiplier),
                null,
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                (float)DrawLayer.Background / 10f);
        }

        // Draws the texture.
        private static void DrawTexture(Texture2D texture, Vector2 pos, DrawLayer layer)
        {
            spriteBatch.Draw(texture,
                new Rectangle((int)pos.X, (int)pos.Y, texture.Bounds.Width, texture.Bounds.Height),
                null, // Textures are not animated, so there is no source rectangle.
                Color.White,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                (float)layer / 10f);
        }

        // Draws the sprite.
        private static void DrawSprite(Sprite sprite, Vector2 pos, DrawLayer layer, bool mirrored, int masterWidth)
        {
            int mirrorOffset = mirrored ? masterWidth - sprite.Data.FrameWidth : 0;
            spriteBatch.Draw(sprite.Data.GetTexture(),
                sprite.GetDestRect((int)pos.X + mirrorOffset, (int)pos.Y),
                sprite.GetSourceRect(),
                Color.White,
                0,
                Vector2.Zero,
                mirrored ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
                (float)layer / 10f);
        }
    }
}
