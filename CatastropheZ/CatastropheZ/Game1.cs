using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace CatastropheZ
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int Timer;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
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

            Globals.Textures = new Dictionary<string, Texture2D>();
            Globals.Batch = spriteBatch;
            Globals.Projectiles = new List<Projectile>();
            Globals.gameTime = null;
            Globals.Players = new List<Player>();

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

            // TODO: use this.Content to load your game content here
            Globals.Textures["Placeholder"] = this.Content.Load<Texture2D>("images");
            Globals.Textures["Cat1"] = this.Content.Load<Texture2D>("Cat");
            Globals.Textures["Border"] = this.Content.Load<Texture2D>("Border");
            Globals.Font = this.Content.Load<SpriteFont>("Font");
            Globals.FontBig = this.Content.Load<SpriteFont>("Font2");

            string[] Tiles = Directory.GetFiles("Content\\Sprites\\Tiles");
            for (int i = 0; i < Tiles.Count(); i++)
            {
                string sub = Tiles[i].Substring(8, Tiles[i].Length - 12);
                Globals.Textures[sub.Substring(14)] = this.Content.Load<Texture2D>(sub);
            }

            string[] plrTexts = Directory.GetFiles("Content\\Sprites\\Players");
            for (int i = 0; i < plrTexts.Count(); i++)
            {
                string sub = plrTexts[i].Substring(8, plrTexts[i].Length - 12);
                Globals.Textures[sub.Substring(16)] = this.Content.Load<Texture2D>(sub);
            }

            foreach (KeyValuePair<string, Texture2D> entry in Globals.Textures)
            {
                Console.WriteLine(entry.Key);
            }

            int plrCount = 1;
            for (PlayerIndex i = PlayerIndex.One; i <= PlayerIndex.Four; i++)
            {
                GamePadState state = GamePad.GetState(i);
                if (state.IsConnected)
                {
                    Player e = new Player(i, new Rectangle(600, 30, 30, 30), Globals.Textures["Placeholder"]);
                    Globals.Players.Add(e);
                    plrCount++;
                }
            }

            Globals.ActiveLevel = new Level("TestLevel");
            Globals.InGame = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            //test
            //test 2
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
            Globals.gameTime = gameTime;

            if (Globals.InGame && !Globals.ActiveLevel.isBeaten)
            {
                Globals.ActiveLevel.Update();
                foreach (Player player in Globals.Players)
                {
                    player.Update();
                }
                foreach (Zombie zombie in Globals.ActiveLevel.Zombies)
                {
                    zombie.Update();
                }
                foreach (Projectile proj in Globals.Projectiles.ToList())
                {
                    proj.Update();
                }
            }
            else if (Globals.ActiveLevel.isBeaten)
            {
                foreach (Projectile proj in Globals.Projectiles.ToList())
                {
                    proj.Update();
                }

            }

            Timer++;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            Globals.Batch = spriteBatch;

            if (Globals.InGame)
            {
                drawMainGame();
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void drawMainGame()
        {
            for (int i = 0; i < Globals.ActiveLevel.TileData.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.ActiveLevel.TileData.GetLength(1); j++)
                {
                    Globals.ActiveLevel.TileData[i, j].Draw();
                }
            }

            //for (int i = 0; i < Globals.ActiveLevel.PathfindingData.GetLength(0); i++)
            //{
            //    for (int j = 0; j < Globals.ActiveLevel.PathfindingData.GetLength(1); j++)
            //    {
            //        Globals.ActiveLevel.PathfindingData[i, j].Draw();
            //    }
            //}

            for (int i = 0; i < Globals.Players.Count; i++)
            {
                Globals.Players[i].Draw(i);
            }

            foreach (Zombie zombie in Globals.ActiveLevel.Zombies)
            {
                zombie.Draw();
            }

            foreach (Projectile proj in Globals.Projectiles.ToList<Projectile>())
            {
                proj.Draw();
            }

            //Match info
            spriteBatch.Draw(Globals.Textures["Cat1"], new Rectangle(1680, 180, 240, 420), Color.White); // silly little guy

            spriteBatch.Draw(Globals.Textures["Placeholder"], new Rectangle(1680, 0, 240, 180), Color.Black);
            spriteBatch.Draw(Globals.Textures["Placeholder"], new Rectangle(1685, 5, 230, 170), Color.Gray);
            spriteBatch.DrawString(Globals.FontBig, "Wave - " + Globals.ActiveLevel.currentWave.ToString(), new Vector2(1747, 20), Color.White);
            spriteBatch.DrawString(Globals.FontBig, "Cure HP", new Vector2(1753, 100), Color.White);
            spriteBatch.Draw(Globals.Textures["Placeholder"], new Rectangle(1690, 150, (int)(1.1 * Globals.ActiveLevel.cureHP), 20), Color.Red);
        }
    }
}
