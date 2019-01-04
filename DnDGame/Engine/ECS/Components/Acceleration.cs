using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class AccelerationComponent : Component
    {
        public Vector2 Acceleration;

        public AccelerationComponent(Vector2 acc)
        {
            Acceleration = acc;
        }
    }



}
