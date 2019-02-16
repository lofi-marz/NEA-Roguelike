using DnDGame.Engine.ECS.Systems.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems
{
    public static class DrawSystem
    {
        public static void Update(SpriteBatch spriteBatch, Rectangle visible)
        {
            
            
            var spriteType = typeof(Sprite);
            var posType = typeof(Transform);
            var entityids = World.Instance.Sprites.GetItems(visible);
            
            //var sprites = entityids.ToDictionary<int, SpriteComponent>(x => (SpriteComponent)World.Instance.GetComponent(x, spriteType)).ToList();
            entityids.Sort((x, y) => World.Instance.GetComponent<Sprite>(x).Depth.CompareTo((World.Instance.GetComponent<Sprite>(y)).Depth));
			entityids.Reverse();
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
            for (int i = 0; i < entityids.Count(); i++)
            {
                var transform = World.Instance.GetComponent<Transform>(entityids[i]);
                var pos = transform.Pos;
                var scale = transform.Scale;
                var sprite = World.Instance.GetComponent<Sprite>(entityids[i]);
                var tileSet = TilesetManager.Tilesets[sprite.SpriteSheet];
                var SpriteSheet = tileSet.SpriteSheet;
                var sourceRect = tileSet.GetSpriteRect(sprite.Tile);
                var destRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(sprite.Width * scale.X), (int)(sprite.Height * scale.Y));
                spriteBatch.Draw(SpriteSheet,
                    destRect,
                    sourceRect, Color.AliceBlue);
            }
        }

        
    }
}
