﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Graphics;
using SpaceCadetAlif.Source.Engine.Objects;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class DrawManager
    {
        private static SpriteBatch spriteBatch; // The SpriteBatch object allows textures to be drawn in batches to increase efficiency.
        private static Vector2 screenOffset;    // Offsets everything to make (0, 0) the center of the screen. Makes focusing on an object easier.

        // Initialize the DrawManager with the game's SpriteBatch.
        public static void InitDrawManager(SpriteBatch newSpriteBatch, Vector2 newScreenOffset)
        {
            spriteBatch = newSpriteBatch;
            screenOffset = newScreenOffset;
        }

        // Called by the game loop to draw every texture and sprite.
        public static void Draw(Room room, List<GameObject> toDraw, Vector2 focusOffset)
        {
            spriteBatch.Begin();
            DrawRoom(room, focusOffset);
            foreach (GameObject obj in toDraw)
            {
                DrawSprite(obj.Sprite, obj.Body.Position, obj.DrawLayer);
            }
            spriteBatch.End();
        }

        // Draws a Room's foreground and background.
        private static void DrawRoom(Room room, Vector2 focusOffset)
        {
            DrawTexture(room.BackgroundTag, focusOffset + screenOffset, DrawLayer.Background);
            DrawTexture(room.ForegroundTag, focusOffset + screenOffset, DrawLayer.Foreground);
        }

        // Draws the texture.
        private static void DrawTexture(string tag, Vector2 pos, DrawLayer layer)
        {
            Texture2D texture = ResourceManager.GetTexture(tag);
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
        private static void DrawSprite(Sprite sprite, Vector2 pos, DrawLayer layer)
        {
            spriteBatch.Draw(ResourceManager.GetTexture(sprite.Data.TextureTag),
                sprite.GetSourceRect(),
                sprite.GetDestRect((int)pos.X, (int)pos.Y),
                Color.White,
                0,
                new Vector2(0, 0),
                SpriteEffects.None,
                (float)layer / 10f);
        }
    }
}
