using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Components
{

	public class Hurtbox : Component
	{
		public Rectangle AABB;
		public ManageHit OnHurt;
	}

	public class Hitbox : Component
	{
		public Rectangle AABB;
		public ManageHit OnHit;
	}

	public delegate void ManageHit(int hitEntity, int hurtEntity);

}
