using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	public class StatChangeQueue : IComponent
	{
		public Queue<StatChange> ChangeQueue;
		public StatChangeQueue()
		{
			ChangeQueue = new Queue<StatChange>();
		}
	}

	public struct StatChange
	{
		public string Stat;
		public float Change;
		public int Source;
		public StatChange(string stat, float change, int source = -1)
		{
			Stat = stat;
			Change = change;
			Source = source;
		}
	}
}
