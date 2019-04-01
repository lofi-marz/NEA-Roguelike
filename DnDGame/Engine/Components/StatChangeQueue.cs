using System.Collections.Generic;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// Stores the a queue of stat changes to apply to the entity's CharacterStats component.
	/// </summary>
	public class StatChangeQueue : IComponent
	{
		/// <summary>
		/// THe current queue of changes to apply.
		/// </summary>
		public Queue<StatChange> ChangeQueue;
		public StatChangeQueue()
		{
			ChangeQueue = new Queue<StatChange>();
		}
	}

	/// <summary>
	/// A structure to store the values  needed to change a stat.
	/// </summary>
	public struct StatChange
	{
		/// <summary>
		/// The name of the stat to change.
		/// </summary>
		public string Stat;

		/// <summary>
		/// The value to increment or decrement it by.
		/// </summary>
		public float Change;

		public StatChange(string stat, float change)
		{
			Stat = stat;
			Change = change;
		}
	}
}
