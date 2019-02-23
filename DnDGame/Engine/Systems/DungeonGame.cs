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
using DnDGame.MazeGen.DepthFirst;
using DnDGame.Engine.Systems.MazeGen;
using DnDGame.Engine.Player;
using DnDGame.Engine.Components;
using DnDGame.Engine.Initialization.CreateObjects;

namespace DnDGame.Engine.Systems
{
	public class DungeonGame
	{
		public PlayerCharacter Player;
		public bool GameStarted;
		PlayerController InputController;
		public Camera Camera;
		List<int> DungeonSprites;

		public void Initialize()
		{
			
			InputController = new PlayerController();
			
			Camera = new Camera(new Vector2(0));
			Camera.Scale = 2f;
			
		}


		public void LoadContent()
		{
			Player.Entity = CreatePlayer.Init(Vector2.Zero, Player);
			InputAssign.AssignMovementController(Player, ref InputController);

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
			var maze = DepthFirst.GenDungeon(width, height);
			var mazeList = new List<Point>();
			var rnd = new Random();
			foreach (var point in maze)
			{
				for (int i = 1; i <= pathWidth; i++)
				{
					for (int j = 1; j <= pathWidth; j++)
					{
						var xPos = point.X * pathWidth - i;
						var yPos = point.Y * pathWidth - j;
						mazeList.Add(new Point(xPos, yPos));

					}
				}
			}

			var realDungeon = DungeonGen.ConvertMaze(mazeList, width * pathWidth, height * pathWidth);

			foreach (var cell in realDungeon)
			{
				var pos = cell.Item1.ToVector2() * new Vector2(16 * 1);
				var item = cell.Item2;
				List<int> cellEntities = new List<int>();
				if (item.StartsWith("floor"))
				{
					cellEntities.Add(CreateCell.Init(pos, item, 0f));
					if (rnd.Next(0,1000) < 5) cellEntities.Add(CreateNPC.Init(pos, "orc_shaman"));
				}
				else
				{
					cellEntities.Add(CreateCell.Init(pos, item, 0.1f));
				}
				cellEntities.ForEach(e => World.Instance.Sprites.Add(e, pos));
				
			}
			foreach (var follower in World.Instance.GetEntitiesByType(typeof(Follower)))
			{
				var controller = World.Instance.GetComponent<Follower>(follower);
				controller.Parent = Player.Entity;
				World.Instance.SetComponent(follower, controller);
			}
		}

		public void Draw(ref SpriteBatch spriteBatch, Viewport viewport, GameTime gameTime)
		{
			if (!GameStarted) return;
			//SpriteBatch.DrawString(arial, $"{camera.Pos.X}, {camera.Pos.Y}", camera.Pos, Color.Black);

			EntityDrawSystem.Update(ref spriteBatch, GetVisibleRegion(viewport));

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
