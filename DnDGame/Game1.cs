using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DnDGame.Engine;
using DnDGame.Engine.Drawing;
using DnDGame.MazeGen.DepthFirst;
using System.Collections.Generic;
using DnDGame.Engine.ECS;

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
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        World world;
        SpriteBatch spriteBatch;
        PlayerCharacter Player;
        TileGrid testMap;
        InputHelper input;
        Camera camera;
        public Game1()
        {
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
            input = new InputHelper();
            camera = new Camera();
            world = new World();
            world.Systems.Add(new DrawSystem());
            
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var knightSprite = Content.Load<Texture2D>("Sprites/Knight");
            var playerSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/Knight"), 9, 1)
            {
                xScale = 1f,
                yScale = 1f
            };

            world.CreateEntity(1, new PositionComponent(new Vector2(0f)), new SpriteComponent(knightSprite, new Rectangle(0, 0, 16, 32), new Vector2(2f), 32, 16)); 
            
            var spriteSheet = Content.Load<Texture2D>("Sprites/DungeonTileset");
            testMap = new TileGrid(spriteSheet, 12, 9);
            
            var testCells = new List<Cell>();
            var sizeX = 17;
            var sizeY = 15;

            var maze = DepthFirst.GenMaze(sizeX, sizeY);

            var scale = 3;
            foreach (var point in maze)
            {
                /*testCells.Add(new Cell(point.X*scale, point.Y*scale, "floor"));
                testCells.Add(new Cell(point.X * scale-1, point.Y * scale, "floor"));
                testCells.Add(new Cell(point.X * scale, point.Y * scale-1, "floor"));
                testCells.Add(new Cell(point.X * scale-1, point.Y * scale-1, "floor"));*/
                for (int i = 1; i <= scale; i++ )
                {
                    for (int j = 1; j <= scale; j++)
                    {
                        testCells.Add(new Cell( point.X * scale - i + scale, point.Y * scale - j + scale, "floor"));
                    }
                }
            }
            testMap.Cells = testCells;
            Player = new PlayerCharacter(playerSprite);
           
     

               

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
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            input.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Player.UpdateInput(input);
            Player.Update(gameTime);
            // TODO: Add your update logic here

            base.Update(gameTime);
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            
            var viewport = GraphicsDevice.Viewport;
            
            var centre = new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            
            
            camera.Scale = new Vector2(2f);
            
            camera.Pos = (Player.Pos- centre) * new Vector2(0.8f);
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix:camera.GetTransform(GraphicsDevice.Viewport));
            //spriteBatch.Draw(playerSprite, destinationRectangle: new Rectangle(10, 10, 32, 32), sourceRectangle: new Rectangle(0, 0, 32, 32));
            testMap.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            ((DrawSystem)world.Systems[0]).Draw(world, spriteBatch);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
