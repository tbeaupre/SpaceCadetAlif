using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpaceCadetAlif.Source.Engine.Graphics;
using System;
using System.Collections.Generic;

namespace SpaceCadetAlif.Source.Engine.Managers
{
    static class ResourceManager
    {
        private const int DEFAULT_SLOWDOWN = 5;
        private static ContentManager content;
        private static Dictionary<string, Guid> guids = new Dictionary<string, Guid>();
        private static Dictionary<Guid, Texture2D> textures;
        private static Dictionary<Guid, SpriteData> sprites;

        // Initialize the ResourceManager with the game's ContentManager.
        public static void Init(ContentManager newContent)
        {
            content = newContent;
            textures = new Dictionary<Guid, Texture2D>();
            sprites = new Dictionary<Guid, SpriteData>();
        }

        // Load up a texture and add it to the dictionary.
        public static Guid LoadTexture(string path)
        {
            if (guids.ContainsKey(path))
            {
                return guids[path]; // Texture was already loaded.
            }

            Guid id = Guid.NewGuid();
            guids.Add(path, id);

            Texture2D texture = content.Load<Texture2D>(path);
            textures.Add(id, texture);

            return id;
        }

        // Retrieve the texture associated with given ID.
        public static Texture2D GetTexture(Guid id)
        {
            Texture2D texture = textures[id];
            return texture;
        }

        // Create sprite data and add it to the dictionary.
        public static Guid LoadSpriteData(string path, int numFrames, int slowdown = DEFAULT_SLOWDOWN)
        {
            if (guids.ContainsKey(path))
            {
                return guids[path]; // Texture was already loaded.
            }

            SpriteData sprite = new SpriteData(path, numFrames, slowdown);
            Guid id = guids[path];
            sprites.Add(id, sprite);

            return id;
        }

        // Retrieve the sprite data associated with given ID.
        public static SpriteData GetSpriteData(Guid spriteID)
        {
            return sprites[spriteID];
        }
    }
}
