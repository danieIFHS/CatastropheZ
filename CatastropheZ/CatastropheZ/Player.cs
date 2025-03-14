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
            padState = GamePad.GetState(Index, GamePadDeadZone.Circular);

            Vector2 Movement = padState.ThumbSticks.Left;
            Vector2 Rotation = padState.ThumbSticks.Right;

            Degrees = (float)Math.Atan2(Rotation.X, Rotation.Y);

            Rect.X += (int)Math.Round(Movement.X) * 3;
            Rect.Y -= (int)Math.Round(Movement.Y) * 3;

        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(Texture, Rect, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, Degrees, new Vector2(Texture.Width / 2, Texture.Height / 2), SpriteEffects.None, 1);
        }
    }
}
