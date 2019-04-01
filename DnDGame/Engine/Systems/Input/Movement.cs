using DnDGame.Engine.Components;
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
		/// <summary>
		/// Move the given entity in the given direciton.
		/// </summary>
		/// <param name="entity">The given entity.</param>
		/// <param name="direction">The direction to move them in.</param>
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

                    break;

            }
			
            pBody.Acc = new Vector2(x, y);
            World.Instance.SetComponent(entity, pBody);
        }

		/// <summary>
		/// Move the given entity in the direction of the given vector.
		/// </summary>
		/// <param name="entity">The entity to move.</param>
		/// <param name="direction">The vector to move along.</param>
		public static void MoveEntity(int entity, Vector2 direction)
		{
			PhysicsBody pBody = World.Instance.GetComponent<PhysicsBody>(entity);
			Sprite sprite = World.Instance.GetComponent<Sprite>(entity);
			Vector2 PushForce = pBody.DefaultAcc;
			float x = pBody.Acc.X;
			float y = pBody.Acc.Y;
			direction.Normalize();
			Vector2 move = pBody.DefaultAcc * direction;
			pBody.Acc = move;
			World.Instance.SetComponent(entity, pBody);
		}


		
    }
}
