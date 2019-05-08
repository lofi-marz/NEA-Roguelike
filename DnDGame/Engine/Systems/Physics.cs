using DnDGame.Engine.Components;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace DnDGame.Engine.Systems
{
	/// <summary>
	/// This system manages collisions within the world between hitboxes, updating them ina  given region.
	/// </summary>
	public static class Physics
	{

		/// <summary>
		/// Calculate and resolve any collisions for the given region.
		/// </summary>
		/// <param name="region">The region to update collisions in.</param>
		public static void Update(GameTime gameTime, Rectangle region)
		{
			//All of the physics bodies in the region
			IEnumerable<int> entitiesToUpdate = World.Instance.GetByTypeAndRegion(region, true, typeof(PhysicsBody));


			foreach (int entity in entitiesToUpdate)
			{
				//Time since the last rame as fractions of a second
				float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;

				PhysicsBody pBody = World.Instance.GetComponent<PhysicsBody>(entity);
				Transform transform = World.Instance.GetComponent<Transform>(entity);

				//Verlet Integration
				var oldVel = pBody.Velocity;
				var newVel = oldVel + pBody.Acc * delta;
				var oldPos = transform.Pos;
				var displacement = 0.5f * (oldVel + newVel) * delta;
				var newPos = oldPos + displacement;

				//Start of collision management
				CollisionBox hitbox = World.Instance.GetComponent<CollisionBox>(entity);
				if (hitbox == null) continue;
				var realAABB = new Rectangle(
					(int)(hitbox.AABB.X + newPos.X),
					(int)(hitbox.AABB.Y + newPos.Y),
					(int)(hitbox.AABB.Width * transform.Scale.X),
					(int)(hitbox.AABB.Height * transform.Scale.Y));

				IEnumerable<int> nearbyPotentialCollisions = World.Instance.GetByTypeAndRegion(realAABB, true, typeof(CollisionBox));

				var prevPos = oldPos;
				var AllCollisions = new List<Rectangle>();
				var RealHitbox1 = hitbox.Translate(newPos).Scale(transform.Scale);
				//Go through all of the CollisonBoxes near that could be colliding
				foreach (var entity2 in nearbyPotentialCollisions)
				{
					if (entity == entity2) continue;
					var trans2 = World.Instance.GetComponent<Transform>(entity2);
					//Scale the hitboxes to their real size and position in the world
					var realHitbox2 = World.Instance.GetComponent<CollisionBox>(entity2).Translate(trans2.Pos).Scale(trans2.Scale);
					if (realHitbox2.AABB.Width == 0 && realHitbox2.AABB.Height == 0) continue;
					//Check if any of the recangles are actually colliding
					var RectCollisions = realHitbox2.CheckCollidingBoxes(RealHitbox1);
					if (RectCollisions.Count > 0)
					{
						AllCollisions.AddRange(RectCollisions);
					}

				}

				//Sort collisions by distance to the CollisionBox
				var OrderedCollisionRects = AllCollisions.OrderBy(rect => (rect.Center - RealHitbox1.AABB.Center).ToVector2().Length());
				foreach (var rect in OrderedCollisionRects)
				{
					RealHitbox1 = hitbox.Translate(newPos).Scale(transform.Scale);
					if (!RealHitbox1.AABB.Intersects(rect)) continue;

					Direction collDirection = GetCollisionDirection((rect.Center - RealHitbox1.AABB.Center).ToVector2());
					Rectangle intersect = Rectangle.Intersect(RealHitbox1.AABB, rect);
					if (intersect.Width == 0 && intersect.Height == 0) continue;
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
				//Drag simulation
				pBody.Acc *= 0.1f;
				pBody.Velocity *= 0.9f;

				if (oldPos != transform.Pos)
				{
					World.Instance.SpriteHash.Remove(entity, oldPos);
					World.Instance.SpriteHash.Add(entity, transform.Pos);
				}




				World.Instance.SetComponent(entity, transform);
				World.Instance.SetComponent(entity, pBody);

			}
		}


		/// <summary>
		/// Given a vector, return the direction it is moving the most in. using the dotproduct with the 4 cardinal directions.
		/// </summary>
		/// <param name="vector">The vector to check the direction in.</param>
		/// <returns>The direction the vector is actingthe most in.</returns>
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
