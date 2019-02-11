using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS.Components
{
    public class PhysicsBody : Component
    {
        public Mass Mass; //Maybe change to density later
        public Vector2 Velocity;
        public Vector2 Force { get;
            set; }
        public Vector2 Acc;
        public Vector2 DefaultAcc;
        float CoOfRest;
        public PhysicsBody(Vector2 acc)
        {
            Velocity = new Vector2();
            Force = new Vector2();
            Acc = new Vector2();
            DefaultAcc = acc;
        }
    }

    public struct Mass
    {
        public float MassValue;
        public float InvMass;
        public Mass(float mass)
        {
            if (mass < 0) //Using <0 to represent infinite mass
            {
                MassValue = -1;
                InvMass = 0;
                
            } else
            {
                MassValue = mass;
                InvMass = 1 / mass;
            }

        }
    }

    
}
