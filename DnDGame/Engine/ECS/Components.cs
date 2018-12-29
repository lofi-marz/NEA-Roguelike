using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class Component
    {

    }
    public class PositionComponent : Component
    {
        public Vector2 Pos;

        public PositionComponent(Vector2 pos)
        {
            Pos = pos;
        }
    }



    public class VelocityComponent : Component
    {
        public Vector2 Velocity;

        public VelocityComponent(Vector2 velocity)
        {
            Velocity = velocity;
        }

    }

    public class DragComponent : Component
    {
        public Vector2 Drag;

        public DragComponent(Vector2 drag)
        {
            Drag = drag;
        }

    }

    public class AccelerationComponent : Component
    {
        public Vector2 Acceleration;

        public AccelerationComponent(Vector2 acc)
        {
            Acceleration = acc;
        }
    }

    public class SpriteComponent : Component
    {
        public Texture2D SpriteSheet;
        public Rectangle SourceRect;
        public int Height;
        public int Width;
        public Vector2 Scale = new Vector2(1f);
        public SpriteComponent(Texture2D spriteSheet, Rectangle sourceRect, Vector2 scale, int height = 16, int width = 16)
        {
            SpriteSheet = spriteSheet;
            SourceRect = sourceRect;
            Height = height;
            Width = width;
            Scale = scale;
        }
    }

        
   
}
