using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Myra;

using DnDGame.Engine;

using DnDGame.Engine.Drawing;
using DnDGame.Engine.Systems;
using DnDGame.Engine.Systems.Input;
using DnDGame.Engine.Systems.MazeGen;
using DnDGame.Engine.Systems.Drawing;
using DnDGame.Engine.Components;
using DnDGame.MazeGen.DepthFirst;
using System.Collections.Generic;
using System;

using Newtonsoft.Json;
using System.IO;
using DnDGame.Engine.Initialization;
using Myra.Graphics2D.UI;
using DnDGame.Engine.Player;


//TODO
// - Maze gen needs to be able to support rooms with entrances
// - Need to be able to generate tilemaps from that

namespace DnDGame
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>

	//Goals
	//Set up enemies
	//Character animation
	public class MainGame : Game
	{
		
		GraphicsDeviceManager graphics;

		SpriteBatch spriteBatch;
		Rectangle VisibleRegion;
		public int playerid;
		public DungeonGame CurrentGame;
		public Vector2 globalScale;
		private Desktop _host;
		SpriteFont arial;
		public MainGame()
		{
			IsMouseVisible = true;
			graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// TODO: Add your initialization logic here
			//input = new InputHelper();
			CurrentGame = new DungeonGame();
			globalScale = new Vector2(2f);
			graphics.PreferredBackBufferWidth = 1280;  // set this value to the desired width of your window
			graphics.PreferredBackBufferHeight = 720;   // set this value to the desired height of your window
			graphics.ApplyChanges();


			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			MyraEnvironment.Game = this;
			var mainMenu = new Grid
			{
				RowSpacing = 8,
				ColumnSpacing = 8
			};
			mainMenu.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
			mainMenu.RowsProportions.Add(new Grid.Proportion(Grid.ProportionType.Auto));
			var startButton = new Button
			{

				GridColumn = 0,
				GridRow = 0,
				Text = "Start Game"
			};

			var Button = new Button
			{

				GridColumn = 1,
				GridRow = 0,
				Text = "Start"
			};

			startButton.Click += (s, a) =>
			{
				StartGame();
			};

			mainMenu.Widgets.Add(startButton);

			_host = new Desktop();
			_host.Widgets.Add(mainMenu);

			arial = Content.Load<SpriteFont>("fonts/Arial");

			spriteBatch = new SpriteBatch(GraphicsDevice);
			var tileset = TilesetManager.LoadJson("DungeonTileset");
			
			tileset.SpriteSheet = Content.Load<Texture2D>("Sprites/DungeonTileset");
			TilesetManager.AddSet("dungeon", tileset);




			



		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the World.Instance,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{

			CurrentGame.Update();
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			var viewport = GraphicsDevice.Viewport;

			var centre = new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);

			if (CurrentGame.GameStarted)
			{
				VisibleRegion = CurrentGame.GetVisibleRegion(viewport);
				Physics.Update(gameTime, VisibleRegion);
				AnimationManager.Update(gameTime, VisibleRegion);
				NPCController.Update(gameTime, VisibleRegion);
				AttackManager.Update(gameTime, VisibleRegion);
			}


			base.Update(gameTime);
		}



		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);
			_host.Bounds = new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth,
  GraphicsDevice.PresentationParameters.BackBufferHeight);
			_host.Render();
			// TODO: Add your drawing code here
			if (CurrentGame.GameStarted)
			{
				var viewport = GraphicsDevice.Viewport;
				spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: CurrentGame.Camera.GetTransform(viewport));

				CurrentGame.Draw(ref spriteBatch, viewport, gameTime);
				spriteBatch.End();
			}
			

			

			//spriteBatch.DrawString(arial, $"{camera.Pos.X}, {camera.Pos.Y}", camera.Pos, Color.Black);

			base.Draw(gameTime);
			
		}


		public void StartGame()
		{
			CurrentGame.Player = new PlayerCharacter(Class.Elf, Race.Elf, Gender.Male);
			CurrentGame.Initialize();
			CurrentGame.LoadContent();
			CurrentGame.CreateDungeon(20, 20, 5);
			CurrentGame.GameStarted = true;
		}
	}
}
