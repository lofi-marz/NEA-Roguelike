using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component to store the information required for the simple physics system.
	/// </summary>
    public class PhysicsBody : Component
    {
		/// <summary>
		/// The current velocity of the object.
		/// </summary>
        public Vector2 Velocity;
		/// <summary>
		/// The current acceleration of the object.
		/// </summary>
        public Vector2 Acc;
		/// <summary>
		/// The default acceleration of the object. When moving an object, acceleration will be set to this.
		/// </summary>
        public Vector2 DefaultAcc;
        public PhysicsBody(Vector2 acc)
        {
            Velocity = new Vector2();
            Acc = new Vector2();
            DefaultAcc = acc;
        }
    }



    
}
