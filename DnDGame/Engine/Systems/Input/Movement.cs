﻿using DnDGame.Engine.Components;
using DnDGame.Engine.Systems.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Input
{
    public static class Movement
    {
        public static void MoveEntity(int entity, Direction direction)
        {
            PhysicsBody pBody = World.Instance.GetComponent<PhysicsBody>(entity);
			Sprite sprite = World.Instance.GetComponent<Sprite>(entity);
            float PushForce = pBody.DefaultAcc.X;
            float x = pBody.Acc.X;
            float y = pBody.Acc.Y;
            switch (direction)
            {
                case Direction.North:
                    y = -PushForce;
					
                    break;
                case Direction.South:
                    y = PushForce;
                    break;
                case Direction.West:
                    x = -PushForce;
					sprite.Facing = Direction.West;

					break;
                case Direction.East:
                    x = PushForce;
					sprite.Facing = Direction.East;
					break;
                case Direction.None:
                    x = 0f;
                    y = 0f;
                    break;

            }
			
            pBody.Acc = new Vector2(x, y);

            //((MovementComponent)World.Instance.EntityComponents[typeof(MovementComponent)][entityid]).Velocity += acc;
            World.Instance.SetComponent(entity, pBody);
        }


		
    }
}
