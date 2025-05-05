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
        public GamePadState oldPadState;
        public Rectangle Rect;
        public Texture2D Texture;
        public Texture2D iconText;

        public float Degrees;
        public float realDegrees = 0f;
        public Vector2 position;
        public const float Speed = 3f;
        public Color plrColor;

        public float Health = 100;
        public bool isDead = false;
        public int kills = 0;
        public int ZCoins = 0;

        public List<Weapon> Weapons;
        public Weapon activeWeapon;
        public int wepIndex;
        public float rumbleLeft;

        public bool inShop = false;

        public Player(PlayerIndex plrindex, Rectangle rect, Texture2D texture)
        {
            Index = plrindex;
            Rect = rect;
            position = new Vector2(rect.X, rect.Y);

            Weapons = new List<Weapon>();
            Weapon e = new Weapon( //Default primary
                this,
                Globals.Textures["Placeholder"],
                "Gun",
                new Vector2(10, 50),
                Globals.Textures["Placeholder"],
                -350,
                1,
                10,
                "Blank", 15);
            e.cooldown = 50;
            e.lastUsed = -5000;
            Weapons.Add(e);
            activeWeapon = Weapons[0];
            wepIndex = 0;

            Weapon b = new Weapon( //Default secondary
                this,
                Globals.Textures["Placeholder"],
                "Gun",
                new Vector2(10, 80),
                Globals.Textures["Placeholder"],
                -350,
                1,
                1,
                "Blank", 15);
            b.cooldown = 1;
            b.lastUsed = -5000;
            Weapons.Add(b);

            switch (plrindex) 
            {
                case PlayerIndex.One:
                    plrColor = Color.Red;
                    Texture = Globals.Textures["Red"];
                    iconText = Globals.Textures["RedIcon"];
                    break;
                case PlayerIndex.Two:
                    plrColor = Color.Blue;
                    Texture = Globals.Textures["Blue"];
                    iconText = Globals.Textures["BlueIcon"];
                    break;
                case PlayerIndex.Three:
                    plrColor = Color.Yellow;
                    Texture = Globals.Textures["Yellow"];
                    iconText = Globals.Textures["YellowIcon"];
                    break;
                case PlayerIndex.Four:
                    plrColor = Color.Green;
                    Texture = Globals.Textures["Green"];
                    iconText = Globals.Textures["GreenIcon"];
                    break;
                default:
                    plrColor = Color.White;
                    Texture = Globals.Textures["Placeholder"];
                    iconText = Globals.Textures["Placeholder"];
                    break;
            }

        }

        public void Update()
        {
            if (!isDead)
            {
                padState = GamePad.GetState(Index, GamePadDeadZone.Circular);

                Vector2 movementInput = padState.ThumbSticks.Left;
                Vector2 rotationInput = padState.ThumbSticks.Right;

                if (rotationInput.LengthSquared() > 0.001f) 
                {
                    realDegrees = (float)Math.Atan2(rotationInput.X, rotationInput.Y);
                }

                Degrees = realDegrees;

                position.X += movementInput.X * Speed;
                position.Y -= movementInput.Y * Speed;

                Rect.X = (int)Math.Round(position.X);
                Rect.Y = (int)Math.Round(position.Y);

                CollisionManager();

                if (padState.Triggers.Right > 0)
                {
                    if (Globals.gameTime.TotalGameTime.TotalMilliseconds - activeWeapon.lastUsed > activeWeapon.cooldown)
                    {
                        activeWeapon.lastUsed = (int)Globals.gameTime.TotalGameTime.TotalMilliseconds;
                        switch (activeWeapon.Type)
                        {
                            case "Gun":
                                activeWeapon.Fire();
                                rumbleLeft = activeWeapon.rumble;
                                break;

                            default:
                                break;
                        }

                    }
                }

                if (rumbleLeft != 0)
                {
                    GamePad.SetVibration(PlayerIndex.One, 1, 1);
                    rumbleLeft--;
                }
                else
                {
                    GamePad.SetVibration(PlayerIndex.One, 0, 0);
                }

                if (padState.Buttons.RightShoulder > 0 && oldPadState.Buttons.RightShoulder == 0)
                {
                    if (wepIndex == Weapons.Count - 1)
                    {
                        wepIndex = 0;
                    }
                    else
                    {
                        wepIndex = wepIndex + 1;
                    }
                    activeWeapon = Weapons[wepIndex];
                }
                if (padState.Buttons.LeftShoulder > 0 && oldPadState.Buttons.LeftShoulder == 0)
                {
                    if (wepIndex == 0)
                    {
                        wepIndex = Weapons.Count - 1;
                    }
                    else
                    {
                        wepIndex = wepIndex - 1;
                    }
                    activeWeapon = Weapons[wepIndex];
                }

                if (padState.Buttons.A > 0 && oldPadState.Buttons.A == 0)
                {
                    if (Globals.ActiveLevel.inBetween)
                    {
                        Purchase(1);
                    }
                }
                if (padState.Buttons.X > 0 && oldPadState.Buttons.X == 0)
                {
                    if (Globals.ActiveLevel.inBetween)
                    {
                        Purchase(2);
                    }
                }
                if (padState.Buttons.Y > 0 && oldPadState.Buttons.Y == 0)
                {
                    if (Globals.ActiveLevel.inBetween)
                    {
                        Purchase(3);
                    }
                }
                if (padState.Buttons.B > 0 && oldPadState.Buttons.B == 0)
                {
                    if (Globals.ActiveLevel.inBetween)
                    {
                        Purchase(4);
                    }
                }

                if (padState.Buttons.Start > 0 && oldPadState.Buttons.Start == 0)
                {
                    Globals.ActiveLevel.inBetween = false;
                }

                if (Health <= 0) { isDead = true; }

                oldPadState = padState;
            }
        }

        public void Purchase(int index)
        {
            int price = 0;
            int index2 = 0;
            switch (index)
            {
                case 1:
                    price = Globals.ActiveLevel.shopkeeper.fPrice;
                    index2 = Globals.ActiveLevel.shopkeeper.first;
                    break;
                case 2:
                    price = Globals.ActiveLevel.shopkeeper.sPrice;
                    index2 = Globals.ActiveLevel.shopkeeper.second;
                    break;
                case 3:
                    price = Globals.ActiveLevel.shopkeeper.tPrice;
                    index2 = Globals.ActiveLevel.shopkeeper.third;
                    break;
                case 4:
                    price = Globals.ActiveLevel.shopkeeper.foPrice;
                    index2 = Globals.ActiveLevel.shopkeeper.fourth;
                    break;
            }
            Console.WriteLine(index2);
            if (ZCoins >= price)
            {
                Console.WriteLine("Bought");
                ZCoins -= price;
                if (index < 3)
                {
                    Weapons[0] = Globals.ActiveLevel.shopkeeper.Weapons[index2].Clone();
                    Weapons[0].lastUsed = -5000;
                    Weapons[0].attatchedPlayer = this;
                    activeWeapon = Weapons[0];
                }
                else
                {
                    Weapons[1] = Globals.ActiveLevel.shopkeeper.Weapons[index2].Clone();
                    Weapons[1].lastUsed = -5000;
                    Weapons[1].attatchedPlayer = this;
                    activeWeapon = Weapons[1];
                }
            }
            else
            {
                Console.WriteLine("Broke");
            }
        }

        public void CollisionManager()
        {
            Rectangle bounds = new Rectangle(Rect.X - 15, Rect.Y - 15, Rect.Width, Rect.Height);
            int leftTile = Math.Max((int)Math.Floor(bounds.Left / 20f), 0);
            int rightTile = Math.Min((int)Math.Ceiling(bounds.Right / 20f) - 1, 83);
            int topTile = Math.Max((int)Math.Floor(bounds.Top / 20f), 0);
            int bottomTile = Math.Min((int)Math.Ceiling(bounds.Bottom / 20f) - 1, 53);

            for (int y = topTile; y <= bottomTile; y++)
            {
                for (int x = leftTile; x <= rightTile; x++)
                {
                    Tile toCheck = Globals.ActiveLevel.TileData[x, y];
                    if (toCheck.CollisionType == 0)
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

            position.X = MathHelper.Clamp(position.X, 15, (84 * 20) - Rect.Width / 2);
            position.Y = MathHelper.Clamp(position.Y, 15, (54 * 20) - Rect.Height / 2);

            Rect.X = (int)Math.Round(position.X);
            Rect.Y = (int)Math.Round(position.Y);
        }

        public void Draw(int position)
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
                new Rectangle(Rect.X, Rect.Y, (int)activeWeapon.size.X, (int)activeWeapon.size.Y),
                new Rectangle(0, 0, activeWeapon.texture.Width, activeWeapon.texture.Height),
                Color.Red,
                Degrees,//-350
                new Vector2(activeWeapon.offset, activeWeapon.texture.Height / 2),
                SpriteEffects.None,
                1);

            //Player info box
            Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1680, 600 + (position * 120), 240, 120), plrColor);
            Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1685, 605 + (position * 120), 230, 110), Color.DimGray);
            Globals.Batch.Draw(iconText, new Rectangle(1685, 605 + (position * 120), 60, 60), Color.AliceBlue); // Player icon
            Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1687, 698 + (position * 120), (int)(2.265 * Health), 15), Color.Red); // Health bar
            Globals.Batch.DrawString(Globals.Font, "Health:" + Health.ToString(), new Vector2(1687, 680 + (position * 120)), Color.White); // Health Counter
            Globals.Batch.DrawString(Globals.Font, "Kills:  " + kills.ToString(), new Vector2(1827, 605 + (position * 120)), Color.White); // Kill Counter
            Globals.Batch.DrawString(Globals.Font, "Z-Coins:" + ZCoins.ToString(), new Vector2(1827, 620 + (position * 120)), Color.White); // Kill Counter

            Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1750, 605 + (position * 120), 35, 35), Color.White); // Primary Weapon
            if (Weapons.Count == 2)
            {
                Globals.Batch.Draw(Globals.Textures["Placeholder"], new Rectangle(1790, 605 + (position * 120), 35, 35), Color.White); // Secondary Weapon
            }
            if (wepIndex == 0)
            {
                Globals.Batch.Draw(Globals.Textures["Border"], new Rectangle(1750, 605 + (position * 120), 35, 35), Color.Red);
            }
            else if (wepIndex == 1)
            {
                Globals.Batch.Draw(Globals.Textures["Border"], new Rectangle(1790, 605 + (position * 120), 35, 35), Color.Red); 
            }

            if (inShop)
            {
                Globals.Batch.DrawString(Globals.Font, "In Shop", new Vector2(1827, 680 + (position * 120)), Color.White);
            }

        }
    }
}


