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
    public class Zombie
    {
        public Rectangle rect;
        public Texture2D text;
        public Vector2 position;

        public Zombie()
        {
            rect = new Rectangle(30, 300, 25, 25);
            text = Globals.Textures["Placeholder"];
        }

        public void Draw()
        {
            Globals.Batch.Draw(text, rect, Color.Black);
        }
    }
}
