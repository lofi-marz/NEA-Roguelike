using DnDGame.Engine.Components;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Stats
{

	public static class StatsManager
	{
		public enum CharLevel
		{
			VeryLow,
			Low,
			Average,
			High,
			VeryHigh
		}

		public static void Update(GameTime gameTime)
		{
			/*float delta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
			var entities = World.Instance.GetEntitiesByType(typeof(CharacterStats));
			foreach (var entity in entities)
			{
				var stats = World.Instance.GetComponent<CharacterStats>
			}*/
		}
	}
}
