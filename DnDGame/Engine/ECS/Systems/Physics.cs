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

                //X Axis
                var oldVel = pBody.Velocity;
                var newVel = oldVel + pBody.Acc * delta;
                var oldPos = transform.Pos;
                var newPos = oldPos + 0.5f * (oldVel + newVel) * delta;

                var displacement = newPos - oldPos;
                //pBody.Acc = newAcc;
                Hitbox hitbox = World.Instance.GetComponent<Hitbox>(entity);
                var realAABB = new Rectangle(
                    (int)(hitbox.AABB.X + newPos.X),
                    (int)(hitbox.AABB.Y + newPos.Y),
                    (int)(hitbox.AABB.Width * transform.Scale.X),
                    (int)(hitbox.AABB.Height * transform.Scale.Y));
                if (hitbox == null) continue;
                List<int> nearbyPotentialCollisions = World.Instance.GetByTypeAndRegion(realAABB, typeof(Hitbox));
                foreach (var entity2 in nearbyPotentialCollisions)
                {
                    if (entity == entity2) continue;
                    var hitbox2 = World.Instance.GetComponent<Hitbox>(entity2);
                    var trans2 = World.Instance.GetComponent<Transform>(entity2);
                    var realHitbox1 = hitbox.Translate(newPos).Scale(transform.Scale);
                    var realHitbox2 = hitbox2.Translate(trans2.Pos).Scale(trans2.Scale);
                    var rectCollisions = realHitbox2.CheckCollidingBoxes(realHitbox1);
                    if (rectCollisions.Count() == 0) continue;

                    foreach (var rect in rectCollisions)
                    {
                        if (displacement.X > 0 && realHitbox1.AABB.Right > rect.X)
                        {
                            newPos.X = oldPos.X;
                            newVel.X = 0;
                        }
                        if (displacement.X < 0 && realHitbox1.AABB.X < rect.Right)
                        {
                            newPos.X = oldPos.X;
                            newVel.X = 0;

                        }
                    }
                    var newXHitbox1 = hitbox.Translate(newPos).Scale(transform.Scale);
                    var newXRectCollisions = realHitbox2.CheckCollidingBoxes(newXHitbox1);
                    if (newXRectCollisions.Count() == 0) continue;
                    foreach (var rect in newXRectCollisions)
                    {
                        if (displacement.Y > 0 && realHitbox1.AABB.Bottom > rect.Top)
                        {
                            newPos.Y = oldPos.Y;
                            newVel.Y = 0;
                        }
                        if (displacement.Y < 0 && realHitbox1.AABB.Top < rect.Bottom)
                        {
                            newPos.Y = oldPos.Y;
                            newVel.Y = 0;
                        }
                    }

                }

                transform.Pos = newPos;
                pBody.Velocity = newVel;

                pBody.Velocity *= 0.9f;

                if (oldPos != transform.Pos)
                {
                    World.Instance.Sprites.Remove(entity, oldPos);
                    World.Instance.Sprites.Add(entity, transform.Pos);
                }

                

 
                World.Instance.SetComponent(entity, transform);
                World.Instance.SetComponent(entity, pBody);

                /*if (hitbox == null) continue;
                List<int> nearbyPotentialCollisions = World.Instance.GetByTypeAndRegion(nearbyRegion, typeof(Hitbox));

             

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
