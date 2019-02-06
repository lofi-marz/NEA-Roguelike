using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Old.Engine.ECS
{



    public class Movement
    {
        float Mass;
        private Vector2 _velocity;
        public Vector2 Velocity {
            get { return _velocity; }
            set { OldVelocity = _velocity; _velocity = value; }
        }
        public Vector2 OldVelocity;
        public Vector2 Acceleration;
        public Vector2 Drag;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity">Change in displacement</param>
        /// <param name="acc">Change in velocity</param>
        /// <param name="drag">How much velocity will reduce by per frame</param>
        public Movement(float mass, Vector2 velocity, Vector2 acc, Vector2 drag)
        {
            OldVelocity = new Vector2();
            Velocity = velocity; //NB: Max velocity ends up being acc / (1-drag)
            Acceleration = acc;
            Drag = drag;
        }

    }



}
