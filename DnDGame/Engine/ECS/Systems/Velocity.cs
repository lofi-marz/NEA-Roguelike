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
                var transformComponent = world.GetComponent<TransformComponent>(ec.Key);
                var collisionComponent = world.GetComponent<CollisionPolygon>(ec.Key);
                var velocity = moveComponent.Velocity;
                var drag = moveComponent.Drag;
                var displacement = velocity * new Vector2(delta, delta);
                velocity *= drag;
                var pos = transformComponent.Pos;
                var nearbyRegion = new Rectangle((int)(pos.X - 64), (int)(pos.Y - 64), (int)(pos.X + 128), (int)(pos.Y + 128));
                var nearbyItems = world.Sprites.GetItems(nearbyRegion);
                
                foreach (var entity in nearbyItems)
                {
                    
                    if (entity == ec.Key) continue;
                    if (!world.EntityComponents[typeof(CollisionPolygon)].ContainsKey(entity)) continue;

                    var displacedPoly = collisionComponent.Translate(transformComponent.Pos + displacement);
                    var actualPoly = displacedPoly.Scale(transformComponent.Scale);

                    var poly2 = world.GetComponent<CollisionPolygon>(entity);
                    var trans2 = world.GetComponent<TransformComponent>(entity);
                    var actualPoly2 = poly2.Translate(trans2.Pos).Scale(trans2.Scale);

                    if (actualPoly.IsColliding(actualPoly2))
                    {
                        Console.WriteLine("Collision");
                        velocity = Vector2.Zero;
                        displacement = Vector2.Zero;
                        break;
                    }
                }
                if (velocity.Length() < 0.01f) velocity = Vector2.Zero;
                moveComponent.Velocity = velocity;
                //Console.WriteLine(velocity.ToString());
                transformComponent.Pos += displacement;
                world.SetComponent(ec.Key, moveComponent);
                world.SetComponent(ec.Key, transformComponent);
            };

            // Pos += Velocity * new Vector2(delta, delta);
        }

    }
}
