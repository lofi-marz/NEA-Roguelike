using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class SpriteComponent : Component
    {
        public Texture2D SpriteSheet;
        public Rectangle SourceRect;
        public int Height;
        public int Width;
        public int Depth = 1;
        public Vector2 Scale = new Vector2(1f);
        public SpriteComponent(Texture2D spriteSheet, Rectangle sourceRect, Vector2 scale, int depth = 1, int height = 16, int width = 16)
        {
            SpriteSheet = spriteSheet;
            SourceRect = sourceRect;
            Height = height;
            Width = width;
            Depth = depth;
            Scale = scale;
        }
    }



}
