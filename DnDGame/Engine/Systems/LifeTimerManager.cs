using DnDGame.Engine.Components;
using Microsoft.Xna.Framework;


namespace DnDGame.Engine.Systems
{
	public static class LifeTimerManager
	{
		public static void Update(GameTime gameTime)
		{
			var entities = World.Instance.GetEntitiesByType(typeof(LifeTimer));
			foreach (var entity in entities)
			{
				var timer = World.Instance.GetComponent<LifeTimer>(entity);
				if (!timer.Active) continue;
				timer.ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
				if (timer.ElapsedTime >= timer.Lifespan)
				{
					World.Instance.DestroyEntity(entity);
				};
			}
		}

	}
}
