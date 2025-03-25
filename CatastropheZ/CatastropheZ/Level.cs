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
    public class Level
    {
        public Tile[,] TileData;
        public string LevelName;

        public Level(string levelname)
        {
            LevelName = levelname;
            TileData = new Tile[83,96];
            Read();
        }

        private void Read()
        {
            StreamReader Reader = new StreamReader(@"Content\Levels\" + LevelName + ".txt");
            try
            {
                using (Reader)
                {
                    int xCount = 0;
                    int yCount = 0;
                    while (!Reader.EndOfStream)
                    {
                        string Line = Reader.ReadLine();
                        foreach (char c in Line)
                        {
                            Tile e = new Tile();
                            HandleTile(e, c, xCount, yCount);
                            TileData[xCount, yCount] = e;
                            xCount++;
                        }
                        yCount++;
                        xCount = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void HandleTile(Tile _tile, Char _char, int _x, int _y)
        {
            switch (_char)
            {
                case '.':
                    _tile.CollisonType = 1;
                    _tile.Texture = Globals.Textures["Placeholder"]; //1600 total width
                    _tile.Rect = new Rectangle(_x * 20, _y * 20, 20, 20);
                    break;
            }
        }
    }
}
