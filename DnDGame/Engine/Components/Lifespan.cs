
namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to give entities a lifespan in milliseconds, after which they will be destroyed.
	/// </summary>
	public class LifeTimer : IComponent
	{
		/// <summary>
		/// The lifespan of the entity in seconds.
		/// </summary>
		public float Lifespan;
		/// <summary>
		/// How far through the lifespan the entity is currently in milliseconds..
		/// </summary>
		public int ElapsedTime;
		/// <summary>
		/// Whether or not the timer is active.
		/// </summary>
		public bool Active;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="lifespan">The lifespan of the entity in milliseconds.</param>
		public LifeTimer(float lifespan)
		{
			Lifespan = lifespan;
			ElapsedTime = 0;
			Active = true;
		}
	}
}
