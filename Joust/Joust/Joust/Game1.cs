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

namespace Joust
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldkb;

        Texture2D tileSpriteSheet;//Contains the platform
        Rectangle platformSource = new Rectangle(80, 115, 47, 10);
        bool btn = false;
        enum GameState
        {
            MainMenu,
            Options,
            Playing,
        }
        GameState CurrentGameState = GameState.MainMenu;
        cButton btnPlay;
        int screenWidth = 800, screenHeight = 600;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            graphics.IsFullScreen = true;
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
            tileSpriteSheet = Content.Load<Texture2D>("tile_sprite_sheet");
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            btnPlay = new Joust.cButton(Content.Load<Texture2D>("Button"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(350, 300));
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
            MouseState mouse = Mouse.GetState();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    break;
                case GameState.Playing:
                    break;
            }
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            drawPlatforms(spriteBatch);
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Main Menu"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    btnPlay.Draw(spriteBatch);

                    bool btn = true;
                    break;
                case GameState.Playing:
                    break;
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected void drawPlatforms(SpriteBatch sb)
        {
            List<Rectangle> platforms = new List<Rectangle>();
            platforms.Add(new Rectangle(0, 100, 100, 50));//Top Left Platform
            platforms.Add(new Rectangle(0, 400, 200, 50));//Middle Left Platform
            platforms.Add(new Rectangle(0, 700, 1600, 50));//Bottom Floor Platform

            platforms.Add(new Rectangle(300, 150, 400, 50));//Middle Column Top Platform
            platforms.Add(new Rectangle(350, 500, 266, 50));//Middle Column Bottom Platform

            platforms.Add(new Rectangle(840, 375, 266, 50));//Jutting to the left of the last column platform

            platforms.Add(new Rectangle(1100, 100, 200, 50));//Left column top platform
            platforms.Add(new Rectangle(1100, 400, 200, 50));//Left column bottom platform


            foreach (Rectangle platform in platforms)
            {
                sb.Draw(tileSpriteSheet, platform, platformSource, Color.White);
            }
        }
    }
}