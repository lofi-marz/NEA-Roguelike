﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems.Input
{
    public static class Movement
    {
        public static void MoveEntity(World world, int entityid, Direction direction)
        {
            var movement = world.GetComponent<MovementComponent>(entityid);
            float x = 0f;
            float y = 0f;
            switch (direction)
            {
                case Direction.Up:
                    y = -movement.Acceleration.Y;
                    break;
                case Direction.Down:
                    y = movement.Acceleration.Y;
                    break;
                case Direction.Left:
                    x = -movement.Acceleration.X;
                    break;
                case Direction.Right:
                    x = movement.Acceleration.X;
                    break;
            }
            var acc = new Vector2(x, y);
            movement.Velocity += acc;
            //((MovementComponent)world.EntityComponents[typeof(MovementComponent)][entityid]).Velocity += acc;
            world.SetComponent(entityid, movement);
        }
    }
}
