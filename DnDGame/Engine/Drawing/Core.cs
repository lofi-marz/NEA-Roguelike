using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Drawing
{
    public interface IObject
    {

        Vector2 Pos { get; set; }


    }

    public class Object
    {
        public Vector2 Pos { get; set; }
        public Vector2 Scale;
    }

}
