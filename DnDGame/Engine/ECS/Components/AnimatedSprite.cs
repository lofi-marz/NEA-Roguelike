using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class AnimatedSpriteComponent : Component
    {
        public Texture2D SpriteSheet;
        public Rectangle FrameRect;
        public int FrameHeight;
        public int FrameWidth;
        public int XFrame = 0;
        public int YFrame = 0;
        public int CurrentFrame;
        public Vector2 Scale = new Vector2(1f);

        public AnimatedSpriteComponent(Texture2D spriteSheet,  Vector2 scale, int height = 16, int width = 16)
        { 
            SpriteSheet = spriteSheet;
            Scale = scale;
            FrameHeight = height;
            FrameWidth = width;
            FrameRect = new Rectangle(0, 0, width, height);
        }
    }



}
