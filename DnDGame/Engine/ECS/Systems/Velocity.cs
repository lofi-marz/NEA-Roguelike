using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems
{
    public static class Velocity
    {
        public static void Update(World world, GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var moveType = typeof(MovementComponent);
            var velocities = world.EntityComponents[moveType];
            foreach (var ec in velocities.ToList())
            {
                var moveComponent = world.GetComponent<MovementComponent>(ec.Key);
                var posComponent = world.GetComponent<TransformComponent>(ec.Key);
                var collisionComponent = world.GetComponent<CollisionPolygon>(ec.Key);
                var velocity = moveComponent.Velocity;
                var drag = moveComponent.Drag;
                var displacement = velocity;
                velocity *= drag * new Vector2(delta, delta);
                moveComponent.Velocity = velocity;
                posComponent.Pos += displacement;
                var cell = world.Sprites.GetCell(posComponent.Pos);
                cell.Remove(ec.Key);
                /*foreach (var entity in cell)
                {
                    if (world.EntityComponents[typeof(CollisionPolygon)].ContainsKey(entity))
                    {
                        var box2 = world.GetComponent<CollisionPolygon>(entity);
                        if (collisionComponent.Translate(velocity * new Vector2(delta)).IsColliding(box2))
                        {
                            return;
                        }
                            

                    }
                }*/

                world.SetComponent(ec.Key, moveComponent);
                world.SetComponent(ec.Key, posComponent);
                
            };

            // Pos += Velocity * new Vector2(delta, delta);
        }

    }
}
