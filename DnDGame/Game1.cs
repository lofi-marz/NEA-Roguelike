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
        int playerid;
        TileGrid testMap;
        InputHelper input;
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
            input = new InputHelper();
            camera = new Camera();
            world = new World();
            
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
            var PlayerCollision = new List<Rectangle>();
            PlayerCollision.Add(new Rectangle(4, 4, 8, 24));
            playerid = world.CreateEntity( 
                new TransformComponent(new Vector2(0f)), 
                new SpriteComponent(knightSprite, new Rectangle(0, 0, 16, 32), new Vector2(2f), 2, 32, 16),
                new MovementComponent(Vector2.Zero, new Vector2(10), new Vector2(0.8f)),
                new CollisionPolygon(PlayerCollision));
            
            var spriteSheet = Content.Load<Texture2D>("Sprites/DungeonTileset");
            testMap = new TileGrid(spriteSheet, 12, 9);
            
            var sizeX = 50;
            var sizeY = 50;

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
                        var xPos = point.X * scale - i + scale;
                        var yPos = point.Y * scale - j + scale;
                        var cell = world.CreateEntity(
    new TransformComponent(new Vector2(xPos*16*2, yPos*16*2)),
    new SpriteComponent(spriteSheet, new Rectangle(1*16, 4*16, 16, 16), new Vector2(2f)));
                        world.Sprites.Add(cell, new Vector2(xPos * 16*2, yPos * 16*2));
                    }
                }
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
            input.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            var oldPos = Player.Pos;
            Player.UpdateInput(input);
            Player.Update(gameTime);
            Velocity.Update(world, gameTime);
            if (Player.Pos != oldPos)
            {
                world.Sprites.Remove(playerid, oldPos);
                world.Sprites.Add(playerid, Player.Pos);
                ((TransformComponent)world.EntityComponents[typeof(TransformComponent)][playerid]).Pos = Player.Pos;
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
            
            camera.Pos = (Player.Pos-centre) * new Vector2(0.95f);
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
