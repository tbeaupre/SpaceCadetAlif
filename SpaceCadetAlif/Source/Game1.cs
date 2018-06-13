using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceCadetAlif.Source.Engine.Managers;
using SpaceCadetAlif.Source.Engine.Objects;
using SpaceCadetAlif.Source.Engine.TestObjects;
using SpaceCadetAlif.Source.Game;
using SpaceCadetAlif.Source.Public;
using System;

namespace SpaceCadetAlif
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = Screen.screenWidth-50;
            graphics.PreferredBackBufferHeight = Screen.screenHeight-50;
            graphics.IsFullScreen = Screen.fullscreen;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ResourceManager.Init(Content);
            DrawManager.Init(graphics.GraphicsDevice, new SpriteBatch(graphics.GraphicsDevice));
            InputManager.Init();
            WorldManager.Init();

            WorldManager.FocusObject = new Cadet(new Vector2(0, 0));
            
            WorldManager.ChangeRoom(new Room("Room/2/Background2", "Room/2/Hitbox2", "Room/2/Hitbox2", 3), new Vector2(800, 300));

            TestObject1 testObject = new TestObject1(new Vector2(780, 300));
            //TestObject1 testObject2 = new TestObject1(new Vector2(730, 300));


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            InputManager.Update();
            WorldManager.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawManager.Draw(WorldManager.CurrentRoom, WorldManager.ToDraw);

            base.Draw(gameTime);
        }
    }
}
