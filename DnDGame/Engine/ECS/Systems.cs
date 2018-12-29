using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{
    public class ECSSystem
    {


    }

    public class UpdateSystem : ECSSystem
    {
        public virtual void Update(World world,  GameTime gameTime)
        {

        }

    }

    public class DrawSystem : ECSSystem
    {
        public virtual void Draw(World world, SpriteBatch spriteBatch)
        {
            var spriteType = typeof(SpriteComponent);
            var entities = world.FilterEntities(spriteType);
            foreach (var entity in entities)
            {
                var pos = ((PositionComponent)world.GetComponent(entity, typeof(PositionComponent))).Pos;
                var sprite = ((SpriteComponent)world.GetComponent(entity, spriteType));
                var destRect = new Rectangle((int)pos.X, (int)pos.Y, (int)(sprite.Width * sprite.Scale.X), (int)(sprite.Height * sprite.Scale.Y));
                spriteBatch.Draw(sprite.SpriteSheet,
                    destRect,
                    sprite.SourceRect, Color.AliceBlue);
            }

        }
    }


    static class Systems
    {
    

        public class UpdateVelocitySystem : UpdateSystem
        {
            public override void Update(World world, GameTime gameTime) {
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var velocityType = typeof(VelocityComponent);
                var entities = world.FilterEntities(velocityType);
                foreach (var entity in entities)
                {
                    var velocity = ((VelocityComponent)world.GetComponent(entity, typeof(VelocityComponent))).Velocity;
                    var drag = ((DragComponent)world.GetComponent(entity, typeof(DragComponent))).Drag;
                    var displacement = velocity * new Vector2(delta, delta);
                    velocity *= drag;
                    world.SetComponent(entity, new VelocityComponent(velocity));
                };
                
               // Pos += Velocity * new Vector2(delta, delta);
            }

        }
    }

}
