using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine
{
	/// <summary>
	/// An entity is a simple object which components are assigned to. By itself it is simply a unique identifier and a group identifier.
	/// </summary>
    public struct Entity
    {
        public int Id;
        public string Group;
        public Entity(int id, string group = null)
        {
            Id = id;
			Group = group;
        }
    }

}
