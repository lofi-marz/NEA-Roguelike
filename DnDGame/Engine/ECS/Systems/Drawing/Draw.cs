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
        public static void Update(World world, SpriteBatch spriteBatch, Rectangle visible)
        {
            var spriteType = typeof(SpriteComponent);
            var posType = typeof(TransformComponent);
            var entityids = world.Sprites.GetItems(visible);
            
            //var sprites = entityids.ToDictionary<int, SpriteComponent>(x => (SpriteComponent)world.GetComponent(x, spriteType)).ToList();
            entityids.Sort((x, y) => world.GetComponent<SpriteComponent>(x).Depth.CompareTo((world.GetComponent<SpriteComponent>(y)).Depth));
            /*foreach (var entity in entityids)
            {
                //Console.WriteLine(entity);
                var pos = ((TransformComponent)world.GetComponent(entity, posType)).Pos;

                var sprite = ((SpriteComponent)world.GetComponent(entity, spriteType));
                var destRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(sprite.Width * sprite.Scale.X), (int)(sprite.Height * sprite.Scale.Y));
                spriteBatch.Draw(sprite.SpriteSheet,
                    destRect,
                    sprite.SourceRect, Color.AliceBlue);
            }*/
            for (int i = 0; i < entityids.Count(); i++)
            {
                var transform = world.GetComponent<TransformComponent>(entityids[i]);
                var pos = transform.Pos;
                var scale = transform.Scale;
                var sprite = world.GetComponent<SpriteComponent>(entityids[i]);

                var destRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(sprite.Width * scale.X), (int)(sprite.Height * scale.Y));
                spriteBatch.Draw(sprite.SpriteSheet,
                    destRect,
                    sprite.SourceRect, Color.AliceBlue);
            }
        }

        
    }
}
