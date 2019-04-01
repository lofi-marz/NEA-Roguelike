using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Input
{

    public enum ActionType
    {
        Down,
        Press,
        Release
    }
	/// <summary>
	/// A class to store all of the components required for controling a player character with keyboard inputs.
	/// </summary>
    public class PlayerController
    {
        KeyboardManager Input;
        public InputMap Map;

        public PlayerController()
        {
            Input = new KeyboardManager();
            Map = new InputMap();
        }

		/// <summary>
		/// Add an action to the player character.
		/// </summary>
        public void AddAction(GameAction gameAction, Action action, ActionType type = ActionType.Down)
        {
            switch (type)
            {
                case ActionType.Down:
                    Map.ActionDownMap[gameAction] = action;
                    break;
                case ActionType.Press:
                    Map.ActionPressedMap[gameAction] = action;
                    break;
                case ActionType.Release:
                    Map.ActionReleasedMap[gameAction] = action;
                    break;
            }

        }


		/// <summary>
		/// Update the KeyboardManager and check if any actions have been triggered.
		/// </summary>
        public void Update()
        {
            Input.Update();
            CheckKeysDown();
        }

        public void CheckKeysDown()
        {
            foreach (var binding in Map.KeyMap)
            {
                var keys = binding.Value;
                var action = binding.Key;
                var downKeys = keys.Where(x => Input.IsKeyDown(x)).ToList();
                var isActionDown = downKeys.Count() > 0;
                if (isActionDown)
                {
                    Map.InvokeAction(ActionType.Down, action);
                }

                var pressedKeys = keys.Where(x => Input.IsKeyPress(x)).ToList();
                var isActionPressed = pressedKeys.Count() > 0;
                if (isActionPressed)
                {
                    Map.InvokeAction(ActionType.Press, action);
                }

                var releasedKeys = keys.Where(x => Input.IsKeyRelease(x)).ToList();
                var isActionReleased = releasedKeys.Count() > 0;
                if (isActionReleased)
                {
                    Map.InvokeAction(ActionType.Release, action);
                }
            }
        }
    }
}
