using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using DnDGame.Engine;
using DnDGame.Engine.Drawing;
using DnDGame.MazeGen.DepthFirst;
using System.Collections.Generic;
using DnDGame.Engine.ECS;
using DnDGame.Engine.ECS.Systems;
using System;
using DnDGame.Engine.Input;
using DnDGame.Engine.ECS.Systems.Input;
using DnDGame.Engine.ECS.Systems.MazeGen;

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
        public int playerid;
        TileGrid testMap;
        PlayerInput playerInput;
        //InputHelper input;
        Camera camera;
        SpriteFont arial;
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
            //input = new InputHelper();
            camera = new Camera();
            world = new World();
            playerInput = new PlayerInput();
            
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
            arial = Content.Load<SpriteFont>("fonts/Arial");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            var knightSprite = Content.Load<Texture2D>("Sprites/Knight");
            var playerSprite = new AnimatedSprite(Content.Load<Texture2D>("Sprites/Knight"), 9, 1)
            {
                xScale = 1f,
                yScale = 1f
            };
            var PlayerCollision = new List<Rectangle>
            {
                new Rectangle(0, 0, 16, 32)
            };
            playerid = world.CreateEntity( 
                new TransformComponent(new Vector2(0f), new Vector2(2f)), 
                new SpriteComponent(knightSprite, new Rectangle(0, 0, 16, 32), 2, 32, 16),
                new MovementComponent(Vector2.Zero, new Vector2(150), new Vector2(0.75f)),
                new CollisionPolygon(PlayerCollision));
            
            Console.WriteLine(playerid);
            playerInput.Map.ActionMap[GameAction.MoveUp] = new Action(() => { Movement.MoveEntity(world, playerid, Direction.Up); });
            playerInput.Map.ActionMap[GameAction.MoveDown] = new Action(() => { Movement.MoveEntity(world, playerid, Direction.Down); });
            playerInput.Map.ActionMap[GameAction.MoveLeft] = new Action(() => { Movement.MoveEntity(world, playerid, Direction.Left); });
            playerInput.Map.ActionMap[GameAction.MoveRight] = new Action(() => { Movement.MoveEntity(world, playerid, Direction.Right); });
            var spriteSheet = Content.Load<Texture2D>("Sprites/DungeonTileset");
            testMap = new TileGrid(spriteSheet, 12, 9);
            
            var sizeX = 50;
            var sizeY = 50;

            var maze = DepthFirst.GenMaze(sizeX, sizeY);

            var scale = 3;
            var FloorCollision = new List<Rectangle>
            {
                new Rectangle(0, 0, 16, 16)
            };
            var mazeList = new List<Point>();
            foreach (var point in maze)
            {
                for (int i = 1; i <= scale; i++ )
                {
                    for (int j = 1; j <= scale; j++)
                    {
                        var xPos = point.X * scale - i;
                        var yPos = point.Y * scale - j;
                        mazeList.Add(new Point(xPos, yPos));
                        
                    }
                }
            }
            var newMaze = MazeCon.ConvertMaze(mazeList, sizeX * scale, sizeY * scale);
            foreach (var cell in newMaze)
            {
                var pos = cell.Key;
                var type = cell.Value;
                var cellEntity = world.CreateEntity(
new TransformComponent(pos.ToVector2() * new Vector2(16 * 2), new Vector2(2f)),
new SpriteComponent(spriteSheet, new Rectangle(1 * 16, 4 * 16, 16, 16))
//new CollisionPolygon(FloorCollision)
);
                world.Sprites.Add(cellEntity, pos.ToVector2() * new Vector2(16 * 2));
            }
            Player = new PlayerCharacter(playerSprite);
            world.Sprites.Add(playerid, Player.Pos);




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
            /*var testBox = new List<Rectangle>
            {
                new Rectangle(0, 0, 15, 15)
            };
            var testBox2 = new List<Rectangle>
            {
                new Rectangle(10, 10, 10, 10)
            };
            var testPoly = new CollisionPolygon(testBox);
            var testPoly2 = new CollisionPolygon(testBox2);
            Console.WriteLine(testPoly.IsColliding(testPoly2));*/
            
            playerInput.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var oldPos = world.GetComponent<TransformComponent>(playerid).Pos;
            var vel = world.GetComponent<MovementComponent>(playerid).Velocity;
            //Player.UpdateInput(input);
            //Player.Update(gameTime);
            Velocity.Update(world, gameTime);
            var newPos = world.GetComponent<TransformComponent>(playerid).Pos;
            if (vel.Length() > 0)
            {
                world.Sprites.Remove(playerid, oldPos);
                world.Sprites.Add(playerid, newPos);
                //((TransformComponent)world.EntityComponents[typeof(TransformComponent)][playerid]).Pos = Player.Pos;
            }
            
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
         
            
            camera.Scale = new Vector2(1f);
            var playerPos = world.GetComponent<TransformComponent>(playerid).Pos;
            camera.Pos = (playerPos-centre) * new Vector2(0.95f);
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix:camera.GetTransform(GraphicsDevice.Viewport));
            //spriteBatch.Begin(samplerState: SamplerState.PointWrap);
            //spriteBatch.Draw(playerSprite, destinationRectangle: new Rectangle(10, 10, 32, 32), sourceRectangle: new Rectangle(0, 0, 32, 32));
            //testMap.Draw(spriteBatch);
            //Player.Draw(spriteBatch);
            
            spriteBatch.DrawString(arial, $"{camera.Pos.X}, {camera.Pos.Y}", camera.Pos, Color.Black);
            int startX = (int)camera.Pos.X;
            int startY = (int)camera.Pos.Y;
            int width = viewport.Width;
            int height = viewport.Height;
            DrawSystem.Update(world, spriteBatch, new Rectangle(startX, startY, width, height));
            base.Draw(gameTime);
            spriteBatch.End();
        }
    }
}
