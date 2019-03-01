using DnDGame.Engine.Components;
using DnDGame.Engine.Player;
using DnDGame.Engine.Systems.Drawing;
using DnDGame.Engine.Systems.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Initialization
{
	public static class InputAssign
	{
		/// <summary>
		/// Assign a movement controller to an entity.
		/// </summary>
		/// <param name="player">The player character to be controlled.</param>
		/// <param name="playerInput">The inputs to be register the actions with.</param>
		public static void AssignMovementController(PlayerCharacter player, ref PlayerController playerInput)
		{
			for (int i = 0; i < 4; i++)
			{
				var iTemp = i;
				playerInput.AddAction((GameAction)iTemp,
					new Action(() =>
					{
						Direction dir = (Direction)iTemp;
						Movement.MoveEntity(player.Entity, (Direction)iTemp);
						AnimationManager.PlayNext(player.Entity, $"{player.GetClassName()}_{player.GetGenderName()}_run");
						//Console.WriteLine("Key press");
					}));
				playerInput.AddAction((GameAction)iTemp,
					new Action(() =>
					{
						Movement.MoveEntity(player.Entity, Direction.None);
						//Console.WriteLine("Key release");
					}),
					ActionType.Release);
			}
		}
	}
}
