using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	public enum AnchorPoint
	{
		Centre,
		TopLeft
	}
	/// <summary>
	/// A component used to store the information used to place an object in the game.
	/// </summary>
	
    public class Transform : IComponent
    {
		/// <summary>
		/// The current position of the object in the world.
		/// </summary>
		public Vector2 Pos;
		/// <summary>
		/// The current scale of the object relative to the world.
		/// </summary>
		public Vector2 Scale;

		/// <summary>
		/// The rotation of the sprite in radians.
		/// </summary>
		public float Rotation;

		/// <summary>
		/// Where the entity will be positioned from.
		/// </summary>
		public AnchorPoint Anchor;


		public Transform(Vector2 pos, Vector2 scale, AnchorPoint anchor = AnchorPoint.TopLeft, float rotation = 0f)
        {

            Pos = pos;
            Scale = scale;
			Rotation = rotation;
			Anchor = anchor;
        }

    }



}
