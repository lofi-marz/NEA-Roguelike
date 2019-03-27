using DnDGame.Engine;
using DnDGame.Engine.Components;
using DnDGame.Engine.Player;
using DnDGame.Engine.Systems.Drawing;
using DnDGame.Engine.Systems.MazeGen;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Initialization
{
	/// <summary>
	/// Creates a player character in the world.
	/// 
	/// </summary>
	static class CreatePlayer
	{
		/// <summary>
		/// Initialze a player with a given start position and a given player character class, which provides all the infomration for creating the character.
		/// </summary>
		/// <param name="startPos"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public static int Init(Vector2 startPos, PlayerCharacter player)
		{
			int playerId;
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0").AABB;
			AABB.Inflate(1.1f, 1.1f);
			var hurtBox = new Hurtbox()
			{
				AABB = AABB,
				OnHurt = (int hit, int hurt) =>
				{
					var MyHurtQueue = World.Instance.GetComponent<HurtQueue>(hurt);
					MyHurtQueue.HittingEntities.Enqueue(hit);
				}
			};

			playerId = World.Instance.CreateEntity(
				new Transform(new Vector2(-64f, -32f), new Vector2(1f)),
				new Sprite("dungeon", $"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0", 0.5f),
				new PhysicsBody(new Vector2(2000f)),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0"),
				new AnimationPlayer("dungeon", $"{player.GetClassName()}_{player.GetGenderName()}_idle", $"{player.GetClassName()}_{player.GetGenderName()}_idle"),
				new CharacterStats((Race)Enum.Parse(typeof(Race), player.GetClassName(), true)),
				hurtBox);
			World.Instance.Sprites.Add(playerId, startPos);
			return playerId;
		}

		





	}
}
