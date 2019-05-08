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
		public static int Init(int parentId, string type, Direction direction)
		{
			int weaponId;

			string fullName = $"weapon_{type}";
			var parentSprite = World.Instance.GetComponent<Sprite>(parentId);
			var parentTransform = World.Instance.GetComponent<Transform>(parentId);
			var pos = parentTransform.Pos;
			var AABB = TilesetManager.Tilesets["dungeon"].GetCollisionBox(fullName).AABB;
			var parentTile = TilesetManager.Tilesets[parentSprite.SpriteSheet].GetSprite(parentSprite.Tile);

			var hitbox = new Hitbox()
			{
				AABB = AABB,
				OnHit = (int hit, int hurt) => {

				}
			};

			float rot = 0f;
			Vector2 offset = new Vector2(); //Make the sword face the attackee. Need to translate it a bit to keep it at the centre.
			switch (parentSprite.Facing)
			{
				case Direction.North:

					break;
				case Direction.East:
					offset = new Vector2(parentTile.Width*1.5f, parentTile.Height * 0.7f); 
					rot = (float)Math.PI / 2f;
					break;
				case Direction.South:
					break;
				case Direction.West:
					offset = new Vector2(-parentTile.Width * 0.5f, parentTile.Height * 0.7f);
					rot = (float)Math.PI * 1.5f;
					break;
				case Direction.None:
					break;
				default:
					break;
			}
			weaponId = World.Instance.CreateEntity(
				new Transform(pos, new Vector2(1f), AnchorPoint.Centre, rot),
				new ParentController(parentId, offset),
				new Sprite("dungeon", fullName),
				new PhysicsBody(new Vector2(2000f)),
				new LifeTimer(1000f),
				hitbox);
			World.Instance.SpriteHash.Add(weaponId, pos);
			return weaponId;
		}
	}
}
