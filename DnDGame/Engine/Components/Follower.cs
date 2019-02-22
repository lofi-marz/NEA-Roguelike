using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	public class Follower : Component
	{
		public int MaxRange;
		public int MinRange;
		public int Parent;
		public Follower(int max, int min = 5)
		{
			MaxRange = max;
			MinRange = min;
		}
	}
}
