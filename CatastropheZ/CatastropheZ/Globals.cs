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
    public class Globals
    {
        public static Dictionary<string, Texture2D> Textures;
        public static Dictionary<string, Song> SFX;
        public static SpriteBatch Batch;
        public static Level ActiveLevel;
        public static bool InGame;
        public static List<Projectile> Projectiles;
        public static GameTime gameTime;
        public static List<Player> Players;
        public static SpriteFont Font;
        public static SpriteFont FontBig;
    }
}
