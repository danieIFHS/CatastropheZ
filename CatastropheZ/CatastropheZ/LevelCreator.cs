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

namespace CatastropheZ
{
    public class LevelCreator
    {
        public bool active;
        public Tile[,] Grid;
        public int activeX;
        public int activeY;

        public LevelCreator()
        {
            activeX = 0;
            activeY = 0;
            Grid = new Tile[84, 54];
            for (int i = 0; i < Globals.ActiveLevel.TileData.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.ActiveLevel.TileData.GetLength(1); j++)
                {
                    Tile e = new Tile();
                    e.Texture = Globals.Textures["Grass"];
                    e.CollisionType = 1;
                    e.Rect = new Rectangle(i * 20, j * 20, 20, 20);
                    e.color = Color.DarkGray;

                    Grid[i, j] = e;
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < Globals.ActiveLevel.TileData.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.ActiveLevel.TileData.GetLength(1); j++)
                {
                    Grid[i, j].Draw();
                }
            }
            Globals.Batch.Draw(Globals.Textures["Border"], new Rectangle(activeX * 20, activeY * 20, 20, 20), Color.White);

            
        }
    }
}
