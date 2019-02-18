using DnDGame.Engine.ECS;
using DnDGame.Engine.ECS.Components;
using DnDGame.Engine.ECS.Systems.Drawing;
using DnDGame.Engine.ECS.Systems.MazeGen;
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
		public static int Player()
		{
			int playerId;
			playerId = World.Instance.CreateEntity(
				new Transform(new Vector2(-64f, -32f), new Vector2(1f)),
				new Sprite("dungeon", "knight_m_idle_anim_0", 0.5f, 32, 16),
				new PhysicsBody(new Vector2(2000f)),
				TilesetManager.Tilesets["dungeon"].GetSpriteHit("knight_m_idle_anim_0"));
			return playerId;
		}

		public static int DungeonCell(Vector2 pos, string item, float depth)
		{
			int cellEntity = World.Instance.CreateEntity(
				new Transform(pos, new Vector2(1f)),
				new Sprite("dungeon", item.ToString(), depth),
				TilesetManager.Tilesets["dungeon"].GetSpriteHit(item.ToString()));
			return cellEntity;
		}
	}
}
