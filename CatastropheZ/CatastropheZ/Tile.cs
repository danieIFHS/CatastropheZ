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
    public class Tile
    {
        public int CollisionType; // 0 is passable, 1 is impassable
        public Texture2D Texture;
        public Rectangle Rect;
        public Color color;
        public float transparency;

        public Tile()
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
            color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            transparency = 1f;
        }
        public void Draw()
        {
            Globals.Batch.Draw(Texture, Rect, color * transparency);
        }
    }
}
