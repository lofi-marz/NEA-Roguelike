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
                move.Velocity *= move.Drag;
                transform.Pos += move.Velocity * new Vector2(delta);
                World.Instance.SetComponent(entity, move);
                World.Instance.SetComponent(entity, transform);
                if (move.Velocity.Length() > 0)
                {
                    World.Instance.Sprites.Remove(entity, oldPos);
                    World.Instance.Sprites.Add(entity, transform.Pos);
                    //((TransformComponent)World.Instance.EntityComponents[typeof(TransformComponent)][playerid]).Pos = Player.Pos;
                }

                Hitbox hitbox = World.Instance.GetComponent<Hitbox>(entity);
                if (hitbox == null) continue;
                Vector2 pos = transform.Pos;

                Rectangle nearbyRegion = new Rectangle((int)(pos.X - 64), (int)(pos.Y - 64), (int)(pos.X + 128), (int)(pos.Y + 128));
                List<int> nearbyItems = World.Instance.GetByTypeAndRegion(nearbyRegion, typeof(Hitbox));
                foreach (var entity2 in nearbyItems)
                {
                    if (entity == entity2) continue;
                    
                }

            }
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
