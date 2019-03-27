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
		/// <summary>
		/// Initialize a weapon with the required components
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="type"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		static int Init(int parentId, string type, Direction direction)
		{
			int weaponId;
			var parentSprite = World.Instance.GetComponent<Sprite>(parentId);
			var parentTransform = World.Instance.GetComponent<Transform>(parentId);
			var pos = parentTransform.Pos;
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{type}").AABB;
			AABB.Inflate(1.1f, 1.1f);
			var hitbox = new Hitbox()
			{
				AABB = AABB,
			};

			weaponId = World.Instance.CreateEntity(
				new Transform(pos, new Vector2(1f)),
				new Parent(parentId),
				new Sprite("dungeon", type),
				new PhysicsBody(new Vector2(2000f)),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox($"{type}"),
				hitbox);
			World.Instance.Sprites.Add(weaponId, pos);
			return weaponId;
		}
	}
}
