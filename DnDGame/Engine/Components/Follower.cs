using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to store the information needed for an entity to follow another entity.
	/// </summary>
	public class Follower : Component
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
		public Follower(int max, int min = 5)
		{
			MaxRange = max;
			MinRange = min;
		}
	}
}
