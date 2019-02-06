using DnDGame.Engine.ECS.Components;
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
            List<int> entitiesToUpdate = World.Instance.GetByTypeAndRegion(region, typeof(PhysicsBody));


            foreach (int entity in entitiesToUpdate)
            {
                
                float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;
                Vector2 vDelta = new Vector2(delta);
                PhysicsBody pBody = World.Instance.GetComponent<PhysicsBody>(entity);
                Transform transform = World.Instance.GetComponent<Transform>(entity);

                if (pBody.Force != Vector2.Zero)
                {

                }

                var oldPos = transform.Pos;
                var oldAcc = pBody.Acc;

                transform.Pos += pBody.Velocity * vDelta + (0.5f * oldAcc * vDelta * vDelta);

                var newAcc = pBody.Force * pBody.Mass.InvMass;
                var avgAcc = (oldAcc + newAcc) / 2;

                pBody.Velocity += avgAcc;

                //pBody.Acc = newAcc;
                //Rectangle nearbyRegion = new Rectangle((int)(pos.X - 64), (int)(pos.Y - 64), (int)(pos.X + 128), (int)(pos.Y + 128));
                //Hitbox hitbox = World.Instance.GetComponent<Hitbox>(entity);

                if (oldPos != transform.Pos)
                {
                    World.Instance.Sprites.Remove(entity, oldPos);
                    World.Instance.Sprites.Add(entity, transform.Pos);
                }

                pBody.Force -= pBody.Velocity * 0.5f;
                pBody.Force *= 0.5f;
                World.Instance.SetComponent(entity, transform);
                World.Instance.SetComponent(entity, pBody);

                /*if (hitbox == null) continue;
                List<int> nearbyPotentialCollisions = World.Instance.GetByTypeAndRegion(nearbyRegion, typeof(Hitbox));

                
                transform.Pos.X += (move.OldVelocity.X + move.Velocity.X) * 0.5f * new Vector2(delta).X;

                foreach (var entity2 in nearbyPotentialCollisions)
                {
                    if (entity == entity2) continue;
                    var hitbox2 = World.Instance.GetComponent<Hitbox>(entity2);
                    var transform2 = World.Instance.GetComponent<Transform>(entity2);
                    //(CheckCollision(hitbox, transform, hitbox2, transform2))

                }*/

            }
        }


        public static bool CheckCollision(Hitbox hit1, Transform trans1, Hitbox hit2, Transform trans2)
        {
            var realHit1 = hit1.Scale(trans1.Scale).Translate(trans1.Pos);
            var realHit2 = hit2.Scale(trans2.Scale).Translate(trans2.Pos);
            return IsColliding(hit1, hit2);
        }

        public static int CheckCollisionSide(Hitbox hit1, Transform trans1, Hitbox hit2, Transform trans2)
        {
            var realHit1 = hit1.Scale(trans1.Scale).Translate(trans1.Pos);
            var realHit2 = hit2.Scale(trans2.Scale).Translate(trans2.Pos);
            return 1;
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
