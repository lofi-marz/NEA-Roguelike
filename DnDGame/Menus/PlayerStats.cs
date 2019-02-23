
using DnDGame.Engine;
using DnDGame.Engine.Components;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DnDGame.Engine.Components.CharacterStats;

namespace DnDGame.Menus
{
	public static class PlayerStats
	{
		public static Panel Init(int player)
		{
			Panel panel = new Panel(new Vector2(250, 250), PanelSkin.Default, Anchor.BottomLeft)
			{
				Identifier = "playerstats"
			};

			// add title and text
			panel.AddChild(new Header("Stats:"));
			panel.AddChild(new HorizontalLine());
			panel.AddChild(new Label("Health:")
			{
				Identifier = "health"
			});
			panel.AddChild(new Label("Mana:")
			{
				Identifier = "mana"
			});
			panel.AddChild(new Label("Stamina:")
			{
				Identifier = "stamina"
			});
			panel.AddChild(new HorizontalLine());
			var playerStats = World.Instance.GetComponent<CharacterStats>(player);
			playerStats.OnStatChange += (StatChangeArgs e) =>
			{
				((Label)panel.Find("health")).Text = $"Health: {e.CurrentStats["health"]}";
				((Label)panel.Find("mana")).Text = $"Mana: {e.CurrentStats["mana"]}";
				((Label)panel.Find("stamina")).Text = $"Stamina: {e.CurrentStats["stamina"]}";
			};
			World.Instance.SetComponent(player, playerStats);
			return panel;
		}
	}
}
