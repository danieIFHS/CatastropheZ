﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Text;
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
        public bool saving = false;

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
                    e.character = '.';

                    Grid[i, j] = e;
                }
            }

        }

        public void Update(char Button)
        {
            switch (Button)
            {
                case 'A':
                    Grid[activeX, activeY].Texture = Globals.Textures["Stone"];
                    Grid[activeX, activeY].character = 'W';
                    break;
                case 'B':
                    Grid[activeX, activeY].Texture = Globals.Textures["Grass"];
                    Grid[activeX, activeY].character = '.';
                    break;
                case 'X':
                    Grid[activeX, activeY].Texture = Globals.Textures["Floor"];
                    Grid[activeX, activeY].character = 'F';
                    break;
                case 'Y':
                    for (int i = 0; i < Globals.ActiveLevel.TileData.GetLength(0); i++)
                    {
                        for (int j = 0; j < Globals.ActiveLevel.TileData.GetLength(1); j++)
                        {
                            if (Grid[i, j].character == 'C')
                            {
                                Grid[i, j].Texture = Globals.Textures["Grass"];
                                Grid[i, j].character = '.';
                            }
                        }
                    }
                    Grid[activeX, activeY].Texture = Globals.Textures["Cure"];
                    Grid[activeX, activeY].character = 'C';
                    break;
                case 'S':
                    if (!saving)
                    {
                        Save();
                    }
                    break;
                default:
                    break;
            }
        }

        public void Save()
        {
            //saving = true;
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var path = Path.Combine(appDataPath, @"CatastropheZ\");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DateTime date = DateTime.UtcNow;
            string fileName = date.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
            string folderPath = Path.Combine(appDataPath, "CatastropheZ");
            path = Path.Combine(folderPath, fileName);
            try
            {
                FileStream fs = File.Create(path);
                fs.Close();
                string[] lines = File.ReadAllLines(path);
                using (StreamWriter sw = new StreamWriter(path))
                {
                    for (int x = 0; x < Globals.ActiveLevel.TileData.GetLength(1); x++)
                    {
                        string stack = "";
                        for (int y = 0; y < Globals.ActiveLevel.TileData.GetLength(0); y++)
                        {
                            stack += Grid[y, x].character; 
                        }
                        Console.WriteLine(stack);
                        sw.WriteLine(stack);
                    }
                    sw.WriteLine("10,200,10,75");
                }

                //using (StreamReader sr = File.OpenText(path))
                //{
                //    string s = "";
                //    while ((s = sr.ReadLine()) != null)
                //    {
                //        Console.WriteLine(s);
                //    }
                //}
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

            Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1680, 0, 240, 1080), Color.DimGray);

            Globals.Batch.Draw(Globals.Textures["AButton"], new Rectangle(1680, 40, 50, 50), Color.White);
            Globals.Batch.DrawString(Globals.FontBig, "Place Wall", new Vector2(1750, 50), Color.White);

            Globals.Batch.Draw(Globals.Textures["BButton"], new Rectangle(1680, 110, 50, 50), Color.White);
            Globals.Batch.DrawString(Globals.FontBig, "Place Grass", new Vector2(1750, 120), Color.White);

            Globals.Batch.Draw(Globals.Textures["XButton"], new Rectangle(1680, 180, 50, 50), Color.White);
            Globals.Batch.DrawString(Globals.FontBig, "Place Floor", new Vector2(1750, 190), Color.White);

            Globals.Batch.Draw(Globals.Textures["YButton"], new Rectangle(1680, 250, 50, 50), Color.White);
            Globals.Batch.DrawString(Globals.FontBig, "Place Cure", new Vector2(1750, 260), Color.White);

        }
    }
}
