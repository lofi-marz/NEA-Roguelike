using DnDGame.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DnDGame.Engine.Systems.Drawing
{
	public class Tileset
	{
		const float DEFAULT_ANIM_LENGTH = 0.1f;
		public Texture2D SpriteSheet;
		public int CellSize;
		public Dictionary<string, TilePoints> PointMap;
		public Dictionary<string, Tile> Map;
		public Dictionary<string, List<Tile>> Anims;
		public Dictionary<string, float> AnimLengths;
				
		public void GenTileset()
		{
			Map = new Dictionary<string, Tile>();
			Anims = new Dictionary<string, List<Tile>>();
			AnimLengths = new Dictionary<string, float>();
			foreach (var tilePoint in PointMap)
			{
				var tileName = tilePoint.Key;
				var tileSpritePos = tilePoint.Value.Pos;
				var tileHitPoints = tilePoint.Value.AABB;
				Map[tilePoint.Key] = new Tile
				{
					Sprite = GenSpriteRect(tileSpritePos),
					AABB = (tileHitPoints == null) ? new CollisionBox(new Rectangle(0,0,0,0)) : new CollisionBox(GenSpriteRect(tileHitPoints)) 
				};

				var animPattern = new Regex(".+_anim_[0-9]+");
				var frameSuffixPattern = new Regex("_anim_[0-9]+");
				var frameCountPattern = new Regex("[0-9]+");
				var animName = frameSuffixPattern.Replace(tileName, "");
				if (animPattern.IsMatch(tileName))
				{
					int frameNo = int.Parse(frameCountPattern.Match(tileName).ToString());

					if (frameNo == 0)
					{
						Anims[animName] = new List<Tile>
						{
							new Tile
							{
								Sprite = GetSprite(tileName),
								AABB = GetCollisionBox(tileName)
							}
						};

						AnimLengths[animName] = tilePoint.Value.Length == 0f ? DEFAULT_ANIM_LENGTH : tilePoint.Value.Length;
					}
					else {
						Anims[animName].Add(new Tile
						{
							Sprite = GetSprite(tileName),
							AABB = GetCollisionBox(tileName)
						});
					}
					
				}
			}


		}

		public Rectangle GenSpriteRect(int[] points)
		{
			var sourceRect = new Rectangle(points[0], points[1], points[2], points[3]);
			return sourceRect;
		}

		public CollisionBox GenSpriteHit(int[] points)
		{
			var AABBPoints = points;
			var FullAABBPoints = AABBPoints.Length == 4 ? AABBPoints : new int[] { 0, 0 }.Concat(AABBPoints).ToArray();
			var AABBRect = new Rectangle(FullAABBPoints[0], FullAABBPoints[1], FullAABBPoints[2], FullAABBPoints[3]);
			return new CollisionBox(new List<Rectangle> { AABBRect });
		}

		public Rectangle GetSprite(string tile)
		{
			return Map[tile].Sprite;
		}

		public CollisionBox GetCollisionBox(string tile)
		{
			return Map[tile].AABB;
		}
	}





    

	public class TilePoints
	{
		public int[] Pos;
		public int[] AABB;
		public float Length;
		/*public TilePoints(int[] pos, int[] aabb)
		{
			Pos = pos;
			AABB = aabb;
		}*/

	}

	public class Tile
	{
		public Rectangle Sprite;
		public CollisionBox AABB;
	}

	public class Anim
	{
		public List<Tile> Frames;
	}
}
