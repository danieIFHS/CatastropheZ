﻿using Microsoft.Xna.Framework;
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
        private Vector2 position; 
        private const float Speed = 3f; 

        public Player(PlayerIndex plrindex, Rectangle rect, Texture2D texture)
        {
            Index = plrindex;
            Rect = rect;
            Texture = texture;
            position = new Vector2(rect.X, rect.Y);
        }

        public void Update()
        {
            padState = GamePad.GetState(Index, GamePadDeadZone.Circular);

            Vector2 movementInput = padState.ThumbSticks.Left;
            Vector2 rotationInput = padState.ThumbSticks.Right;

            Degrees = (float)Math.Atan2(rotationInput.X, rotationInput.Y);

            position.X += movementInput.X * Speed;
            position.Y -= movementInput.Y * Speed;

            Rect.X = (int)Math.Round(position.X);
            Rect.Y = (int)Math.Round(position.Y);

            CollisionManager();
        }

        public void CollisionManager()
        {
            Rectangle bounds = new Rectangle(Rect.X - 15, Rect.Y - 15, Rect.Width, Rect.Height);
            int leftTile = Math.Max((int)Math.Floor(bounds.Left / 20f), 0);
            int rightTile = Math.Min((int)Math.Ceiling(bounds.Right / 20f) - 1, 82);
            int topTile = Math.Max((int)Math.Floor(bounds.Top / 20f), 0);
            int bottomTile = Math.Min((int)Math.Ceiling(bounds.Bottom / 20f) - 1, 53);

            for (int y = topTile; y <= bottomTile; y++)
            {
                for (int x = leftTile; x <= rightTile; x++)
                {
                    Tile toCheck = Globals.ActiveLevel.TileData[x, y];
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
                                position.Y += depth.Y;
                            }
                            else
                            {
                                position.X += depth.X;
                            }

                            Rect.X = (int)Math.Round(position.X);
                            Rect.Y = (int)Math.Round(position.Y);

                            bounds = new Rectangle(Rect.X - 15, Rect.Y - 15, Rect.Width, Rect.Height);
                        }
                    }
                }
            }

            position.X = MathHelper.Clamp(position.X, 15, (83 * 20) - Rect.Width / 2);
            position.Y = MathHelper.Clamp(position.Y, 15, (54 * 20) - Rect.Height / 2);

            Rect.X = (int)Math.Round(position.X);
            Rect.Y = (int)Math.Round(position.Y);
        }

        public void Draw()
        { 
            Globals.Batch.Draw(
                Texture,
                Rect,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                Color.White,
                Degrees,
                new Vector2(Texture.Width / 2, Texture.Height / 2),
                SpriteEffects.None,
                1);
        }
    }
}
