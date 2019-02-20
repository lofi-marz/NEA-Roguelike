using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to store the varying stats
	/// </summary>
	public class CharacterStats : Component
	{
		public float Health;
		public float Mana;
		public float Stamina;
		public float Level;
		public float XP;
	}
}
