using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DnDGame.Engine
{
  

    public enum ComponentType
    {
        Position,
        Velocity,
        Drag,
        Acceleration,
        Sprite,
    }

    public class Component
    {

    }



    public static class ComponentUtils
    {
        public static ComponentType GetEnum(Type componentType)
        {
            var typeName = componentType.ToString().Replace("Component", "");
            Enum.TryParse(typeName, out ComponentType type);
            return type;
        }
    }
}
