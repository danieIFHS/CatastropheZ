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
        public Tile[,] PathfindingData;
        public List<Zombie> Zombies;
        public string LevelName;
        public float cureHP;
        public int waves;
        public int currentWave;
        public int zombies;
        public int spawnDelay;
        public int deadZombies;
        public int spawnedZombies;
        public double toSpawn;
        public double lastSpawn;
        public bool isLoaded;
        public bool isBeaten;

        public Level(string levelname)
        {
            LevelName = levelname;
            TileData = new Tile[84,54];
            PathfindingData = new Tile[42, 27];
            Zombies = new List<Zombie>();
            currentWave = 1;
            lastSpawn = -9999;
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
                        bool data = false;
                        string Line = Reader.ReadLine();
                        foreach (char c in Line)
                        {
                            if (yCount >= 54)
                            {
                                data = true;
                                break;
                            }
                            Tile e = new Tile();
                            HandleTile(e, c, xCount, yCount);
                            TileData[xCount, yCount] = e;
                            xCount++;
                        }
                        if (data)
                        {
                            string[] info = Line.Split(',');
                            Console.WriteLine(info);
                            waves = Convert.ToInt32(info[0]);
                            cureHP = Convert.ToInt32(info[1]);
                            zombies = Convert.ToInt32(info[2]);
                            spawnDelay = Convert.ToInt32(info[3]);

                            toSpawn = Math.Round((double)zombies * (double)Math.Ceiling((double)currentWave / 2));
                            break;
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

            PathfindingGrid();
        }

        private void HandleTile(Tile _tile, Char _char, int _x, int _y)
        {
            switch (_char)
            {
                case '.': // Blank tile (testing)
                    _tile.CollisionType = 1;
                    _tile.Texture = Globals.Textures["Grass"]; 
                    _tile.Rect = new Rectangle(_x * 20, _y * 20, 20, 20);
                    _tile.color = Color.DarkGray;
                    break;
                case 'W': // Wall
                    _tile.CollisionType = 0;
                    _tile.Texture = Globals.Textures["Stone"];
                    _tile.Rect = new Rectangle(_x * 20, _y * 20, 20, 20);
                    _tile.color = Color.White;
                    break;
                case 'C': // Cure
                    _tile.CollisionType = 2;
                    _tile.Texture = Globals.Textures["Cure"];
                    _tile.Rect = new Rectangle(_x * 20, _y * 20, 20, 20);
                    _tile.color = Color.White;
                    break;
            }
        }

        private void PathfindingGrid()
        {
            for (int i = 0; i < TileData.GetLength(0); i++)
            {
                for (int v = 0; v < TileData.GetLength(1); v++)
                {
                    if (i % 2 == 0 && v % 2 == 0)
                    {
                        Tile e = new Tile();
                        e.transparency = 0.8f;
                        e.Rect = new Rectangle(i * 20, v * 20, 40, 40);
                        e.Texture = Globals.Textures["Placeholder"];
                        e.CollisionType = 1;
                        PathfindingData[(i / 2), (v / 2)] = e;
                    }
                }
            }
            
            for (int i = 0; i < PathfindingData.GetLength(0); i++)
            {
                for (int v = 0; v < PathfindingData.GetLength(1); v++)
                {
                    Tile entry1 = TileData[i * 2, v * 2];
                    Tile entry2 = TileData[i * 2 + 1, v * 2];
                    Tile entry3 = TileData[i * 2, v * 2 + 1];
                    Tile entry4 = TileData[i * 2 + 1, v * 2 + 1];

                    if (entry1.CollisionType == 0 || entry2.CollisionType == 0 || entry3.CollisionType == 0 || entry4.CollisionType == 0)
                    {
                        PathfindingData[i, v].color = Color.Black;
                        PathfindingData[i, v].CollisionType = 0;
                    }
                    if (entry1.CollisionType == 2 || entry2.CollisionType == 2 || entry3.CollisionType == 2 || entry4.CollisionType == 2)
                    {
                        PathfindingData[i, v].color = Color.White;
                        PathfindingData[i, v].CollisionType = 2;
                        Console.WriteLine("Found Cure");
                    }
                }
            }
            isLoaded = true;
            Console.WriteLine("Level Loaded");
        }

        public void Update()
        {   
            if (!isBeaten && waves + 1 != currentWave && spawnedZombies < toSpawn && Globals.gameTime.TotalGameTime.TotalMilliseconds - lastSpawn >= spawnDelay)
            {
                Rectangle rect = new Rectangle(0, 0, 0, 0);
                Random random = new Random(Guid.NewGuid().GetHashCode());
                int where = random.Next(1, 5);
                switch (where)
                {
                    case 1:
                        rect = new Rectangle(20, random.Next(10, 980), 25, 25);
                        break;
                    case 2:
                        rect = new Rectangle(random.Next(10, 980), 15, 25, 25);
                        break;
                    case 3:
                        rect = new Rectangle(1600, random.Next(10, 980), 25, 25);
                        break;
                    case 4:
                        rect = new Rectangle(random.Next(10, 980), 1065, 25, 25);
                        break;
                    default:
                        Console.WriteLine("random error");
                        break;
                }
                Zombie e = new Zombie(rect);
                Zombies.Add(e);
                lastSpawn = Globals.gameTime.TotalGameTime.TotalMilliseconds;
                spawnedZombies++;
            }

            if (!isBeaten && waves + 1 != currentWave && deadZombies == toSpawn)
            {
                Console.WriteLine("Wave Beaten");
                deadZombies = 0;
                toSpawn = 0;
                spawnedZombies = 0;
                if (currentWave + 1 >= waves) { isBeaten = true; }
                currentWave = Math.Min(waves, currentWave += 1);
                toSpawn = Math.Round((double)zombies * (double)Math.Ceiling((double)currentWave / 2));
            }

            if (isBeaten)
            {
                Console.WriteLine("You won yippe");
            }
        }
    }
}
