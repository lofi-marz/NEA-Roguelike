using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	//Hurtboxes and hitboxes are complimentary collision boxes. Hurtboxes will only register collisions with hitboxes, and vice versa.

	/// <summary>
	/// A rectangle used to register collisions with Hitboxes.
	/// </summary>
	public class Hurtbox : IComponent
	{
		/// <summary>
		/// The hurt rectangle to register collisions in.
		/// </summary>
		public Rectangle AABB;
		/// <summary>
		/// The function to run on a collision with a Hitbox.
		/// </summary>
		public ManageHit OnHurt;
	}

	/// <summary>
	/// A rectangle used to register collisions with Hurtboxes.
	/// </summary>
	public class Hitbox : IComponent
	{
		/// <summary>
		/// The hit rectangle to register collisions in.
		/// </summary>
		public Rectangle AABB;
		/// <summary>
		/// The function to run on collision with a Hurtbox.
		/// </summary>
		public ManageHit OnHit;
	}

	/// <summary>
	/// The function to run on a hit.
	/// </summary>
	/// <param name="hitEntity">The id of the entity doing the hitting.</param>
	/// <param name="hurtEntity">The id of the entity being hurt.</param>
	public delegate void ManageHit(int hitEntity, int hurtEntity);

}
