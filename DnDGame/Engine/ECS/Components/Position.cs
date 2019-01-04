using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class TransformComponent : Component
    {
        public Vector2 Pos;

        public TransformComponent(Vector2 pos)
        {
            Pos = pos;
        }
    }



}
