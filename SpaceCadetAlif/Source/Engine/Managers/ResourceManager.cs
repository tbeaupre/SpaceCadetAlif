using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Graphics;
using System.Collections.Generic;
using System.Diagnostics;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class ResourceManager
    {
        private static ContentManager content;
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SpriteData> sprites = new Dictionary<string, SpriteData>();

        // Initialize the ResourceManager with the game's ContentManager.
        public static void Init(ContentManager newContent)
        {
            content = newContent;
        }

        // Load up a texture and add it to the dictionary. Return -1 if error, 0 otherwise.
        public static bool LoadTexture(string tag, string path)
        {
            if (textures.ContainsKey(tag))
            {
                // Tag is already used.
                Debug.WriteLine("DrawManager::LoadTexture(): Tag " + tag + " is already in use.");
                return false;
            }

            Texture2D texture = content.Load<Texture2D>(path);
            textures.Add(tag, texture);

            return true;
        }

        // Retrieve the texture associated with given tag.
        public static Texture2D GetTexture(string tag)
        {
            Texture2D texture = textures[tag];
            return texture;
        }

        // Create a sprite based and add it to the dictionary. Return -1 if error, 0 otherwise.
        public static bool LoadSprite(string spriteTag, string textureTag, int numFrames, int slowdown)
        {
            if (sprites.ContainsKey(spriteTag))
            {
                // Tag is already used.
                Debug.WriteLine("DrawManager::LoadSprite(): Tag " + spriteTag + " is already in use.");
                return false;
            }

            if (!textures.ContainsKey(textureTag))
            {
                // Given texture has not been loaded yet.
                Debug.WriteLine("DrawManager::LoadSprite(): Texture associated with " + textureTag + " has not been loaded.");
                return false;
            }

            SpriteData sprite = new SpriteData(textureTag, numFrames, slowdown);
            sprites.Add(spriteTag, sprite);

            return true;
        }

        // Retrieve the sprite associated with given tag.
        public static SpriteData GetSprite(string spriteTag)
        {
            SpriteData sprite = sprites[spriteTag];
            return sprite;
        }
    }
}
