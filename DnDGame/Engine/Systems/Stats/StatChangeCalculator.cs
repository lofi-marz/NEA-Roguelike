using DnDGame.Engine.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Stats
{
	/// <summary>
	/// Calculates the stat changes that will happen as a result of a hurtbox being attacked.
	/// </summary>
	static class StatChangeCalculator
	{
		public static void Update()
		{
			var hurtEntities = World.Instance.GetEntitiesByType(typeof(HurtQueue));
			foreach (var entity in hurtEntities)
			{
				var hurtQueue = World.Instance.GetComponent<HurtQueue>(entity);
				if (hurtQueue.HittingEntities.Count == 0) continue;
				var hurtChangeQueue = World.Instance.GetComponent<StatChangeQueue>(entity);
				var hurtEntityObject = World.Instance.Entities.Where(e => e.Id == entity).First();
				
				//Go through each attack and calculate how much damage it should do, then add it to the stat change queue.
				while (hurtQueue.HittingEntities.Count > 0)
				{

					var attackEntity = hurtQueue.HittingEntities.Dequeue();
					if (World.Instance.Entities.Where(e => e.Id == attackEntity).Count() == 0) continue;
					var attackerStats = World.Instance.GetComponent<CharacterStats>(attackEntity);
					if (attackerStats == null) //Check if the attaker itself is the character, or if it has a parent we should get the stats from.
					{
						var parentController = World.Instance.GetComponent<ParentController>(attackEntity);

						if (parentController == null) continue;
						
						attackerStats = World.Instance.GetComponent<CharacterStats>(parentController.ParentId);
					}

					var attackerAttack = attackerStats.CurrentStats["attack"];
					
					hurtChangeQueue.ChangeQueue.Enqueue(new StatChange("health", -2.5f * attackerAttack));
				}
				World.Instance.SetComponent(entity, hurtQueue);
				World.Instance.SetComponent(entity, hurtChangeQueue);
			}
		}
	}
}
