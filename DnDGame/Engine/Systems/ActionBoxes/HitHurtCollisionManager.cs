using DnDGame.Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems
{
	public static class HitHurtCollisionManager
	{




	
		public static void Update(GameTime gameTime, Rectangle region)
		{
			var Hitboxes = World.Instance.GetByTypeAndRegion(region, true, typeof(Hitbox))
				.Select(e => 
				(hitEntity: e, 
				hitbox: World.Instance.GetComponent<Hitbox>(e),
				hittransform: World.Instance.GetComponent<Transform>(e)
				));

			foreach (var (hitEntity, hitbox, hitTransform) in Hitboxes)
			{
				var realHitboxAABB = new Rectangle(
					(int)(hitbox.AABB.X + hitTransform.Pos.X), 
					(int)(hitbox.AABB.Y + hitTransform.Pos.Y),
					(int)(hitbox.AABB.Width * hitTransform.Scale.X),
					(int)(hitbox.AABB.Height * hitTransform.Scale.Y));

				var nearbyRegion = realHitboxAABB;
				var nearbyHurtboxes = World.Instance.GetByTypeAndRegion(nearbyRegion, true, typeof(Hurtbox))
					.Select(e =>
					(hurtEntity: e,
					hurtbox: World.Instance.GetComponent<Hurtbox>(e),
					hurttransform: World.Instance.GetComponent<Transform>(e)
					));
				foreach (var (hurtentity, hurtbox, hurttransform) in nearbyHurtboxes)
				{

					var realHurtboxAABB = new Rectangle(
						(int)(hurtbox.AABB.X + hurttransform.Pos.X),
						(int)(hurtbox.AABB.Y + hurttransform.Pos.Y),
						(int)(hurtbox.AABB.Width * hurttransform.Scale.X),
						(int)(hurtbox.AABB.Height * hurttransform.Scale.Y));
					if (realHitboxAABB.Intersects(realHurtboxAABB))
					{
						hitbox.OnHit(hitEntity, hurtentity);
						hurtbox.OnHurt(hitEntity, hurtentity);
						
					}
				}

			}
		}
	}


}
