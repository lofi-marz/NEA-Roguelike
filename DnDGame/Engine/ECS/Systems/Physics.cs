using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Systems
{
    public static class Physics
    {
        
        public static void Update(GameTime gameTime, Rectangle region) {
            List<int> entitiesToUpdate = World.Instance.GetByTypeAndRegion(region, typeof(Movement), typeof(Hitbox));


            foreach (int entity in entitiesToUpdate)
            {
                float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;
                Movement move = World.Instance.GetComponent<Movement>(entity);
                Transform transform = World.Instance.GetComponent<Transform>(entity);
                Vector2 oldPos = transform.Pos;
                Vector2 pos = transform.Pos;
                Rectangle nearbyRegion = new Rectangle((int)(pos.X - 64), (int)(pos.Y - 64), (int)(pos.X + 128), (int)(pos.Y + 128));
                Hitbox hitbox = World.Instance.GetComponent<Hitbox>(entity);


                if (move.Velocity.Length() > 0)
                {
                    World.Instance.Sprites.Remove(entity, oldPos);
                    World.Instance.Sprites.Add(entity, transform.Pos);
                    //((TransformComponent)World.Instance.EntityComponents[typeof(TransformComponent)][playerid]).Pos = Player.Pos;
                }

               
                if (hitbox == null) continue;
                List<int> nearbyPotentialCollisions = World.Instance.GetByTypeAndRegion(nearbyRegion, typeof(Hitbox));


                move.Velocity.X *= move.Drag.X;
                transform.Pos.X += move.Velocity.X * new Vector2(delta).X;
                foreach (var entity2 in nearbyPotentialCollisions)
                {
                    if (entity == entity2) continue;
                    var hitbox2 = World.Instance.GetComponent<Hitbox>(entity2);
                    var transform2 = World.Instance.GetComponent<Transform>(entity2);
                    if (CheckCollision(hitbox, transform, hitbox2, transform2))
                    {
                        Console.WriteLine("Collision");
                        move.Velocity.X = 0;
                        transform.Pos.X -= move.Velocity.X * new Vector2(delta).X;
                    }
                    
                }
                
                World.Instance.SetComponent(entity, move);
                World.Instance.SetComponent(entity, transform);
            }
        }

        public static bool CheckCollision(Hitbox hit1, Transform trans1, Hitbox hit2, Transform trans2)
        {
            var realHit1 = hit1.Scale(trans1.Scale).Translate(trans1.Pos);
            var realHit2 = hit2.Scale(trans2.Scale).Translate(trans2.Pos);
            return IsColliding(hit1, hit2);
        }

        public static bool IsColliding(Hitbox hit1, Hitbox hit2)
        {

            foreach (var box1 in hit1.Boxes)
            {
                foreach (var box2 in hit2.Boxes)
                {
                    if (box1.Intersects(box2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    /*                foreach (var entity2 in nearbyItems)
}*/

}
