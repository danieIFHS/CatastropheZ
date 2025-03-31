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
                Projectile e = new Projectile(Globals.Textures["Placeholder"], weapon.rect, weapon.attatchedPlayer.Degrees);
                Globals.Projectiles.Add(e);
            }
        }
    }
}
