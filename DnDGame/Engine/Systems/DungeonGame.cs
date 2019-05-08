using DnDGame.Engine.Initialization;
using DnDGame.Engine.Systems.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DnDGame.Engine.Drawing;
using DnDGame.Engine.Systems.Drawing;

using DnDGame.Engine.Systems.MazeGen;
using DnDGame.Engine.Player;
using DnDGame.Engine.Components;
using DnDGame.Engine.Initialization.CreateObjects;
using static DnDGame.Engine.Systems.MazeGen.DungeonGen;
using static DnDGame.Engine.Components.CharacterStats;

namespace DnDGame.Engine.Systems
{
	public class DungeonGame
	{
		public PlayerCharacter Player;
		public bool GameStarted;
		PlayerController InputController;
		public Camera Camera;
		List<int> DungeonSprites;
		public bool EndGame;
		public void Initialize()
		{
			
			InputController = new PlayerController();

			Camera = new Camera(new Vector2(0))
			{
				Scale = 2f
			};
			

		}


		public void LoadContent()
		{
			Player.Entity = CreatePlayer.Init(Player);
			InputAssign.AssignMovementController(Player, ref InputController);
			InputAssign.AssignWeaponController(Player, ref InputController);
		

			var playerStats = World.Instance.GetComponent<CharacterStats>(Player.Entity);
			playerStats.CurrentStats = playerStats.MaxStats;
			playerStats.OnStatChange += (StatChangeArgs e) =>
			{
				if (e.CurrentStats["health"] <= 0)
				{
					EndGame = true;
				}
			};
			World.Instance.SetComponent(Player.Entity, playerStats);


		}


		public void Update()
		{
			if (!GameStarted) return;
			InputController.Update();
			var playerTransform = World.Instance.GetComponent<Transform>(Player.Entity);
			Camera.Centre = playerTransform.Pos;
		}


		public void CreateDungeon(int width, int height, int pathWidth)
		{
			bool playerAdded = false;
			var (Grid, Maze) = DepthFirst.GenDungeon(width, height);
			var mazeList = new List<Point>();
			var rnd = new Random(); 
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < width; y++)
				{
					if (Grid[x, y] == (int)DepthFirst.CellType.Empty) continue;
					for (int i = 1; i <= pathWidth; i++)
					{
						for (int j = 1; j <= pathWidth; j++)
						{
							var xPos = x * pathWidth - i;
							var yPos = y * pathWidth - j;
							mazeList.Add(new Point(xPos, yPos));
						}
					}
				}
			}
		
			var realDungeon = ConvertMaze(mazeList, width * pathWidth, height * pathWidth);
			
			foreach (var (Pos, Item) in realDungeon)
			{
				var realPos = Pos.ToVector2() * new Vector2(16 * 1);
				List<int> cellEntities = new List<int>();
				if (Item.StartsWith("floor"))
				{
					cellEntities.Add(CreateCell.Init(realPos, Item, 0f));
					var nonFloorItems = realDungeon.Where(x => x.Pos == Pos && x.Item != "floor");

					if (rnd.Next(0,1000) < 5 && nonFloorItems.Count() <= 1) cellEntities.Add(CreateNPC.Init(realPos, "orc_shaman"));

				}
				else
				{
					cellEntities.Add(CreateCell.Init(realPos, Item, 0.1f));
				}
				cellEntities.ForEach(e => World.Instance.SpriteHash.Add(e, realPos));
				
			}
			foreach (var follower in World.Instance.GetEntitiesByType(typeof(Follower)))
			{
				var controller = World.Instance.GetComponent<Follower>(follower);
				controller.Parent = Player.Entity;
				World.Instance.SetComponent(follower, controller);
			}

			do
			{
				(Point Pos, string Item) = realDungeon[rnd.Next(0, realDungeon.Count())];
				var nonFloorItems = realDungeon.Where(x => x.Pos == Pos && x.Item != "floor");
				if (nonFloorItems.Count() > 1) continue;
				playerAdded = true;

				var transform = World.Instance.GetComponent<Transform>(Player.Entity);

				World.Instance.SpriteHash.Remove(Player.Entity, transform.Pos);
				transform.Pos = Pos.ToVector2() * 16;
				World.Instance.SetComponent(Player.Entity, transform);
				World.Instance.SpriteHash.Add(Player.Entity, transform.Pos);
			} while (!playerAdded);
		}

		public void Draw(ref SpriteBatch spriteBatch, Viewport viewport, GameTime gameTime)
		{
			if (!GameStarted) return;
			//SpriteBatch.DrawString(arial, $"{camera.Pos.X}, {camera.Pos.Y}", camera.Pos, Color.Black);
			var visibleRegion = GetVisibleRegion(viewport);
			visibleRegion.Inflate(-32, -32);
			SpriteDrawSystem.Update(ref spriteBatch, visibleRegion);

		}



		public Rectangle GetVisibleRegion(Viewport viewport)
		{
			int startX = (int)(Camera.Centre.X - (viewport.Width / (2*Camera.Scale)));
			int startY = (int)(Camera.Centre.Y - (viewport.Height / (2*Camera.Scale)));
			int width = (int)(viewport.Width / Camera.Scale);
			int height = (int)(viewport.Height/Camera.Scale);
			return new Rectangle(startX, startY, width, height);
		}
	}

	
}
