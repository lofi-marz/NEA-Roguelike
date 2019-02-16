using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Drawing
{
    public interface IObject
    {

        Vector2 Pos { get; set; }

        void Update(GameTime gameTime);

    }

    public class GameObject : IObject
    {
        public Vector2 Pos { get; set; }
        public Vector2 Scale;

        public GameObject()
        {
            Pos = new Vector2(0f);
            Scale = new Vector2(1f);
        }

        public virtual void Update(GameTime gameTime) { }
            


    }

    public class KinematicObject : GameObject
    {
        public float ACC = 150f;
        public float DRAG = 0.5f;
        public Vector2 Velocity;

        public  void UpdateVelocity(GameTime gameTime)
        {
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Pos += Velocity * new Vector2(delta, delta);

        }
    }

}
