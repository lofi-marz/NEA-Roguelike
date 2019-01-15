﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems
{
    public static class Velocity
    {
        public static void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var moveType = typeof(Movement);
            var velocities = World.Instance.EntityComponents[moveType];
            foreach (var ec in velocities.ToList())
            {
                var moveComponent = World.Instance.GetComponent<Movement>(ec.Key);
                var transformComponent = World.Instance.GetComponent<TransformComponent>(ec.Key);
                var collisionComponent = World.Instance.GetComponent<Hitbox>(ec.Key);
                var velocity = moveComponent.Velocity;
                var drag = moveComponent.Drag;
                var displacement = velocity * new Vector2(delta, delta);
                velocity *= drag;
                var pos = transformComponent.Pos;
                var nearbyRegion = new Rectangle((int)(pos.X - 64), (int)(pos.Y - 64), (int)(pos.X + 128), (int)(pos.Y + 128));
                var nearbyItems = World.Instance.Sprites.GetItems(nearbyRegion);
                
                foreach (var entity in nearbyItems)
                {
                    
                    if (entity == ec.Key) continue;
                    if (!World.Instance.EntityComponents[typeof(Hitbox)].ContainsKey(entity)) continue;

                    var displacedPoly = collisionComponent.Translate(transformComponent.Pos + displacement);
                    var actualPoly = displacedPoly.Scale(transformComponent.Scale);

                    var poly2 = World.Instance.GetComponent<Hitbox>(entity);
                    var trans2 = World.Instance.GetComponent<TransformComponent>(entity);
                    var actualPoly2 = poly2.Translate(trans2.Pos).Scale(trans2.Scale);

                    if (actualPoly.IsColliding(actualPoly2))
                    {
                        Console.WriteLine("Collision");
                        velocity = velocity * new Vector2(-0.1f);
                        displacement = displacement * new Vector2(0);
                        break;
                    }
                }
                if (velocity.Length() < 0.01f) velocity = Vector2.Zero;
                moveComponent.Velocity = velocity;
                //Console.WriteLine(velocity.ToString());
                transformComponent.Pos += displacement;
                World.Instance.SetComponent(ec.Key, moveComponent);
                World.Instance.SetComponent(ec.Key, transformComponent);
            };

            // Pos += Velocity * new Vector2(delta, delta);
        }

    }
}