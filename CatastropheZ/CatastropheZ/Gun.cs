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
    public static class Gun
    {
        public static void Fire(this Weapon weapon)
        {
            if (weapon.Equipped == true)
            {
                Vector2 tipOffset = new Vector2(20, 0);
                Vector2 rotatedTipOffset = Vector2.Transform(tipOffset, Matrix.CreateRotationZ(weapon.attatchedPlayer.Degrees));
                Vector2 gunTipPosition = weapon.attatchedPlayer.position + rotatedTipOffset;
                Projectile e = new Projectile(Globals.Textures["Placeholder"],new Rectangle((int)gunTipPosition.X, (int)gunTipPosition.Y, 10, 10), 
                    weapon.attatchedPlayer.Degrees - MathHelper.PiOver2 );

                Globals.Projectiles.Add(e);
            }
        }
    }
}
