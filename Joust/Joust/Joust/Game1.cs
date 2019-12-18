using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Joust
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random random = new Random();

        Texture2D tileSpriteSheet;//Contains the platform
        Rectangle platformSource = new Rectangle(80, 115, 47, 10);
        Texture2D pixel;
        Rectangle Ground = new Rectangle(0, 460, 840, 20);
        Rectangle Platform = new Rectangle(350, 300, 150, 20);
        Rectangle player = new Rectangle(400, 400, 50, 50);
        List<Rectangle> platforms = new List<Rectangle>();


        Enemy[] Enemies = new Enemy[10];
        Vector2[] Spawns = new Vector2[4];
        int maxOnScreen = 4;

        Rectangle EnemyRect;

        enum GameState
        {
            MainMenu,
            Options,
            Playing,
        }

        GameState CurrentGameState = GameState.MainMenu;
        cButton btnPlay;
        int screenWidth = 1400, screenHeight = 1000;


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

            Spawns[0] = (new Vector2(50, 100));
            Spawns[1] = (new Vector2(50, 400));
            Spawns[2] = (new Vector2(1150, 50));
            Spawns[3] = (new Vector2(890, 325));

            int spawnPoint = 0;
            for (int i = 0; i <= 9; i++)
            {
                spawnPoint = random.Next(1, 5);
                if (spawnPoint == 1)
                {
                    Enemies[i] = new Enemy();
                    Enemies[i].setInfo(40, 50, false, 1);
                }
                if (spawnPoint == 2)
                {
                    Enemies[i] = new Enemy();
                    Enemies[i].setInfo(50, 350, false, 2);
                }
                if (spawnPoint == 3)
                {
                    Enemies[i] = new Enemy();
                    Enemies[i].setInfo(1150, 50, false, 3);
                }
                if (spawnPoint == 4)
                {
                    Enemies[i] = new Enemy();
                    Enemies[i].setInfo(890, 325, false, 4);
                }
            }

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
            btnPlay = new cButton(Content.Load<Texture2D>("Button"), graphics.GraphicsDevice);
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

            int screenCount = 0;
            for (int OnScreen = 0; OnScreen <= maxOnScreen; OnScreen++)
            {
                for (int e = 0; e <= 9 && screenCount <= maxOnScreen; e++)
                {
                    if (!Enemies[e].destroyed)
                    {
                        screenCount++;
                        int cX = Enemies[e].x;
                        int cY = Enemies[e].y;
                        bool cV = Enemies[e].visible;
                        int cSpawn = Enemies[e].spawnPoint;

                        if (cSpawn < 3)
                        {
                            Enemies[e].x += (10 * (Enemies[e].flip ? -1 : 1));
                        }

                        if (cSpawn > 2)
                        {
                            Enemies[e].x -= (10 * (Enemies[e].flip ? -1 : 1));
                        }
                    }
                    //screen wrapping
                    if (Enemies[e].x < -50)
                    {
                        Enemies[e].x = screenWidth;
                    }
                    if (Enemies[e].x > screenWidth + 50)
                    {
                        Enemies[e].x = -20;
                    }

                    bool enemyOnPlatform = false;
                    foreach (Rectangle platform in platforms)
                    {
                        Rectangle enemyFeet = new Rectangle(Enemies[e].x, Enemies[e].y + 49, 50, 1);
                        bool playerAbovePlatform = (player.Y + 45) < platform.Y;
                        if (enemyFeet.Intersects(platform) && playerAbovePlatform)
                        {
                            enemyOnPlatform = true;
                            break;
                        }
                    }

                    //gravity
                    if (!enemyOnPlatform)
                        Enemies[e].y += 2;

                    bool jump = random.Next(0, 3) == 1;
                    if (jump)
                        Enemies[e].y -= 5;
                }
            }

            for (int e = 0; e < 4; e++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (e != i)
                    {
                        if (Enemies[e].Intersects(Enemies[i]))
                        {
                            Enemies[e].flip = !Enemies[e].flip;
                            Enemies[i].flip = !Enemies[i].flip;
                            //Console.WriteLine("Collided with Enemy");
                        }
                        if (Enemies[e].Intersects(player))
                        {
                            Enemies[e].flip = !Enemies[e].flip;
                            //Console.WriteLine("Collided with Player");
                        }
                    }
                }
            }

            for (int e = 0; e <= 9 && screenCount <= maxOnScreen; e++)
            {
                for (int i = 0; i <= 9 && screenCount <= maxOnScreen; i++)
                {
                }
            }

            updatePlayer(kb);

            base.Update(gameTime);
        }

        public void updatePlayer(KeyboardState kb)
        {
            bool playerOnPlatform = false;
            foreach (Rectangle platform in platforms)
            {
                Rectangle playerFeet = new Rectangle(player.X, player.Y + 49, 50, 1);
                bool playerAbovePlatform = (player.Y + 45) < platform.Y;
                if (playerFeet.Intersects(platform) && playerAbovePlatform)
                {
                    playerOnPlatform = true;
                    break;
                }
            }
            //gravity
            if (!playerOnPlatform)
                player.Y += 2;

            //jumping
            //Remove oldKB
            if (kb.IsKeyDown(Keys.Space))
            {
                player.Y -= 5;
            }

            //directional movement
            if (kb.IsKeyDown(Keys.Left))
                player.X -= 4;
            if (kb.IsKeyDown(Keys.Right))
                player.X += 4;

            //screen wrapping
            if (player.X < -50)
            {
                player.X = screenWidth;
            }
            if (player.X > screenWidth + 50)
            {
                player.X = -20;
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            drawPlatforms(spriteBatch);
            drawEnemies(spriteBatch);

            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Main Menu"), new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
                    btnPlay.Draw(spriteBatch);

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
            foreach (Rectangle platform in platforms)
            {
                sb.Draw(tileSpriteSheet, platform, platformSource, Color.White);
            }
        }

        protected void drawEnemies(SpriteBatch sb)
        {
            int shipCount = 0;
            for (int shipOnScreen = 0; shipOnScreen <= 1; shipOnScreen++)
            {
                for (int bs = 0; bs <= 9 && shipCount <= maxOnScreen; bs++)
                {
                    if (!Enemies[bs].destroyed)
                    {
                        shipCount++;
                        EnemyRect = new Rectangle(Enemies[bs].x, Enemies[bs].y, 50, 50);
                        spriteBatch.Draw(pixel, EnemyRect, Color.Green);
                    }
                }
            }
        }
    }
}
