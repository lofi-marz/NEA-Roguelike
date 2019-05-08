using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using DnDGame.Engine;
using DnDGame.Engine.Systems;
using DnDGame.Engine.Systems.Drawing;
using DnDGame.Engine.Components;
using DnDGame.Engine.Player;
using GeonBit.UI;
using DnDGame.Engine.Systems.Stats;
using System;
using System.Linq;


//TODO
// - Maze gen needs to be able to support rooms with entrances
// - Need to be able to generate tilemaps from that

namespace DnDGame
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>

	public class MainGame : Game
	{
		
		GraphicsDeviceManager graphics;

		SpriteBatch spriteBatch;
		Rectangle VisibleRegion;
		public int playerid;
		public DungeonGame CurrentGame;
		public Vector2 globalScale;

		public MainGame()
		{
			IsMouseVisible = false;
			graphics = new GraphicsDeviceManager(this);

			Content.RootDirectory = "Content";

		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// 
		/// </summary>
		protected override void Initialize()
		{
			
			UserInterface.Initialize(Content, BuiltinThemes.hd);
			UserInterface.Active.UseRenderTarget = true;
			// TODO: Add your initialization logic here
			//input = new InputHelper();
			CurrentGame = new DungeonGame();
			globalScale = new Vector2(2f);
			graphics.PreferredBackBufferWidth = 1280;  
			graphics.PreferredBackBufferHeight = 720;
			Window.AllowUserResizing = true;
			graphics.ApplyChanges();
			

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// 
		/// </summary>
		protected override void LoadContent()
		{

			spriteBatch = new SpriteBatch(GraphicsDevice);
			//Loading the dungeon tileset
			TileAtlas tileset = TilesetManager.LoadJson("DungeonTileset");
			
			tileset.SpriteSheet = Content.Load<Texture2D>("Sprites/DungeonTileset");
			TilesetManager.AddSet("dungeon", tileset);

			//Creating the main menu
			var MainMenu = Menus.MainMenu.Init();
			MainMenu.Find("menuOptions").Find("start").OnClick += (GeonBit.UI.Entities.Entity btn) => {
				StartGame();
				var playerStatsBox = Menus.PlayerStats.Init(CurrentGame.Player.Entity);
				UserInterface.Active.AddEntity(playerStatsBox);
				World.Instance.GetComponent<CharacterStats>(CurrentGame.Player.Entity).CurrentStats["health"] += 0;
				btn.Parent.Parent.Visible = false;
			};
			
			MainMenu.Find("menuOptions").Find("exit").OnClick += (GeonBit.UI.Entities.Entity btn) => { Exit(); };
			UserInterface.Active.AddEntity(MainMenu);







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
			UserInterface.Active.Update(gameTime);

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			var viewport = GraphicsDevice.Viewport;

			var centre = new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);

			//While the game is in progress, update all of the systems.
			if (CurrentGame.GameStarted)
			{
				VisibleRegion = CurrentGame.GetVisibleRegion(viewport);
				

				Physics.Update(gameTime, VisibleRegion);
				AnimationManager.Update(gameTime, VisibleRegion);
				NPCController.Update(gameTime, VisibleRegion);
				HitHurtCollisionManager.Update(gameTime, VisibleRegion);
				LifeTimerManager.Update(gameTime);
				StatChangeCalculator.Update();
				StatChangeUpdater.Update();
				ChildPropertyUpdater.Update();

			}

			//If the game has ended, go to the end game menu.
			if (CurrentGame.EndGame)
			{
				UserInterface.Active.Clear();
				var endGame = Menus.EndGame.Init();
				endGame.Find("exit").OnClick += (GeonBit.UI.Entities.Entity btn) => { Exit(); };
				UserInterface.Active.AddEntity(endGame);
				
			}


			base.Update(gameTime);
		}



		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			UserInterface.Active.Draw(spriteBatch);
			GraphicsDevice.Clear(Color.Black);

			// While the game is in progress
			if (CurrentGame.GameStarted)
			{
				//Draw the game with the current transform created by the camera.
				var viewport = GraphicsDevice.Viewport;
				spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: CurrentGame.Camera.GetTransform(viewport));

				CurrentGame.Draw(ref spriteBatch, viewport, gameTime);
				spriteBatch.End();
			}
			UserInterface.Active.DrawMainRenderTarget(spriteBatch);



			//spriteBatch.DrawString(arial, $"{camera.Pos.X}, {camera.Pos.Y}", camera.Pos, Color.Black);

			base.Draw(gameTime);
			
		}


		public void StartGame()
		{
			var rnd = new Random();
			CurrentGame.Player = new PlayerCharacter((Class)(rnd.Next(0, 3)), (Race)rnd.Next(0,3), Gender.Male);
			CurrentGame.Initialize();
			CurrentGame.LoadContent();
			CurrentGame.CreateDungeon(20, 20, 5);

			
			CurrentGame.GameStarted = true;
			Console.WriteLine("Entities:");
			Console.WriteLine(string.Join(", ", World.Instance.Entities.Select(e => e.Id)));
			Console.WriteLine("Player Components:");
			foreach (var componentType in World.Instance.EntityComponents)
			{
				foreach (var component in componentType.Value)
				{
					if (component.Key == CurrentGame.Player.Entity)
					{
						Console.WriteLine(component.Value.GetType() + ", ");
					}
				}
			}
		}
	}
}
