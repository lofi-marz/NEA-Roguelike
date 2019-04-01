using DnDGame.Engine.Components;
using DnDGame.Engine.Systems.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace DnDGame.Engine.Systems
{
	/// <summary>
	/// System for drawing sprites in a given region.
	/// </summary>
    public static class SpriteDrawSystem
    {
		/// <summary>
		/// Draw all of the sprites in the given region.
		/// </summary>
		/// <param name="spriteBatch">The spritebatch to draw to.</param>
		/// <param name="visible">The region to draw in.</param>
        public static void Update(ref SpriteBatch spriteBatch, Rectangle visible)
        {

            var spriteType = typeof(Sprite);
            var posType = typeof(Transform);
            var entityids = World.Instance.SpriteHash.GetItems(visible, true);
			var spriteEntitys = World.Instance.GetByTypeAndRegion(visible, true, typeof(Sprite));

			///Get all of the sprites, sort them by depth, so lower depths are drawn first.
			var entitySprites = spriteEntitys.Select(e =>
			{
				(int entity, Sprite sprite) es = (e, World.Instance.GetComponent<Sprite>(e));
				return es;
			}).OrderBy(e => e.sprite.Depth);


			foreach (var entitySprite in entitySprites)
			{
				var transform = World.Instance.GetComponent<Transform>(entitySprite.entity);
				var pos = transform.Pos;
				var scale = transform.Scale;
				var sprite = entitySprite.sprite;
				var rotation = transform.Rotation;
				var tileSet = TilesetManager.Tilesets[sprite.SpriteSheet];
				var SpriteSheet = tileSet.SpriteSheet;
				var sourceRect = tileSet.GetSprite(sprite.Tile);
				var origin = new Vector2();

				//Calculate the physical place in the screen to draw the sprite.
				var destRect = new Rectangle(
					(int)Math.Ceiling(pos.X),
					(int)Math.Ceiling(pos.Y), 
					(int)(sourceRect.Width * scale.X), (int)(sourceRect.Height * scale.Y));

				//Set the origin for the sprites. By default it will be the top left point, but some sprites may be centered.
				switch (transform.Anchor)
				{
					case AnchorPoint.Centre:
						origin = new Vector2(destRect.Width * 0.5f, destRect.Height * 0.5f);
						break;
					case AnchorPoint.TopLeft:
						origin = Vector2.Zero;
						break;
				}

				spriteBatch.Draw(SpriteSheet,
					destinationRectangle: destRect,
					sourceRectangle: sourceRect, color: Color.AliceBlue,
					layerDepth: entitySprite.sprite.Depth,
					rotation: rotation,
					origin: origin,
					effects: (sprite.Facing == Direction.East)? SpriteEffects.None:SpriteEffects.FlipHorizontally);
			}
		}
	}
}
