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
    public class Projectile
    {
        Texture2D text;
        Rectangle rect;
        float degrees;
        Vector2 velocity;
        Vector2 position;

        public Projectile(Texture2D _text, Rectangle _rect, float _degrees)
        {
            text = _text;
            rect = _rect;
            degrees = _degrees;
            velocity = new Vector2((float)Math.Cos(degrees) * 15f, (float)Math.Sin(degrees) * 15f);
            //velocity.Normalize();

            position = new Vector2(rect.X, rect.Y);

            Console.WriteLine($"Degrees: {_degrees}, Velocity: {velocity}");
        }

        public void Update()
        {

            position.X += velocity.X;
            position.Y += velocity.Y;

            rect.X = (int)Math.Round(position.X);
            rect.Y = (int)Math.Round(position.Y);
        }

        public void Draw()
        {
            Globals.Batch.Draw(
                text,
                rect,
                new Rectangle(0, 0, text.Width, text.Height),
                Color.White,
                degrees,
                new Vector2(text.Width / 2, text.Height / 2),
                SpriteEffects.None,
                1
                );
        }
    }
}
