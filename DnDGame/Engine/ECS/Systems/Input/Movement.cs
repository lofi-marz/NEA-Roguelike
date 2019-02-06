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
            float x = 0f;
            float y = 0f;
            switch (direction)
            {
                case Direction.Up:
                    y = -10f;
                    break;
                case Direction.Down:
                    y = 10f;
                    break;
                case Direction.Left:
                    x = -10f;
                    break;
                case Direction.Right:
                    x = 10f;
                    break;
            }

            pBody.Force = new Vector2(x, y);
            //((MovementComponent)World.Instance.EntityComponents[typeof(MovementComponent)][entityid]).Velocity += acc;
            World.Instance.SetComponent(entityid, pBody);
        }
    }
}
