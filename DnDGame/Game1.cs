using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DnDGame.Engine;
using DnDGame.Engine.Drawing;
using DnDGame.MazeGen.DepthFirst;
using System.Collections.Generic;

//TODO
// - Maze gen needs to be able to support rooms with entrances
// - Need to be able to generate tilemaps from that

namespace DnDGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    //Goals
    //Set up dungeon map system
    //Set up enemies
    //Character animation
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;
        PlayerCharacter Player;
        TileGrid testMap;
        InputHelper input;
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
            var playerSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/Knight"), 9, 1)
            {
                xScale = 2f,
                yScale = 2f
            };
            var spriteSheet = Content.Load<Texture2D>("Sprites/DungeonTileset");
            testMap = new TileGrid(spriteSheet, 12, 9);
            
            var testCells = new List<Cell>();
            var sizeX = 15;
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
                        testCells.Add(new Cell( point.X * scale - i, point.Y * scale - j, "floor"));
                    }
                }
            }
            testMap.Cells = testCells;
            Player = new PlayerCharacter(playerSprite);
            Player.Pos = new Vector2(GraphicsDevice.Viewport.Width * 0.5f, GraphicsDevice.Viewport.Height * 0.5f);
            /*for (int x = 0; x < size; x++)
            {

                for (int y = 0; y < size+2; y++)
                {

                    if (x == 0 && y == 0)
                    {

                        testCells.Add(new Cell(x, y, "topLeftWallTop"));
                    }
                    else if (x == 0 && y == size)
                    {
                        testCells.Add(new Cell(x, y, "floor"));
                        testCells.Add(new Cell(x, y, "bottomLeftWallTop"));
                    }

                    else if (x == size-1 && y == 0)
                    {
                        testCells.Add(new Cell(x, y, "topRightWallTop"));
                    }
                    else if (x == size-1 && y == size)
                    {
                        testCells.Add(new Cell(x, y, "floor"));
                        testCells.Add(new Cell(x, y, "bottomRightWallTop"));
                    }
                    else if (y == size+1)
                    {
                        testCells.Add(new Cell(x, y, "wall"));
                    }
                    else if (x == 0 && y == 1)
                    {
                        testCells.Add(new Cell(x, y, "wall"));
                        testCells.Add(new Cell(x, y, "leftWallMid"));
                    }
                    else if (x == 0)
                    {
                        testCells.Add(new Cell(x, y, "floor"));
                        testCells.Add(new Cell(x, y, "leftWallMid"));
                    }
                    else if (x == size-1 && y == 1)
                    {
                        testCells.Add(new Cell(x, y, "wall"));
                        testCells.Add(new Cell(x, y, "rightWallMid"));
                    }
                    else if (x == size-1)
                    {
                        testCells.Add(new Cell(x, y, "floor"));
                        testCells.Add(new Cell(x, y, "rightWallMid"));
                    }

                    else if (y == 0)
                    {
                        testCells.Add(new Cell(x, y, "wallTop"));
                    }
                    else if (y == 1)
                    {

                        testCells.Add(new Cell(x, y, "wall"));
                    }
                    else if (y == size)
                    {
                        testCells.Add(new Cell(x, y, "floor"));
                        testCells.Add(new Cell(x, y, "wallTop"));
                    }
                    else
                    {
                        testCells.Add(new Cell(x, y, "floor"));
                    }

                }
            }*/



            //PlayerCharacter = new PlayerCharacter(Content.Load<Texture2D>());
            // TODO: use this.Content to load your game content here

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
            var camera = new Camera();
            var viewport = GraphicsDevice.Viewport;
            var centre = new Vector2(viewport.Width * 0.5f, viewport.Height * 0.5f);
            
            camera.Pos = (Player.Pos - centre) * new Vector2(0.9f);
            camera.Scale = new Vector2(1f);
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix:camera.GetTransform(GraphicsDevice.Viewport));
            //spriteBatch.Draw(playerSprite, destinationRectangle: new Rectangle(10, 10, 32, 32), sourceRectangle: new Rectangle(0, 0, 32, 32));
            testMap.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
