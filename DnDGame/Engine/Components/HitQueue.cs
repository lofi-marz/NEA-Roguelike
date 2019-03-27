using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// Stores a queue of the hitboxes currently hitting the entity's hurtboxes.
	/// </summary>
	public class HurtQueue : IComponent
	{
		public Queue<int> HittingEntities;
		public HurtQueue()
		{
			HittingEntities = new Queue<int>();
		}
	}
}
