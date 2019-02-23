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
			Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.Center);
			
			// add title and text
			panel.AddChild(new Header("Dungeon Game"));
			panel.AddChild(new HorizontalLine());
			//panel.AddChild(new Paragraph("This is a simple panel with a button."));

			
			// add a button at the bottom

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
