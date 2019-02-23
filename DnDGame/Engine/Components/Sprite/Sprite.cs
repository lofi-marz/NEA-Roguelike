using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine
{

    public class Sprite : Component
    {
        public string SpriteSheet;
        public string Tile;

		public float Depth = 0f;
		public Direction Facing;
        public Sprite(string spriteSheet, string tile, float depth = 0f)
        {
            SpriteSheet = spriteSheet;
            Tile = tile;


            Depth = depth;
			Facing = Direction.East;
        }
    }



}
