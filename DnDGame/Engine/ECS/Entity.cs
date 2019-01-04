using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.ECS
{


    public class Entity
    {
        public int Id;
        public BitArray ComponentFlags;
        public string Group;
        public Entity(int id)
        {
            Id = id;
            ComponentFlags = new BitArray(Enum.GetNames(typeof(ComponentType)).Length);
        }


    }

    

   


 
}
