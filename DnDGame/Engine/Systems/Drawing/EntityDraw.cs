using DnDGame.Engine.Systems.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems
{
    public static class EntityDrawSystem
    {
        public static void Update(ref SpriteBatch spriteBatch, Rectangle visible)
        {

            var spriteType = typeof(Sprite);
            var posType = typeof(Transform);
            var entityids = World.Instance.Sprites.GetItems(visible, true);
			var spriteEntitys = World.Instance.GetByTypeAndRegion(visible, true, typeof(Sprite));
			var entitySprites = spriteEntitys.Select(e =>
			{
				(int entity, Sprite sprite) es = (e, World.Instance.GetComponent<Sprite>(e));
				return es;
			}).OrderBy(e => e.sprite.Depth);

            //var sprites = entityids.ToDictionary<int, SpriteComponent>(x => (SpriteComponent)World.Instance.GetComponent(x, spriteType)).ToList();

			/*foreach (var entity in entityids)
            {
                //Console.WriteLine(entity);
                var pos = ((TransformComponent)World.Instance.GetComponent(entity, posType)).Pos;

                var sprite = ((SpriteComponent)World.Instance.GetComponent(entity, spriteType));
                var destRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(sprite.Width * sprite.Scale.X), (int)(sprite.Height * sprite.Scale.Y));
                spriteBatch.Draw(sprite.SpriteSheet,
                    destRect,
                    sprite.SourceRect, Color.AliceBlue);
            }*/

			foreach (var entitySprite in entitySprites)
			{
				var transform = World.Instance.GetComponent<Transform>(entitySprite.entity);
				var pos = transform.Pos;
				var scale = transform.Scale;
				var sprite = entitySprite.sprite;
				var tileSet = TilesetManager.Tilesets[sprite.SpriteSheet];
				var SpriteSheet = tileSet.SpriteSheet;
				var sourceRect = tileSet.GetSprite(sprite.Tile);
				
				var destRect = new Rectangle(
					(int)Math.Ceiling(pos.X),
					(int)Math.Ceiling(pos.Y), 
					(int)(sourceRect.X * scale.X), (int)(sourceRect.Y * scale.Y));
				spriteBatch.Draw(SpriteSheet,
					destinationRectangle: destRect,
					sourceRectangle: sourceRect, color: Color.AliceBlue, layerDepth:entitySprite.sprite.Depth, effects: (sprite.Facing == Direction.East)? SpriteEffects.None:SpriteEffects.FlipHorizontally);
			}
		

        }

        
    }
}
