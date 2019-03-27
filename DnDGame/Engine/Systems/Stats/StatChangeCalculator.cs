using DnDGame.Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Stats
{
	static class StatChangeCalculator
	{
		static void Update()
		{
			var hurtEntities = World.Instance.GetEntitiesByType(typeof(HurtQueue));
			foreach (var entity in hurtEntities)
			{
				var hurtQueue = World.Instance.GetComponent<HurtQueue>(entity);
				if (hurtQueue.HittingEntities.Count == 0) continue;
				while (hurtQueue.HittingEntities.Count > 0)
				{
					var attackEntity = hurtQueue.HittingEntities.Dequeue();
					var attackerStats = World.Instance.GetComponent<CharacterStats>(attackEntity);

				}
			}
		}
	}
}
