using DnDGame.Engine.Components;
using DnDGame.Engine.Player;
using DnDGame.Engine.Systems.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Initialization.CreateObjects
{
	static class CreateNPC
	{
		public static int Init(Vector2 startPos, string type)
		{
			var defaultSprite = type + "_idle_anim_0";
			var defaultAnim = type + "_idle";
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox(defaultSprite).AABB;
			AABB.Inflate(1.1f, 1.1f);
			var CharacterStats = new CharacterStats(Race.Orc);
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
				CharacterStats,
				hitBox
				);
			return npcid;
		}
	}
}
