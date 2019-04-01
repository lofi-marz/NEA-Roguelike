using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to store the information needed for an entity to follow another entity.
	/// This differs from the parent component, as this will move the child towards the parent using acceleration, whereas the parent will simply update the position to be the same distance fromm the parent.
	/// </summary>
	public class Follower : IComponent
	{
		/// <summary>
		/// The max range of the follower. The parent must be within this to be visible.
		/// </summary>
		public int MaxRange;
		/// <summary>
		/// The min range of the follower. The parent must be further than this to be visible.
		/// </summary>
		public int MinRange;
		/// <summary>
		/// The id of the parent entity to follow.
		/// </summary>
		public int Parent;
		/// <summary>
		/// The starting position of the follower.
		/// </summary>
		public Vector2 StartPos;
		/// <summary>
		/// The event to invoke when the entity first enters the range of the parent.
		/// </summary>
		public RangeTrigger EnteredRange;
		public delegate void RangeTrigger(int npcid);


		public bool inRange;
		
		public Follower(Vector2 startPos, int max, int min = 10)
		{
			StartPos = startPos;
			MaxRange = max;
			MinRange = min;
		}
	}
}
