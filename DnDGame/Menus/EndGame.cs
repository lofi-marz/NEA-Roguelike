using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnDGame.Menus
{
	public static class EndGame
	{
		public static Panel Init()
		{
			Panel panel = new Panel(new Vector2(500, 500), PanelSkin.Default, Anchor.Center);

			// add title and text
			panel.AddChild(new Header("Game over!"));
			panel.AddChild(new HorizontalLine());


			var exitButton = new Button("Exit Game", ButtonSkin.Default, Anchor.Auto)
			{
				Identifier = "exit"
			};
			panel.AddChild(exitButton);
			return panel;
		}


	}
}
