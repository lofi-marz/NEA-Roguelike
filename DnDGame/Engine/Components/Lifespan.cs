using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	public class LifeTimer : Component
	{
		public float Lifespan;
		public int ElapsedTime;
		public bool Active;
		public LifeTimer(float lifespan)
		{
			Lifespan = lifespan;
			ElapsedTime = 0;
		}
	}
}
