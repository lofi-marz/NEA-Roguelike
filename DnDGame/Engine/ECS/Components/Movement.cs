using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{



    public class Movement : Component
    {
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public Vector2 Drag;
        public Movement(Vector2 velocity, Vector2 acc, Vector2 drag)
        {
            Velocity = velocity;
            Acceleration = acc;
            Drag = drag;
        }

    }



}
