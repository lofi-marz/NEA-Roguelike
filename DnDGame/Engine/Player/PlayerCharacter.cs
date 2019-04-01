using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Player
{
	public enum Gender
	{
		Male,
		Female
	}

	public enum Class
	{
		Elf,
		Knight,
		Wizard
	}


	public enum Race
	{
		Human,
		Orc,
		Elf
	}


	/// <summary>
	/// Store the information required for a player character
	/// </summary>
	public class PlayerCharacter
	{
		public int Entity;
		public string Weapon;
		public Class Class;
		public Race Race;
		public Gender Gender;
		public PlayerCharacter(Class pclass, Race race, Gender gender, string weapon = "knife")
		{
			Class = pclass;
			Race = race;
			Gender = gender;
			Weapon = weapon;
		}

		public string GetClassName()
		{
			return Class.ToString().ToLower();
		}

		public char GetGenderName()
		{
			return Gender.ToString().ToLower()[0];
		}
	}
}
