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
	static class CreatePlayer
	{
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
					var MyStats = World.Instance.GetComponent<CharacterStats>(hurt);
					var HitboxStats = World.Instance.GetComponent<CharacterStats>(hit);
					MyStats.CurrentStats["health"] -= HitboxStats.CurrentStats["dps"] * (1f/60f);
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
