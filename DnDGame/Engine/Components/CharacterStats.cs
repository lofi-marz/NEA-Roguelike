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
	/// A component to store the varying stats
	/// </summary>
	/// 

	public class CharacterStats : Component
	{
		public Dictionary<string, StatLevel> Chars;
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
		public Dictionary<string, float> MaxStats;
		public delegate void StatChange(StatChangeArgs e);
		public event StatChange OnStatChange;
		public class StatChangeArgs : EventArgs
		{
			public Dictionary<string, float> CurrentStats;
			public Dictionary<string, float> MaxStats;
		}
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

	static class CalcStats
	{
		const float BASEHEALTH = 100;
		const float BASEMANA = 50;
		const float BASESTAMINA = 50;
		const float BASEDPS = 10;
		public static float Health(Dictionary<string, StatLevel> Levels)
		{
			var health = BASEHEALTH;
			var strValue = (int)Levels["str"] - 2;
			var conValue = (int)Levels["str"] - 2;
			health += (5 * strValue);
			health += (20 * conValue);
			return health;
		}

		public static float Mana(Dictionary<string, StatLevel> Levels)
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

		public static float Stamina(Dictionary<string, StatLevel> Levels)
		{
			var stamina = BASESTAMINA;

			var strValue = (int)Levels["str"] - 2;
			var conValue = (int)Levels["str"] - 2;

			stamina += (10 * conValue);
			stamina += (5 * strValue);

			return stamina;
		}

		public static float DPS(Dictionary<string, StatLevel> levels)
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

	static class RaceChars
	{
		public static readonly Dictionary<string, StatLevel> Elf = new Dictionary<string, StatLevel>()
		{
			{ "str", StatLevel.High},
			{ "dex", StatLevel.VeryHigh},
			{ "con", StatLevel.VeryLow},
			{ "int", StatLevel.Average},
			{ "wis", StatLevel.Average},
			{ "cha", StatLevel.Average},
		};

		public static readonly Dictionary<string, StatLevel> Human = new Dictionary<string, StatLevel>()
		{
			{ "str", StatLevel.Low},
			{ "dex", StatLevel.Average},
			{ "con", StatLevel.Low},
			{ "int", StatLevel.VeryHigh},
			{ "wis", StatLevel.High},
			{ "cha", StatLevel.Average},
		};

		public static readonly Dictionary<string, StatLevel> Orc = new Dictionary<string, StatLevel>()
		{
			{ "str", StatLevel.VeryHigh},
			{ "dex", StatLevel.VeryLow},
			{ "con", StatLevel.VeryHigh},
			{ "int", StatLevel.Low},
			{ "wis", StatLevel.Low},
			{ "cha", StatLevel.Low},
		};
	}
}


