using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to give entities a lifespan, after which they will be destroyed.
	/// </summary>
	public class LifeTimer : Component
	{
		/// <summary>
		/// The lifespan of the entity in seconds.
		/// </summary>
		public float Lifespan;
		/// <summary>
		/// How far through the lifespan the entity is currently in seconds.
		/// </summary>
		public int ElapsedTime;
		/// <summary>
		/// Whether or not the timer is active.
		/// </summary>
		public bool Active;
		public LifeTimer(float lifespan)
		{
			Lifespan = lifespan;
			ElapsedTime = 0;
		}
	}
}
