using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Input
{
    public class PlayerInput
    {
        InputHelper Input;
        public InputMap Map;

        public PlayerInput()
        {
            Input = new InputHelper();
            Map = new InputMap();

        }
        public void Update()
        {
            Input.Update();
            foreach (var binding in Map.KeyMap)
            {
                var keys = binding.Value;
                var pressedKeys = keys.Where(x => Input.IsKeyDown(x)).ToList() ;
                var isActionTriggered = pressedKeys.Count() > 0;
                if (isActionTriggered)
                {
                    var action = binding.Key;
                    Map.ActionMap[action].Invoke();
                }
            }
        }
    }
}
