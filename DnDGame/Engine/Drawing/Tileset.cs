using DnDGame.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace DnDGame.Engine.Drawing
{
	public class Tileset
    {
        public Texture2D SpriteSheet;
        public int CellSize;
        public Dictionary<string, Tile> Map;
		public Dictionary<string, Rectangle> SpriteMap;
		public Dictionary<string, Hitbox> HitboxMap;



        public Rectangle GetSpriteRect(string tile)
        {
            var pos = Map[tile].Pos.Length == 4 ? Map[tile].Pos : Map[tile].Pos.Concat(new int[] { 1, 1 }).ToArray();
            pos = pos.Select(x => x * CellSize).ToArray();
            var sourceRect = new Rectangle(pos[0], pos[1], pos[2], pos[3]);
            return sourceRect;
        }

		public Hitbox GetSpriteHit(string tile)
		{
			var AABBPoints = Map[tile].AABB;
			var FullAABBPoints = AABBPoints.Length == 4 ? AABBPoints : new int[] { 0, 0 }.Concat(AABBPoints).ToArray();
			var AABBRect = new Rectangle(FullAABBPoints[0], FullAABBPoints[1], FullAABBPoints[2], FullAABBPoints[3]);
			return new Hitbox(new List<Rectangle> { AABBRect });
		}
    }

	public class Tile
	{
		public int[] Pos;
		public int[] AABB;
		public Tile(int[] pos, int[] aabb)
		{
			Pos = pos;
			AABB = aabb;
		}

	}
}
