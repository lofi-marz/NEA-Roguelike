using DnDGame.Engine.Player;
using DnDGame.Engine.Systems;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Menus
{
	public static class MainMenu
	{
		public static Panel Init()
		{
			Panel panel = new Panel(new Vector2(850, 400), PanelSkin.Default, Anchor.Center);

			// add title and text
			panel.AddChild(MenuOptions());

			panel.AddChild(Instructions());
			return panel;
		}

		public static Panel Instructions()
		{
			Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.CenterRight);
			panel.AddChild(new Header("Instructions"));
			panel.AddChild(new HorizontalLine());
			var instructions = new Paragraph()
			{
				Scale = 1f,
				Text = "Welcome to DungeonGame. Your aim is to kill as many enemies as you can before dying. \n\n" +
			"Movement: Arrow keys or WASD.\n\n" +
			"Attack: Q or Space"
			};
			panel.AddChild(instructions);
			return panel;
		}

		public static Panel MenuOptions()
		{
			Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.CenterLeft)
			{
				Identifier = "menuOptions"
			};

			// add title and text
			panel.AddChild(new Header("Dungeon Game"));
			panel.AddChild(new HorizontalLine());


			var startButton = new Button("Start Game", ButtonSkin.Default, Anchor.Auto)
			{
				Identifier = "start"
			};
			var exitButton = new Button("Exit Game", ButtonSkin.Default, Anchor.Auto)
			{
				Identifier = "exit"
			};

			panel.AddChild(startButton);
			panel.AddChild(exitButton);
			return panel;
		}

	}
}
