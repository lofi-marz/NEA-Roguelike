using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine
{
	/// <summary>
	/// A component used to store the information used to place an object in the game.
	/// </summary>
    public class Transform : Component
    {
		/// <summary>
		/// The current position of the object in the world.
		/// </summary>
		public Vector2 Pos;
		/// <summary>
		/// The current scale of the object relative to the world.
		/// </summary>
		public Vector2 Scale;



        public Transform(Vector2 pos, Vector2 scale)
        {

            Pos = pos;
            Scale = scale;

        }

    }



}
