using DnDGame.Engine;
using DnDGame.Engine.Components;
using DnDGame.Engine.Player;
using DnDGame.Engine.Systems.Drawing;
using DnDGame.Engine.Systems.MazeGen;
using Microsoft.Xna.Framework;
using System;


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
		public static int Init(PlayerCharacter player)
		{
			int playerId;
			Vector2 startPos = player.StartPos;
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0").AABB;
			AABB.Inflate(1.1f, 1.1f);
			var hurtBox = new Hurtbox()
			{
				AABB = AABB,
				OnHurt = (int hit, int hurt) =>
				{
					var hitParent = World.Instance.GetComponent<ParentController>(hit);
					if ((hitParent != null) && hitParent.ParentId == hurt) return;
					var MyHurtQueue = World.Instance.GetComponent<HurtQueue>(hurt);
					var animPlayer = World.Instance.GetComponent<AnimationPlayer>(hurt);
					AnimationManager.PlayNext(player.Entity, $"{player.GetClassName()}_{player.GetGenderName()}_hit");
					MyHurtQueue.HittingEntities.Enqueue(hit);
					World.Instance.SetComponent(hurt, MyHurtQueue);
				}
			};

			playerId = World.Instance.CreateEntity(
				new Transform(new Vector2(-64f, -32f), new Vector2(1f)),
				new Sprite("dungeon", $"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0", 0.5f),
				new PhysicsBody(new Vector2(2000f)),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0"),
				new AnimationPlayer("dungeon", $"{player.GetClassName()}_{player.GetGenderName()}_idle", $"{player.GetClassName()}_{player.GetGenderName()}_idle"),
				new CharacterStats((Race)Enum.Parse(typeof(Race), player.GetRaceName(), true)),
				new HurtQueue(),
				new StatChangeQueue(),
				hurtBox);
			
			World.Instance.SpriteHash.Add(playerId, startPos);
			return playerId;
		}

		





	}
}
