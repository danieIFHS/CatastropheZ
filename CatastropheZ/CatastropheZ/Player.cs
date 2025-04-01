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
    public class Player
    {
        public PlayerIndex Index;
        public GamePadState padState;
        public Rectangle Rect;
        public Texture2D Texture;

        public float Degrees;
        public Vector2 position;
        public const float Speed = 3f;

        public Weapon activeWeapon;

        public Player(PlayerIndex plrindex, Rectangle rect, Texture2D texture)
        {
            Index = plrindex;
            Rect = rect;
            Texture = texture;
            position = new Vector2(rect.X, rect.Y);

            activeWeapon = new Weapon(this, Globals.Textures["Placeholder"], "Gun");
            activeWeapon.cooldown = 110;
            activeWeapon.lastUsed = -5000;
            
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

            activeWeapon.rect = new Rectangle(Rect.X, Rect.Y, 10, 50);
            if (padState.Triggers.Right > 0)
            {
                Console.WriteLine(Globals.gameTime.TotalGameTime.TotalMilliseconds);
                if (Globals.gameTime.TotalGameTime.TotalMilliseconds - activeWeapon.lastUsed > activeWeapon.cooldown)
                {
                    activeWeapon.lastUsed = (int)Globals.gameTime.TotalGameTime.TotalMilliseconds;
                    switch (activeWeapon.Type)
                    {
                        case "Gun":
                            activeWeapon.Fire();
                            break;

                        default:
                            break;
                    }
                }
            }
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
            Vector2 playerCenter = new Vector2(Rect.X + Rect.Width / 2, Rect.Y + Rect.Height / 2);

            Globals.Batch.Draw(
                Texture,
                Rect,
                new Rectangle(0, 0, Texture.Width, Texture.Height),
                Color.White,
                Degrees,
                new Vector2(Texture.Width / 2, Texture.Height / 2),
                SpriteEffects.None,
                1);

            Globals.Batch.Draw(
                activeWeapon.texture,
                activeWeapon.rect,
                new Rectangle(0, 0, activeWeapon.texture.Width, activeWeapon.texture.Height),
                Color.Red,
                Degrees,
                new Vector2(-350, activeWeapon.texture.Height / 2),
                SpriteEffects.None,
                1);

        }
    }
}
