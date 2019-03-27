using DnDGame.Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems
{
	public static class Physics
	{

		public static void Update(GameTime gameTime, Rectangle region)
		{
			IEnumerable<int> entitiesToUpdate = World.Instance.GetByTypeAndRegion(region, true, typeof(PhysicsBody));


			foreach (int entity in entitiesToUpdate)
			{
				float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;
				Vector2 vDelta = new Vector2(delta);
				PhysicsBody pBody = World.Instance.GetComponent<PhysicsBody>(entity);
				Transform transform = World.Instance.GetComponent<Transform>(entity);

				//X Axis
				var oldVel = pBody.Velocity;
				var newVel = oldVel + pBody.Acc * delta;
				var oldPos = transform.Pos;
				var displacement = 0.5f * (oldVel + newVel) * delta;
				var newPos = oldPos + displacement;
				//pBody.Acc = newAcc;
				CollisionBox hitbox = World.Instance.GetComponent<CollisionBox>(entity);
				var realAABB = new Rectangle(
					(int)(hitbox.AABB.X + newPos.X),
					(int)(hitbox.AABB.Y + newPos.Y),
					(int)(hitbox.AABB.Width * transform.Scale.X),
					(int)(hitbox.AABB.Height * transform.Scale.Y));
				if (hitbox == null) continue;

				IEnumerable<int> nearbyPotentialCollisions = World.Instance.GetByTypeAndRegion(realAABB, true, typeof(CollisionBox));
				var prevPos = oldPos;
				var AllCollisions = new List<Rectangle>();
				var RealHitbox1 = hitbox.Translate(newPos).Scale(transform.Scale);
				foreach (var entity2 in nearbyPotentialCollisions)
				{
					if (entity == entity2) continue;
					var trans2 = World.Instance.GetComponent<Transform>(entity2);
					var realHitbox2 = World.Instance.GetComponent<CollisionBox>(entity2).Translate(trans2.Pos).Scale(trans2.Scale);
					if (realHitbox2.AABB.Width == 0 && realHitbox2.AABB.Height == 0) continue;
					var RectCollisions = realHitbox2.CheckCollidingBoxes(RealHitbox1);
					if (RectCollisions.Count > 0)
					{
						AllCollisions.AddRange(RectCollisions);
					}

				}

				var OrderedCollisionRects = AllCollisions.OrderBy(rect => ((rect.Center - RealHitbox1.AABB.Center).ToVector2().Length()));
				foreach (var rect in OrderedCollisionRects)
				{
					RealHitbox1 = hitbox.Translate(newPos).Scale(transform.Scale);
					if (!RealHitbox1.AABB.Intersects(rect)) continue;

					Direction collDirection = GetCollisionDirection((rect.Center - RealHitbox1.AABB.Center).ToVector2());
					Rectangle intersect = Rectangle.Intersect(RealHitbox1.AABB, rect);
					if (intersect.Width == 0 && intersect.Height == 0) continue;
					//Console.WriteLine(collDirection);
					switch (collDirection)
					{
						case Direction.South: //Colliding with something below us
							newPos.Y -= intersect.Height;
							newVel.Y = 0;
							break;
						case Direction.North: //Colliding with something above us
							newPos.Y += intersect.Height;
							newVel.Y = 0;
							break;
						case Direction.East: //Colliding with something to the right of us
							newPos.X -= intersect.Width;
							newVel.X = 0;
							break;
						case Direction.West: //Colliding with something to the left of us
							newPos.X += intersect.Width;
							newVel.X = 0;
							break;
					}
				}
				
				transform.Pos = newPos;
				pBody.Velocity = newVel;
				pBody.Acc *= 0.1f;
				pBody.Velocity *= 0.9f;

				if (oldPos != transform.Pos)
				{
					World.Instance.Sprites.Remove(entity, oldPos);
					World.Instance.Sprites.Add(entity, transform.Pos);
				}




				World.Instance.SetComponent(entity, transform);
				World.Instance.SetComponent(entity, pBody);

				/*if (hitbox == null) continue;
                List<int> nearbyPotentialCollisions = World.Instance.GetByTypeAndRegion(nearbyRegion, typeof(Hitbox));

             

                foreach (var entity2 in nearbyPotentialCollisions)
                {
                    if (entity == entity2) continue;
                    var hitbox2 = World.Instance.GetComponent<Hitbox>(entity2);
                    var transform2 = World.Instance.GetComponent<Transform>(entity2);
                    //(CheckCollision(hitbox, transform, hitbox2, transform2))

                }*/

			}
		}


		public static bool CheckCollision(CollisionBox hit1, Transform trans1, CollisionBox hit2, Transform trans2)
		{
			var realHit1 = hit1.Scale(trans1.Scale).Translate(trans1.Pos);
			var realHit2 = hit2.Scale(trans2.Scale).Translate(trans2.Pos);
			return IsColliding(hit1, hit2);
		}

		public static int CheckCollisionSide(CollisionBox hit1, Transform trans1, CollisionBox hit2, Transform trans2)
		{
			var realHit1 = hit1.Scale(trans1.Scale).Translate(trans1.Pos);
			var realHit2 = hit2.Scale(trans2.Scale).Translate(trans2.Pos);
			return 1;
		}

		public static bool IsColliding(CollisionBox hit1, CollisionBox hit2)
		{

			foreach (var box1 in hit1.Boxes)
			{
				foreach (var box2 in hit2.Boxes)
				{
					if (box1.Intersects(box2))
					{
						return true;
					}
				}
			}
			return false;
		}

		/// <summary>
		/// Given a vector, return the direction it is moving the most in.
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		public static Direction GetCollisionDirection(Vector2 vector)
		{
			Vector2[] compass =
			{
				new Vector2(0f, -1f),
				new Vector2(1f, 0f),
				new Vector2(0f, 1f),
				new Vector2(-1f, 0f)
			};
			float maxDot = 0f;
			int bestMatch = 0;
			for (int i = 0; i < 4; i++)
			{
				var dot = Vector2.Dot(vector, compass[i]);
				if (dot > maxDot)
				{
					maxDot = dot;
					bestMatch = i;
				}
			}
			return (Direction)bestMatch;
		}
	}


}
