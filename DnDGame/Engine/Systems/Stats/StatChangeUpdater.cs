using DnDGame.Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Stats
{
	//Apply any stat changes, i.e. as a result of StatChangeCalculator.
	static class StatChangeUpdater
	{
		public static void Update()
		{
			var statChars = World.Instance.GetEntitiesByType(typeof(StatChangeQueue), typeof(CharacterStats));
			foreach (var entity in statChars)
			{
				var statQueueComp = World.Instance.GetComponent<StatChangeQueue>(entity);
				if (statQueueComp.ChangeQueue.Count == 0) continue;
				var charStats = World.Instance.GetComponent<CharacterStats>(entity);
				while (statQueueComp.ChangeQueue.Count > 0) //Go through all of the stat changes and apply them.
				{
					var currentChange = statQueueComp.ChangeQueue.Dequeue();
					charStats.CurrentStats[currentChange.Stat] += currentChange.Change;
				}
				World.Instance.SetComponent(entity, charStats);
			}
		}
	}
}
