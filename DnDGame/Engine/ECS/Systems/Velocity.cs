using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems
{
    ///
    public static class Velocity
    {   /// <summary>
        /// For each object with a velocity component,
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var moveType = typeof(Movement);
            var CollisionEntities = World.Instance.GetEntities(typeof(Movement), typeof(Transform));
            foreach (var entity1 in CollisionEntities)
            {
                var moveComponent = World.Instance.GetComponent<Movement>(entity1);
                var transform1 = World.Instance.GetComponent<Transform>(entity1);
                var hitbox1 = World.Instance.GetComponent<Hitbox>(entity1);
                var velocity = moveComponent.Velocity;
                var drag = moveComponent.Drag;
                var displacement = velocity * new Vector2(delta, delta);
                velocity *= drag;
                var pos = transform1.Pos;
                var nearbyRegion = new Rectangle((int)(pos.X - 64), (int)(pos.Y - 64), (int)(pos.X + 128), (int)(pos.Y + 128));
                var nearbyItems = World.Instance.Sprites.GetItems(nearbyRegion);
                
                foreach (var entity in nearbyItems)
                {
                    
                    if (entity == entity1) continue;
                    if (!World.Instance.EntityComponents[typeof(Hitbox)].ContainsKey(entity)) continue;
                    var hitbox2 = World.Instance.GetComponent<Hitbox>(entity);
                    var transform2 = World.Instance.GetComponent<Transform>(entity);
                    var areColliding = TestCollision(hitbox1, hitbox2, transform1, transform2, displacement);
                    if (areColliding)
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
                transform1.Pos += displacement;
                World.Instance.SetComponent(entity1, moveComponent);
                World.Instance.SetComponent(entity1, transform1);
            };

            // Pos += Velocity * new Vector2(delta, delta);
        }
        /// <summary>
        /// Check if a given moving hitbox is about to collide with a given static one
        /// </summary>
        /// <param name="box1">The moving hitbox.</param>
        /// <param name="box2">The static hitbox.</param>
        /// <param name="transform1">The position of the moving hitbox in the world.</param>
        /// <param name="transform2">The position of the static  hitbox in the world.</param>
        /// <param name="displacement">The displacement the moving hitbox is about to undergo.</param>
        /// <returns></returns>
        public static bool TestCollision(Hitbox box1, Hitbox box2, Transform transform1, Transform transform2, Vector2 displacement) 
        {
            var actualBox = box1.Translate(transform1.Pos + displacement).Scale(transform1.Scale);
            var actualBox2 = box2.Translate(transform2.Pos).Scale(transform2.Scale);
            return actualBox.IsColliding(actualBox2);

        }


    }
}
