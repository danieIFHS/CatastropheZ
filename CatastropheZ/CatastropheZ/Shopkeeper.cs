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
    public class Shopkeeper
    {
        public bool available;
        public bool draw;
        public Tile tile;

        public int first;
        public int second;
        public int third;
        public int fourth;

        public int fPrice;
        public int sPrice;
        public int tPrice;
        public int foPrice;

        public string[] data;

        public List<Weapon> Weapons = new List<Weapon>()
        {
            null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null
        };

        public Shopkeeper()
        {
            data = new string[4];
            Weapons[0] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 100), Globals.Textures["Placeholder"], -350, 1, 1, "Blank");
            Weapons[1] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 150), Globals.Textures["Placeholder"], -350, 1, 20, "Sniper");
            Weapons[2] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 200), Globals.Textures["Placeholder"], -350, 1, 100, "AK-47");
            Weapons[3] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 250), Globals.Textures["Placeholder"], -350, 1, 200, "Deagle");
            Weapons[4] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[5] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[6] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[7] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[8] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[9] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[10] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[11] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");
            Weapons[12] = new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default");

        }
        public void Refresh()
        {
            Console.WriteLine("Refreshing shop...");
            Random random = new Random(Guid.NewGuid().GetHashCode());
            int slot1 = random.Next(1, 4);
            random = new Random(Guid.NewGuid().GetHashCode());
            int slot2 = random.Next(4, 7);
            random = new Random(Guid.NewGuid().GetHashCode());
            int slot3 = random.Next(7, 10);
            random = new Random(Guid.NewGuid().GetHashCode());
            int slot4 = random.Next(10, 13);

            Console.WriteLine(slot1 + " | " + slot2 + " | " + slot3 + " | " + slot4);

            first = slot1;
            second = slot2;
            third = slot3;
            fourth = slot4;

            Determine();
        }
        public void Determine()
        {
            int[] selects = new int[4];
            selects[0] = first;
            selects[1] = second;
            selects[2] = third;
            selects[3] = fourth;

            for (int i = 0; i < selects.Length; i++)
            {
                switch (selects[i])
                {
                    case 1:
                        data[i] = "Sniper: 75 Z-Coins, \nPierces Zombies";
                        fPrice = 75;
                        break;
                    case 2:
                        data[i] = "AK-47: 35 Z-Coins";
                        fPrice = 35;
                        break;
                    case 3:
                        data[i] = "Deagle: 20 Z-Coins";
                        fPrice = 20;
                        break;
                    default:
                        data[i] = "Default: 10 Z-Coins";
                        sPrice = 10;
                        tPrice = 10;
                        foPrice = 10;
                        break;
                }
            }
        }
        public void Update()
        {
            
        }
        public void Draw()
        {
            if (draw)
            {
                Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1680, 180, 240, 420), Color.White);

                Globals.Batch.DrawString(Globals.Font, data[0], new Vector2(1750, 320), Color.Black);
                Globals.Batch.DrawString(Globals.Font, data[1], new Vector2(1750, 380), Color.Black);
                Globals.Batch.DrawString(Globals.Font, data[2], new Vector2(1750, 440), Color.Black);
                Globals.Batch.DrawString(Globals.Font, data[3], new Vector2(1750, 500), Color.Black);

                Globals.Batch.Draw(Globals.Textures["AButton"], new Rectangle(1680, 310, 50, 50), Color.White);
                Globals.Batch.Draw(Globals.Textures["XButton"], new Rectangle(1680, 370, 50, 50), Color.White);
                Globals.Batch.Draw(Globals.Textures["YButton"], new Rectangle(1680, 430, 50, 50), Color.White);
                Globals.Batch.Draw(Globals.Textures["BButton"], new Rectangle(1680, 490, 50, 50), Color.White);
            }
        }
    }
}
