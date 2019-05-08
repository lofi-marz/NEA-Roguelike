using DnDGame.Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems
{
	/// <summary>
	/// Check for any collisions between hitboxes and hurtboxes, and invoke their OnCollision functions if so.
	/// </summary>
	public static class HitHurtCollisionManager
	{
		/// <summary>
		/// Checking for collisions.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="region">The region to check for collisions in.</param>
		public static void Update(GameTime gameTime, Rectangle region)
		{
			var Hitboxes = World.Instance.GetByTypeAndRegion(region, true, typeof(Hitbox))
				.Select(e => 
				(hitEntity: e, 
				hitbox: World.Instance.GetComponent<Hitbox>(e),
				hittransform: World.Instance.GetComponent<Transform>(e)
				)); //Geting all the hitboxes and their transforms in the region.

			foreach (var (hitEntity, hitbox, hitTransform) in Hitboxes)
			{
				bool collChange = false; 
				var realHitboxAABB = new Rectangle( //Translate the hitbox to its real size and scale in the world.
					(int)(hitbox.AABB.X + hitTransform.Pos.X), 
					(int)(hitbox.AABB.Y + hitTransform.Pos.Y),
					(int)(hitbox.AABB.Width * hitTransform.Scale.X),
					(int)(hitbox.AABB.Height * hitTransform.Scale.Y));

				var nearbyRegion = realHitboxAABB;
				var nearbyHurtboxes = World.Instance.GetByTypeAndRegion(nearbyRegion, true, typeof(Hurtbox)) //Check any nearby hurtboxes in a much smaller radius
					.Select(e =>
					(hurtEntity: e,
					hurtbox: World.Instance.GetComponent<Hurtbox>(e),
					hurttransform: World.Instance.GetComponent<Transform>(e)
					)).Where(e =>e.hurtbox != null && e.hurttransform != null);
				foreach (var (hurtentity, hurtbox, hurttransform) in nearbyHurtboxes)
				{

					var realHurtboxAABB = new Rectangle( //Translate the hurtbox to its real size and scale in the world.
						(int)(hurtbox.AABB.X + hurttransform.Pos.X),
						(int)(hurtbox.AABB.Y + hurttransform.Pos.Y),
						(int)(hurtbox.AABB.Width * hurttransform.Scale.X),
						(int)(hurtbox.AABB.Height * hurttransform.Scale.Y));
					if (realHitboxAABB.Intersects(realHurtboxAABB))
					{
						if (!hitbox.HurtingEntities.Contains(hurtentity)) //We only want to trigger a collision when the two boxes begin touching
						{
							hitbox.OnHit(hitEntity, hurtentity);
							hitbox.HurtingEntities.Add(hitEntity);
							collChange = true;
						}
						if (!hurtbox.HittingEntities.Contains(hitEntity))
						{
							hurtbox.OnHurt(hitEntity, hurtentity);
							hurtbox.HittingEntities.Add(hitEntity);
							collChange = true;
						}
					}
					else
					{
						if (hitbox.HurtingEntities.Contains(hurtentity))
						{
							hitbox.HurtingEntities.Remove(hurtentity);
							collChange = true;
						}
						if (hurtbox.HittingEntities.Contains(hitEntity))
						{

							hurtbox.HittingEntities.Remove(hitEntity);
							collChange = true;
						}
					}
					if (collChange)
					{
						World.Instance.SetComponent(hitEntity, hitbox);
						World.Instance.SetComponent(hurtentity, hurtbox);
					}
				}

			}
		}
	}


}
