using DnDGame.Engine.Components;
using DnDGame.Engine.Systems.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Initialization.CreateObjects
{
	static class CreateWeapon
	{
		static void Init(Vector2 pos, string type, Direction direction)
		{
			int weaponId;
			int parentId;
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox($"type").AABB;
			AABB.Inflate(1.1f, 1.1f);
			var hitbox = new Hitbox()
			{
				AABB = AABB,
			};

			weaponId = World.Instance.CreateEntity(
				new Transform(pos, new Vector2(1f)),
				new Sprite("dungeon", type),
				new PhysicsBody(new Vector2(2000f)),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox($"type"),
				hitbox);
			World.Instance.Sprites.Add(weaponId, pos);
			return playerId;
		}
	}
}
