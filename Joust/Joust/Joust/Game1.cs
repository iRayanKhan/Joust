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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState oldKB;
        
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

        Texture2D tileSpriteSheet;//Contains the platform
        Rectangle platformSource = new Rectangle(80, 115, 47, 10);

        Texture2D pixel;
        Rectangle Ground = new Rectangle(0, 460, 840, 20);
        Rectangle Platform = new Rectangle(350, 300, 150, 20);
        Rectangle player = new Rectangle(400, 400, 50, 50);
        List<Rectangle> platforms = new List<Rectangle>();

        int gravity = 1;
        int vel = 0;
        int jump;

        bool draw;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            platforms.Add(new Rectangle(0, 100, 100, 50));//Top Left Platform
            platforms.Add(new Rectangle(0, 400, 200, 50));//Middle Left Platform
            platforms.Add(new Rectangle(0, 700, 1600, 50));//Bottom Floor Platform

            platforms.Add(new Rectangle(300, 150, 400, 50));//Middle Column Top Platform
            platforms.Add(new Rectangle(350, 500, 266, 50));//Middle Column Bottom Platform

            platforms.Add(new Rectangle(840, 375, 266, 50));//Jutting to the left of the last column platform

            platforms.Add(new Rectangle(1100, 100, 200, 50));//Left column top platform
            platforms.Add(new Rectangle(1100, 400, 200, 50));//Left column bottom platform
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileSpriteSheet = Content.Load<Texture2D>("tile_sprite_sheet");
            pixel = Content.Load<Texture2D>("pixel"); //A white square, can be used to make solid colors
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            btnPlay = new Joust.cButton(Content.Load<Texture2D>("Button"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(350, 300));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();
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

            bool playerOnPlatform = false;
            foreach(Rectangle platform in platforms)
            {
                Rectangle playerFeet = new Rectangle(player.X, player.Y + 49, 50, 1);
                bool playerAbovePlatform = (player.Y + 45) < platform.Y;
                if(playerFeet.Intersects(platform) && playerAbovePlatform)
                {
                    playerOnPlatform = true;
                    break;
                }
            }
            //gravity
            if(!playerOnPlatform)
                player.Y += 2;

            //jumping
            if (kb.IsKeyDown(Keys.Space) && !oldKB.IsKeyDown(Keys.Space))
            {
                player.Y -= 5;
            }

            base.Update(gameTime);
        }

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

            spriteBatch.Draw(pixel, player, Color.Red);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void drawPlatforms(SpriteBatch sb)
        {
            foreach(Rectangle platform in platforms)
            {
                sb.Draw(tileSpriteSheet, platform, platformSource, Color.White);
            }
        }
    }
}
