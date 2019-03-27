using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{
	/// <summary>
	/// A component used to give an object a static position relative to another object in the world.
	/// </summary>
	public class Parent : IComponent
	{
		/// <summary>
		/// The ID of the parent object to be placed relative to.
		/// </summary>
		public int ParentId;
		/// <summary>
		/// The relative position of the child object to the parent.
		/// </summary>
		public Vector2 Offset;
		public Parent(int parentId)
		{
			ParentId = parentId;
			Offset = new Vector2(0);
		}
	}
}
