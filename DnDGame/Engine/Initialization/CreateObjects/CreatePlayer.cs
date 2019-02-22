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
	static class CreateObjects
	{
		public static int Player(Vector2 startPos, PlayerCharacter player)
		{
			int playerId;
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0").AABB;
			AABB.Inflate(1.1f, 1.1f);
			var hurtBox = new Hurtbox()
			{
				AABB = AABB,
				OnHurt = (int hit, int hurt) =>
				{
					Console.WriteLine($"{hit} hurt");
				}
			};

			playerId = World.Instance.CreateEntity(
				new Transform(new Vector2(-64f, -32f), new Vector2(1f)),
				new Sprite("dungeon", $"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0", 0.5f, 32, 16),
				new PhysicsBody(new Vector2(2000f)),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{player.GetClassName()}_{player.GetGenderName()}_idle_anim_0"),
				new AnimationPlayer("dungeon", $"{player.GetClassName()}_{player.GetGenderName()}_idle", $"{player.GetClassName()}_{player.GetGenderName()}_idle"),
				hurtBox);
			World.Instance.Sprites.Add(playerId, startPos);
			return playerId;
		}

		

		public static int DungeonCell(Vector2 pos, string item, float depth)
		{
			int cellEntity = World.Instance.CreateEntity(
				new Transform(pos, new Vector2(1f)),
				new Sprite("dungeon", item.ToString(), depth),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox(item.ToString()));
			return cellEntity;
		}


		public static int NPC(Vector2 startPos, string type)
		{
			var defaultSprite = type + "_idle_anim_0";
			var defaultAnim = type + "_idle";
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox(defaultSprite).AABB;
			AABB.Inflate(1.1f, 1.1f);
			var hitBox = new Hitbox()
			{
				AABB = AABB,
				OnHit = (int hit, int hurt) =>
				{
					Console.WriteLine($"{hit} hit");
				}
			};
			int npcid = World.Instance.CreateEntity(
				new Transform(startPos, new Vector2(1f)),
				new Sprite("dungeon", defaultSprite, 0.5f),
				new PhysicsBody(new Vector2(1000f)),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox(defaultSprite),
				new AnimationPlayer("dungeon", defaultAnim, defaultAnim),
				new Follower(100),
				hitBox
				);
			return npcid;
		}
	}
}
