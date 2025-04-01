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
    public class Weapon
    {
        public Player attatchedPlayer;
        public Texture2D texture;
        public bool Equipped;
        public string Type;
        public Rectangle rect;
        public int lastUsed;
        public int cooldown;

        public Weapon(Player _player, Texture2D _texture, string _Type)
        {
            attatchedPlayer = _player;
            texture = _texture;
            Type = _Type;

            Equipped = true; // Add a check here later to see if the player's active slot is this weapon, if so set equipped to true
        }
    }
}
