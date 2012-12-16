using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MazeMaster.Assets;

namespace MazeMaster
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MazeMaster : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;



        Screen CurrentScreen;
        public const int ScreenMultiplier = 2;
        public static int TileSize = 32 * ScreenMultiplier;
        public static int WallSize = 4 * ScreenMultiplier;
        
        public MazeMaster()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 600 * ScreenMultiplier;
            graphics.PreferredBackBufferHeight = 384 * ScreenMultiplier;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GraphicsAssets.Load(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            if (CurrentScreen == null)
            {
                CurrentScreen = new MainScreen(this);
            }
            CurrentScreen.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (CurrentScreen != null)
            {
                CurrentScreen.Draw(spriteBatch, gameTime);
            }
        }

        internal void StartGame()
        {
            GameScreen cs = new GameScreen(this);
            cs.RestartLevel();
            CurrentScreen = cs;
        }

        internal void ShowTutorial()
        {
            TutorialScreen cs = new TutorialScreen(this);
            CurrentScreen = cs;
        }

        internal void ShowMain()
        {
            CurrentScreen = new MainScreen(this);
        }
    }
}
