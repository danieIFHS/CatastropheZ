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
    class Player
    {
        public PlayerIndex Index;
        public GamePadState padState;
        public Rectangle Rect;
        public Texture2D Texture;

        private float Degrees;

        public Player(PlayerIndex plrindex, Rectangle rect, Texture2D texture)
        {
            Index = plrindex;
            Rect = rect;
            Texture = texture;
        }

        public void Update()
        {
            Rectangle prevRect = Rect;

            padState = GamePad.GetState(Index, GamePadDeadZone.Circular);

            Vector2 Movement = padState.ThumbSticks.Left;
            Vector2 Rotation = padState.ThumbSticks.Right;

            Degrees = (float)Math.Atan2(Rotation.X, Rotation.Y);

            Rect.X += (int)Math.Round(Movement.X) * 3;
            Rect.Y -= (int)Math.Round(Movement.Y) * 3;

            CollisionManager();

            if (Rect.X == prevRect.X)
            {
                Movement = Vector2.Zero;
            }
            if (Rect.Y == prevRect.Y)
            {
                Movement = Vector2.Zero;
            }
        }

        public void CollisionManager()
        {
            Rectangle bounds = new Rectangle(Rect.X - 15, Rect.Y - 15 Rect.Width, Rect.Height);
            int leftTile = (int)Math.Floor((float)(bounds.Left) / 20);
            int rightTile = Math.Min((int)Math.Ceiling(((float)(bounds.Right) / 20)) - 1, 82);
            int topTile = (int)Math.Floor((float)(bounds.Top) / 20);
            int bottomTile = (int)Math.Ceiling(((float)(bounds.Bottom) / 20)) - 1;

            Console.WriteLine(leftTile + " | " + rightTile + " | " + topTile + " | " + bottomTile + " | " + bounds.Left + " | " + bounds.Right + " | " + bounds.Top + " | " + bounds.Bottom + " | " + Rect.X);

            for (int y = topTile; y <= bottomTile; y++)
            {
                for (int x = leftTile; x <= rightTile; x++)
                {
                    Tile toCheck = Globals.ActiveLevel.TileData[x, y];
                    //Console.WriteLine(x + " | " + y + " | " + toCheck.CollisonType);
                    if (toCheck.CollisonType != 1)
                    {
                        Rectangle tileBounds = toCheck.Rect;
                        Vector2 depth = bounds.GetIntersectionDepth(tileBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            if (absDepthY < absDepthX)
                            {
                                if (toCheck.CollisonType != 1)
                                {
                                    Rect.Y = (Rect.Y + (int)depth.Y);
                                }
                            }
                            else if (toCheck.CollisonType != 1)
                            {
                                Rect.X = (Rect.X + (int)depth.X);
                            }

                            //Console.WriteLine(x + " | " + y + " | " + absDepthX + " | " + absDepthY);
                        }

                    }
                }
            }
        }

        public void Draw()
        {
            Globals.Batch.Draw(Texture, Rect, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Degrees, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 1);
        }
    }
}
