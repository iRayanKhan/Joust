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

        Texture2D tileSpriteSheet;//Contains the platform
        Rectangle platformSource = new Rectangle(80, 115, 47, 10);

        Texture2D Pixel;
        Rectangle Ground;
        Rectangle Platform;
        Rectangle Player = new Rectangle(400, 400, 50, 50);
        Rectangle Collider;

        int gravity = 1;
        int vel = 0;
        int jump;
        int y = 300;

        bool draw;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileSpriteSheet = Content.Load<Texture2D>("tile_sprite_sheet");
            Pixel = Content.Load<Texture2D>("Pixel");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kb = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (kb.IsKeyDown(Keys.Space) && !oldKB.IsKeyDown(Keys.Space))
            {
                vel = 15;
                Player.Y -= vel;
                vel += gravity;
                jump ++;
            }

            if (Player.Y+50>=Ground.Y|| Player.Intersects(Collider) && draw)
            {
                vel = 0;
            }
            else
            {
                vel -= gravity;
                Player.Y -= vel;
            }

            if(vel<=0)
            {
                draw = true;
            }
            else { draw = false; }
           

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            Ground = new Rectangle(0, 460, 840, 20);
            Platform = new Rectangle(350, 300, 150, 20);
            Collider = new Rectangle(Platform.X, y, 150, 10);

            spriteBatch.Begin();
            drawPlatforms(spriteBatch);

            spriteBatch.Draw(Pixel, Ground, Color.Black);
            spriteBatch.Draw(Pixel, Player, Color.Red);
            spriteBatch.Draw(Pixel, Platform, Color.Black);
            if (draw)
            {
                spriteBatch.Draw(Pixel, Collider, Color.White);
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


            foreach(Rectangle platform in platforms)
            {
                sb.Draw(tileSpriteSheet, platform, platformSource, Color.White);
            }
        }
    }
}
