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
    class Level
    {
        private Tile[,] TileData;
        public string LevelName;

        public Level(string levelname)
        {
            LevelName = levelname;
            TileData = new Tile[100,50];
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
    }
}
