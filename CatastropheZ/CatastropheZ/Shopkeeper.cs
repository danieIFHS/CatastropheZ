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

        public int first, second, third, fourth;
        public int fPrice, sPrice, tPrice, foPrice;
        public string[] data;

        public List<Weapon> Weapons = new List<Weapon>();
        private Weapon[] selectedWeapons = new Weapon[4];

        public Shopkeeper()
        {
            data = new string[4];

            // Populate weapons
            Weapons.Add(new Weapon(null, Globals.Textures["Shotgun"], "Gun", new Vector2(10, 50), Globals.Textures["Placeholder"], -5, 1, 750, "Shotgun", 1, 5));      
            Weapons.Add(new Weapon(null, Globals.Textures["Sniper"], "Gun", new Vector2(2, 150), Globals.Textures["Placeholder"], -10, 1, 1250, "Sniper", 40, 5));            
            Weapons.Add(new Weapon(null, Globals.Textures["AR"], "Gun", new Vector2(10, 50), Globals.Textures["Placeholder"], -5, 1, 110, "AK-47", 10, 5));

            Weapons.Add(new Weapon(null, Globals.Textures["Minigun"], "Gun", new Vector2(10, 50), Globals.Textures["Placeholder"], -5, 1, 35, "Minigun", 10, 5));
            Weapons.Add(new Weapon(null, Globals.Textures["BeeGun"], "Gun", new Vector2(15, 50), Globals.Textures["Placeholder"], -15, 1, 175, "Bee Gun", 10, 5));
            Weapons.Add(new Weapon(null, Globals.Textures["DjGun"], "Gun", new Vector2(20, 50), Globals.Textures["Placeholder"], -5, 1, 350, "DJ Gun", 10, 5));

            Weapons.Add(new Weapon(null, Globals.Textures["Pistol"], "Gun", new Vector2(6, 25), Globals.Textures["Placeholder"], -8, 1, 500, "Deagle", 15, 5));
            Weapons.Add(new Weapon(null, Globals.Textures["LittleMini"], "Gun", new Vector2(8, 25), Globals.Textures["Placeholder"], -12, 1, 75, "Mini Minigun", 15, 5));
            Weapons.Add(new Weapon(null, Globals.Textures["Uzi"], "Gun", new Vector2(6, 25), Globals.Textures["Placeholder"], -11, 1, 90, "Uzi", 15, 5));

            Weapons.Add(new Weapon(null, Globals.Textures["Magnum"], "Gun", new Vector2(6, 25), Globals.Textures["Placeholder"], -8, 1, 150, "Magnum", 15, 5));
            Weapons.Add(new Weapon(null, Globals.Textures["Sawblade Gun"], "Gun", new Vector2(20, 20), Globals.Textures["Placeholder"], -8, 1, 400, "Saw Gun", 10, 5));
            Weapons.Add(new Weapon(null, Globals.Textures["MiniShotgun"], "Gun", new Vector2(8, 25), Globals.Textures["Placeholder"], -12, 1, 75, "MiniShotgun", 15, 5));

            for (int i = 4; i < 24; i++)
            {
                Weapons.Add(new Weapon(null, Globals.Textures["Placeholder"], "Gun", new Vector2(10, 300), Globals.Textures["Placeholder"], -350, 1, 300, "Default", 10, 5));
            }
        }

        public void Refresh()
        {
            Console.WriteLine("Refreshing shop...");
            Random random = new Random(Guid.NewGuid().GetHashCode());
            first = random.Next(0, 3);
            second = random.Next(3, 6);
            third = random.Next(6, 9);
            fourth = random.Next(9, 12);

            Console.WriteLine($"{first} | {second} | {third} | {fourth}");

            selectedWeapons[0] = Weapons[first];
            selectedWeapons[1] = Weapons[second];
            selectedWeapons[2] = Weapons[third];
            selectedWeapons[3] = Weapons[fourth];

            fPrice = selectedWeapons[0].price;
            sPrice = selectedWeapons[1].price;
            tPrice = selectedWeapons[2].price;
            foPrice = selectedWeapons[3].price;

            data[0] = $"{selectedWeapons[0].name}: {fPrice} Z-Coins";
            data[1] = $"{selectedWeapons[1].name}: {sPrice} Z-Coins";
            data[2] = $"{selectedWeapons[2].name}: {tPrice} Z-Coins";
            data[3] = $"{selectedWeapons[3].name}: {foPrice} Z-Coins";
        }

        public void Update()
        {
            // To be implemented as needed
        }

        public void Draw()
        {
            if (!draw) return;

            Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1680, 180, 240, 420), Color.White);

            for (int i = 0; i < 4; i++)
            {
                Globals.Batch.DrawString(Globals.Font, data[i], new Vector2(1725, 320 + i * 60), Color.Black);
            }

            Globals.Batch.Draw(Globals.Textures["AButton"], new Rectangle(1680, 310, 40, 40), Color.White);
            Globals.Batch.Draw(Globals.Textures["XButton"], new Rectangle(1680, 370, 40, 40), Color.White);
            Globals.Batch.Draw(Globals.Textures["YButton"], new Rectangle(1680, 430, 40, 40), Color.White);
            Globals.Batch.Draw(Globals.Textures["BButton"], new Rectangle(1680, 490, 40, 40), Color.White);
        }
    }

}
