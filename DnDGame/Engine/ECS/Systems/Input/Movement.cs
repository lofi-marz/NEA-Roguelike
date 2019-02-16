using DnDGame.Engine.ECS.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems.Input
{
    public static class Movement
    {
        public static void MoveEntity(int entityid, Direction direction)
        {
            PhysicsBody pBody = World.Instance.GetComponent<PhysicsBody>(entityid);
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
                    break;
                case Direction.East:
                    x = PushForce;
                    break;
                case Direction.None:
                    x = 0f;
                    y = 0f;
                    break;
            }
            pBody.Acc = new Vector2(x, y);
            //((MovementComponent)World.Instance.EntityComponents[typeof(MovementComponent)][entityid]).Velocity += acc;
            World.Instance.SetComponent(entityid, pBody);
        }
    }
}
