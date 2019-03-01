using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnDGame.Engine.Player;
using static DnDGame.Engine.Systems.Stats.StatsManager;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to store the varying stats for an entity.
	/// </summary>

	public class CharacterStats : Component
	{

		/// <summary>
		/// The characteristics for a character. These are used to influence the stats.
		/// </summary>
		public Dictionary<string, CharLevel> Chars;
		/// <summary>
		/// The values for the stats of an object.
		/// </summary>
		public Dictionary<string, float> CurrentStats { get
			{
				OnStatChange?.Invoke(new StatChangeArgs()
				{
					CurrentStats = _currentStats,
					MaxStats = MaxStats
				});
				return _currentStats;
			}
			set {
				OnStatChange?.Invoke(new StatChangeArgs()
				{
					CurrentStats = _currentStats,
					MaxStats = MaxStats
				});
				_currentStats = value;
			} }
		private Dictionary<string, float> _currentStats;
		/// <summary>
		/// The current max stats for a character. CurrentStats may vary; these are the max values.
		/// </summary>
		public Dictionary<string, float> MaxStats;
		/// <summary>
		/// The function to run on a stat change.
		/// </summary>
		/// <param name="e">StatChangeArgs; The current stats and their max values.</param>
		public delegate void StatChange(StatChangeArgs e);
		public event StatChange OnStatChange;
		public class StatChangeArgs : EventArgs
		{
			public Dictionary<string, float> CurrentStats;
			public Dictionary<string, float> MaxStats;
		}

		/// <summary>
		/// Create a default character of the given race.
		/// </summary>
		/// <param name="race">The race of the character.</param>
		public CharacterStats(Race race)
		{

			switch (race)
			{
				case Race.Human:
					Chars = RaceChars.Human;
					break;
				case Race.Orc:
					Chars = RaceChars.Orc;
					break;
				case Race.Elf:
					Chars = RaceChars.Elf;
					break;
			}

			_currentStats = new Dictionary<string, float>()
			{
				{"health", CalcStats.Health(Chars)},
				{"stamina", CalcStats.Stamina(Chars) },
				{"mana", CalcStats.Mana(Chars) },
				{"dps", CalcStats.DPS(Chars) }
			};
			MaxStats = _currentStats;
			CurrentStats = _currentStats;
		}
	}

	/// <summary>
	/// Given a character's characteristics from its race, calculate its max stats.
	/// </summary>
	static class CalcStats
	{
		const float BASEHEALTH = 100;
		const float BASEMANA = 50;
		const float BASESTAMINA = 50;
		const float BASEDPS = 10;
		/// <summary>
		/// Calculate the maximum health of a character.
		/// </summary>
		/// <param name="Levels">The character's characteristics.</param>
		/// <returns>The calculated max health.</returns>
		public static float Health(Dictionary<string, CharLevel> Levels)
		{
			var health = BASEHEALTH;
			var strValue = (int)Levels["str"] - 2;
			var conValue = (int)Levels["str"] - 2;
			health += (5 * strValue);
			health += (20 * conValue);
			return health;
		}

		/// <summary>
		/// Calculate the maximum mana of a character.
		/// </summary>
		/// <param name="Levels">The character's characteristics.</param>
		/// <returns>The calculated max mana.</returns>
		public static float Mana(Dictionary<string, CharLevel> Levels)
		{
			var mana = BASEMANA;

			var intValue = (int)Levels["int"] - 2;
			var wisValue = (int)Levels["str"] - 2;
			var chaValue = (int)Levels["cha"] - 2;

			mana += (10 * intValue);
			mana += (10 * wisValue);
			mana += (5 * chaValue);
			return mana;
		}

		/// <summary>
		/// Calculate the maximum stamina of a character.
		/// </summary>
		/// <param name="Levels">The character's characteristics.</param>
		/// <returns>The calculated max stamina.</returns>
		public static float Stamina(Dictionary<string, CharLevel> Levels)
		{
			var stamina = BASESTAMINA;

			var strValue = (int)Levels["str"] - 2;
			var conValue = (int)Levels["str"] - 2;

			stamina += (10 * conValue);
			stamina += (5 * strValue);

			return stamina;
		}

		/// <summary>
		/// Calculate the maximum damage per second of a character.
		/// </summary>
		/// <param name="Levels">The character's characteristics.</param>
		/// <returns>The calculated max DPS.</returns>
		public static float DPS(Dictionary<string, CharLevel> levels)
		{
			var dps = BASEDPS;
			var stamina = BASESTAMINA;

			var strValue = (int)levels["str"] - 2;
			var dexValue = (int)levels["dex"] - 2;

			stamina += (dexValue + 2);
			stamina += (2 * strValue);

			return dps;
		}
	}

	/// <summary>
	/// Stores the default characteristics for each race.
	/// </summary>
	static class RaceChars
	{
		public static readonly Dictionary<string, CharLevel> Elf = new Dictionary<string, CharLevel>()
		{
			{ "str", CharLevel.High},
			{ "dex", CharLevel.VeryHigh},
			{ "con", CharLevel.VeryLow},
			{ "int", CharLevel.Average},
			{ "wis", CharLevel.Average},
			{ "cha", CharLevel.Average},
		};

		public static readonly Dictionary<string, CharLevel> Human = new Dictionary<string, CharLevel>()
		{
			{ "str", CharLevel.Low},
			{ "dex", CharLevel.Average},
			{ "con", CharLevel.Low},
			{ "int", CharLevel.VeryHigh},
			{ "wis", CharLevel.High},
			{ "cha", CharLevel.Average},
		};

		public static readonly Dictionary<string, CharLevel> Orc = new Dictionary<string, CharLevel>()
		{
			{ "str", CharLevel.VeryHigh},
			{ "dex", CharLevel.VeryLow},
			{ "con", CharLevel.VeryHigh},
			{ "int", CharLevel.Low},
			{ "wis", CharLevel.Low},
			{ "cha", CharLevel.Low},
		};
	}
}


