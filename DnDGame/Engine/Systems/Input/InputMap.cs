using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Engine.Systems.Input
{
	/// <summary>
	/// The default controls for the game.
	/// </summary>
    public static class DefaultInputs
    {
        public static Dictionary<GameAction, List<Keys>> KeyMap = new Dictionary<GameAction, List<Keys>>
            {
				{ GameAction.MoveUp, new List<Keys>() {Keys.Up, Keys.W } },
				{ GameAction.MoveDown, new List<Keys>() {Keys.Down, Keys.S } },
				{ GameAction.MoveLeft, new List<Keys>() {Keys.Left, Keys.A } },
				{ GameAction.MoveRight, new List<Keys>() {Keys.Right, Keys.D } },
				{ GameAction.PrimaryAction, new List<Keys> {Keys.Space, Keys.Q} }
			};

    }

	/// <summary>
	/// All of the actions the player is able to perform in the game.
	/// </summary>
    public enum GameAction
    {
        MoveUp,
        MoveRight,
        MoveDown,
        MoveLeft,
        PrimaryAction,
        SecondaryAction
    }


    /// <summary>
    /// Maps a key press, release or new press to an action
    /// </summary>
    public class InputMap
    {
		/// <summary>
		/// Maps a list of key presses to a game action.
		/// </summary>
        public Dictionary<GameAction, List<Keys>> KeyMap;

		/// <summary>
		/// Maps a key press, release or down event to an Action function.
		/// </summary>
        public Dictionary<GameAction, Action> ActionDownMap;
        public Dictionary<GameAction, Action> ActionReleasedMap;
        public Dictionary<GameAction, Action> ActionPressedMap;

        public InputMap(Dictionary<GameAction, List<Keys>> map)
        {
            KeyMap = map;
            ActionDownMap = new Dictionary<GameAction, Action>();
            ActionPressedMap = new Dictionary<GameAction, Action>();
            ActionReleasedMap = new Dictionary<GameAction, Action>();
            foreach (var keymap in map)
            {
                ActionDownMap.Add(keymap.Key, () => { });
                ActionPressedMap.Add(keymap.Key, () => { });
                ActionReleasedMap.Add(keymap.Key, () => { });
            }

        }
        

		/// <summary>
		/// Assign the default keys to the game actions.
		/// </summary>
        public InputMap()
        {
            KeyMap = DefaultInputs.KeyMap;
            ActionDownMap = new Dictionary<GameAction, Action>();
            ActionPressedMap = new Dictionary<GameAction, Action>();
            ActionReleasedMap = new Dictionary<GameAction, Action>();
            foreach (var keymap in KeyMap)
            {
                ActionDownMap.Add(keymap.Key, () => { });
                ActionPressedMap.Add(keymap.Key, () => { });
                ActionReleasedMap.Add(keymap.Key, () => { });
            }
        }

		/// <summary>
		/// Trigger a game action.
		/// </summary>
		/// <param name="type">Key press, down or release.</param>
		/// <param name="gameAction">The type of GameAction.</param>
        public void InvokeAction(ActionType type, GameAction gameAction)
        {
            switch (type)
            {
                case ActionType.Down:
                    if (!ActionDownMap.ContainsKey(gameAction)) break;
                    ActionDownMap[gameAction].Invoke();

                    break;
                case ActionType.Press:
                    if (!ActionPressedMap.ContainsKey(gameAction)) break;
                    ActionPressedMap[gameAction].Invoke();
                    break;
                case ActionType.Release:
                    if (!ActionReleasedMap.ContainsKey(gameAction)) break;
                    ActionReleasedMap[gameAction].Invoke();
                    break;
            }
        }
    }
}
