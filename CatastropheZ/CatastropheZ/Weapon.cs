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
    public class Weapon : System.ICloneable
    {
        public Player attatchedPlayer;
        public Texture2D texture;
        public bool Equipped;
        public string Type;
        public Vector2 size;
        public Texture2D icon;
        public int offset;
        public int slot;
        public int lastUsed;
        public int cooldown;
        public string name;
        public int x;
        public float rumble;
        public int price;

        public Weapon(Player _player, Texture2D _texture, string _Type, Vector2 _size, Texture2D _icon, int _offset, int _slot, int _cooldown, string _name, float _rumble, int _price)
        {
            attatchedPlayer = _player;
            texture = _texture;
            Type = _Type;

            size = _size;
            icon = _icon;
            offset = _offset;
            slot = _slot;
            name = _name;
            rumble = _rumble;

            cooldown = _cooldown;
            price = _price;

            Equipped = true; // Add a check here later to see if the player's active slot is this weapon, if so set equipped to true
        }

        public void Fire()
        {


            if (Equipped == true)
            {
                Vector2 tipOffset = new Vector2(20, (-size.Y / 2) + 5);
                Vector2 rotatedTipOffset = Vector2.Transform(tipOffset, Matrix.CreateRotationZ(attatchedPlayer.Degrees));
                Vector2 gunTipPosition = attatchedPlayer.position + rotatedTipOffset;
                
                Texture2D texture = Globals.Textures["Placeholder"];
                
                switch (name)
                {
                    case "Deagle":
                        Globals.SFX["Deagle"].Play();
                        texture = Globals.Textures["Bullet"];
                        Console.WriteLine(name);
                        break;
                    case "Default":
                        Globals.SFX["Bolt"].Play();
                        texture = Globals.Textures["Placeholder"];
                        Console.WriteLine(name);
                        break;
                    case "AK-47":
                        Globals.SFX["AK"].Play();
                        texture = Globals.Textures["Placeholder"];
                        Console.WriteLine(name);
                        break;
                    case "Bee Gun":
                        Globals.SFX["BeeGun"].Play();
                        Console.WriteLine(name);
                        break;
                    case "Saw Gun":
                        Globals.SFX["SawGun"].Play();
                        Console.WriteLine(name);
                        break;
                    case "DJ Gun":
                        Globals.SFX["DJGun"].Play();
                        Console.WriteLine(name);
                        break;
                    default:
                        Globals.SFX["Bolt"].Play();
                        texture = Globals.Textures["Placeholder"];
                        break;
                        // Music gun can play mulitple, randomly decided sounds
                        // https://www.youtube.com/watch?v=nhJgJ-tRivg
                }
                Projectile e = new Projectile(texture, new Rectangle((int)gunTipPosition.X, (int)gunTipPosition.Y, 10, 10),
                    attatchedPlayer.Degrees - MathHelper.PiOver2, attatchedPlayer);
                Globals.Projectiles.Add(e);

            }
            
        }
        // probably put sound effects in the fire function, just make a big switch case for each name
        public Weapon Clone()
        {
            return this.MemberwiseClone() as Weapon; 
        }

        object System.ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}
