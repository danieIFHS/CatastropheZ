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
            float veloAngle = degrees;
            velocity = new Vector2((float)Math.Cos(veloAngle) * 30f, (float)Math.Sin(veloAngle) * 30f);
            position = new Vector2(rect.X, rect.Y);
        }

        public void Update()
        {

            position.X += velocity.X;
            position.Y += velocity.Y;

            rect.X = (int)Math.Round(position.X);
            rect.Y = (int)Math.Round(position.Y);

            for (int i = 0; i < Globals.ActiveLevel.Zombies.Count; i++)
            {
                if (rect.Intersects(Globals.ActiveLevel.Zombies[i].rect))
                {
                    Globals.ActiveLevel.Zombies.RemoveAt(i);
                    
                    for (int v = 0; v < Globals.Projectiles.Count; v++)
                    {
                        if (Globals.Projectiles[v].rect == rect)
                        {
                            Globals.Projectiles.RemoveAt(v);
                            break;
                        }
                    }
                }
            }

            for (int i = 0; i < Globals.ActiveLevel.TileData.GetLength(0); i++)
            {
                for (int j = 0; j < Globals.ActiveLevel.TileData.GetLength(1); j++)
                {
                    if (rect.Intersects(Globals.ActiveLevel.TileData[i, j].Rect) && Globals.ActiveLevel.TileData[i, j].CollisionType == 0)
                    {
                        for (int v = 0; v < Globals.Projectiles.Count; v++)
                        {
                            if (Globals.Projectiles[v].rect == rect)
                            {
                                Globals.Projectiles.RemoveAt(v);
                                break;
                            }
                        }
                    }
                }
            }
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
