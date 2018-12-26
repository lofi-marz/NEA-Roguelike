using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Input
{
    public static class Input
    {
        public static Dictionary<GameAction, List<Keys>> DefaultKeyMap = new Dictionary<GameAction, List<Keys>>
            {
                { GameAction.MoveUp, new List<Keys>() {Keys.Up, Keys.W } },
                { GameAction.MoveDown, new List<Keys>() {Keys.Down, Keys.S } },
                { GameAction.MoveLeft, new List<Keys>() {Keys.Left, Keys.A } },
                { GameAction.MoveRight, new List<Keys>() {Keys.Right, Keys.D } },
            };

    }

    public enum GameAction
    {
        MoveUp,
        MoveRight,
        MoveDown,
        MoveLeft,
        PrimaryAction,
        SecondaryAction
    }



    public class InputMap
    {
        public Dictionary<GameAction, List<Keys>> KeyMap;
        
        
        public InputMap(Dictionary<GameAction, List<Keys>> map)
        {
            KeyMap = map;
        }
        
        public InputMap()
        {
            KeyMap = Input.DefaultKeyMap;
        }

        public bool IsActionTriggered(InputHelper input, GameAction action)
        {
            var ActionKeys = KeyMap[action];
            foreach (var key in ActionKeys)
            {
                if (input.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
