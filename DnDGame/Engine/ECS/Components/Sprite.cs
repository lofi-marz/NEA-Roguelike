using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class Sprite : Component
    {
        public string SpriteSheet;
        public string Tile;
        public int Height;
        public int Width;
        public int Depth = 1;
        public Sprite(string spriteSheet, string tile, int depth = 1, int height = 16, int width = 16)
        {
            SpriteSheet = spriteSheet;
            Tile = tile;

            Height = height;
            Width = width;
            Depth = depth;
        }
    }



}
