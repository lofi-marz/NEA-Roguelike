using DnDGame.Engine.Systems.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Initialization.CreateObjects
{
	/// <summary>
	/// Creates a dungeon cell entity in the world with all of the required components.
	/// This is a static object with a sprite and collision box.
	/// </summary>
	static class CreateCell
	{
		/// <summary>
		/// Initializes a dungeon cell entity.
		/// </summary>
		/// <param name="pos">The position in the world to place the cell.</param>
		/// <param name="item">The type of cel to create.</param>
		/// <param name="depth">The sprite depth of the cell.</param>
		/// <returns>The Id of the entity created.</returns>
		public static int Init(Vector2 pos, string item, float depth)
		{
			int cellEntity = World.Instance.CreateEntity(
				new Transform(pos, new Vector2(1f)),
				new Sprite("dungeon", item.ToString(), depth),
				TilesetManager.Tilesets["dungeon"].GetCollisionBox(item.ToString()));
			return cellEntity;
		}
	}
}
