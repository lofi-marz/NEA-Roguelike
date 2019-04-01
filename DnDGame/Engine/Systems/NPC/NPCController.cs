using DnDGame.Engine.Components;
using DnDGame.Engine.Systems.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems
{
	/// <summary>
	/// Used for moving followers towards their parents, and  triggering their EnteredRange functions.
	/// </summary>
	public static class NPCController
	{
		public static void Update(GameTime gameTime, Rectangle region)
		{
			var followers = World.Instance.GetByTypeAndRegion(region, true, typeof(Follower));
			foreach (var follower in followers)
			{
				var followerControl = World.Instance.GetComponent<Follower>(follower);
				var followerPos = World.Instance.GetComponent<Transform>(follower).Pos;
				var parentPos = World.Instance.GetComponent<Transform>(followerControl.Parent).Pos;
				var distance = Vector2.Distance(followerPos, parentPos);
				if (distance <= followerControl.MaxRange && distance >= followerControl.MinRange)
				{

					var vectorToParent = (parentPos - followerPos);
					if (!followerControl.inRange)
					{
						followerControl.EnteredRange?.Invoke(follower);
					}
					
					Movement.MoveEntity(follower, vectorToParent);
					var direction = GetFacingDirection(vectorToParent);
					var followerSprite = World.Instance.GetComponent<Sprite>(follower);
					followerSprite.Facing = direction;
					World.Instance.SetComponent(follower, followerSprite);
					followerControl.inRange = true;
					World.Instance.SetComponent(follower, followerControl);
				}
				else
				{
					followerControl.inRange = false;
					World.Instance.SetComponent(follower, followerControl);
				}


			}
		}

		/// <summary>
		/// Use the dot product of a vector with the 4 cardinal directions to calculate the direction it is acting the most in.
		/// </summary>
		public static Direction GetFacingDirection(Vector2 vector)
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
