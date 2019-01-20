﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{

    public class Transform : Component
    {
        public Vector2 Pos;
        public Vector2 Scale;
        public Transform(Vector2 pos, Vector2 scale)
        {
            Pos = pos;
            Scale = scale;
        }
        public Transform(Vector2 pos)
        {
            Pos = pos;
            Scale = new Vector2(1f);
        }
    }



}
