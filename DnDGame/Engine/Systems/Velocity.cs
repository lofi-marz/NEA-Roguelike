using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems
{
    ///
    public static class Velocity
    {   /// <summary>
        /// For each object with a velocity component,
        /// </summary>
        /// <param name="gameTime"></param>
        public static void Update(GameTime gameTime)
        {

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
        /// 
        public static bool TestCollision(CollisionBox box1, CollisionBox box2, Transform transform1, Transform transform2, Vector2 displacement) 
        {
            var actualBox1 = box1.Translate(transform1.Pos + displacement).Scale(transform1.Scale);
            var actualBox2 = box2.Translate(transform2.Pos).Scale(transform2.Scale);
            return Physics.IsColliding(actualBox1, actualBox2);

        }


    }
}
