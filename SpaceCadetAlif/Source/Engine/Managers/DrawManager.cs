using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Graphics;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class DrawManager
    {
        private static SpriteBatch spriteBatch;
        private static List<DrawTextureData> texturesToDraw = new List<DrawTextureData>();
        private static List<DrawSpriteData> spritesToDraw = new List<DrawSpriteData>();

        // Initialize the DrawManager with the game's SpriteBatch.
        public static void InitDrawManager(SpriteBatch newSpriteBatch)
        {
            spriteBatch = newSpriteBatch;
        }

        // Add a texture to the list of textures to draw when the draw method is called by the game loop.
        public static void DrawTexture(string tag, float x, float y, float layer=0)
        {
            texturesToDraw.Add(new DrawTextureData(tag, (int)x, (int)y, layer));
        }

        // Add a sprite to the list of sprites to draw when the draw method is called by the game loop.
        public static void DrawSprite(Sprite sprite, float x, float y, float layer=0)
        {
            spritesToDraw.Add(new DrawSpriteData(sprite, (int)x, (int)y, layer));
        }

        // Called by the game loop to draw every texture and sprite.
        public static void Draw()
        {
            spriteBatch.Begin();
            foreach(DrawTextureData data in texturesToDraw)
            {
                DrawTexture(ref spriteBatch, data);
            }
            foreach (DrawSpriteData data in spritesToDraw)
            {
                DrawSprite(ref spriteBatch, data);
            }
            spriteBatch.End();
        }

        // Actually draws the texture.
        private static void DrawTexture(ref SpriteBatch spriteBatch, DrawTextureData data)
        {
            spriteBatch.Draw(ResourceManager.GetTexture(data.Tag),
                new Vector2(data.X, data.Y),
                Color.White);
        }

        // Actually draws the sprite.
        private static void DrawSprite(ref SpriteBatch spriteBatch, DrawSpriteData data)
        {
            spriteBatch.Draw(ResourceManager.GetTexture(data.Sprite.Data.TextureTag),
                data.Sprite.GetSourceRect(),
                data.Sprite.GetDestRect(data.X, data.Y),
                Color.White,
                0,
                new Vector2(0, 0),
                SpriteEffects.None,
                data.Layer);
        }
    }
}
