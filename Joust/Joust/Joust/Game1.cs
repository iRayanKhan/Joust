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
            this.Window.AllowUserResizing = true;
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
            KeyboardState kb = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
          
           
      
           if (kb.IsKeyDown(Keys.Escape) && !oldkb.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
               
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        
        protected override void Draw(GameTime gameTime)
            
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            // TODO: Add your drawing code here
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
    }
}
