using DnDGame.Engine.Systems.Drawing;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Initialization.CreateObjects
{
	static class CreateCell
	{
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
