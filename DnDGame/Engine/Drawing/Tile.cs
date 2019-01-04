using DnDGame.Engine.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace DnDGame.Engine.Drawing
{

    public class TileGrid : GameObject
    {
        public List<Cell> Cells;
        public int Height;
        public int Width;
        public Cell[,] Grid;
        public int CellSize;
        private TileMap Tiles;
        
        
        public TileGrid(Texture2D tileSet, int height, int width)
        {
            Grid = new Cell[width, height];
            Tiles = new TileMap(tileSet);
            Pos = new Vector2(0, 0);
        }


        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var cell in Cells)
            {
                var Tile = Tiles.Map[cell.Tile];
                var sprite = new Sprite(Tiles.SpriteSheet, new Rectangle(16 * Tile[0], 16 * Tile[1], 16, 16), 16, 16);
                sprite.Draw(spriteBatch, new Vector2(Pos.X + cell.X * 16f, Pos.Y + cell.Y * 16f));
            }
        }
    }

    public class Cell
    {
        public int X, Y;
        public string Tile;

        public Cell(int x, int y, string tile)
        {
            X = x;
            Y = y;
            Tile = tile;
        }
    }



    public class TileMap
    {
        public Texture2D SpriteSheet;

        public Dictionary<string, int[]> Map = new Dictionary<string, int[]>()
        {
            {"wallTop", new[] {1, 0 } },
            {"wall", new[] {1, 1 } },
            {"floor", new[] {1, 4 } },

            {"leftWallMid", new[]  {1, 8} },

            {"rightWallMid", new[]  {0, 8} },

            {"topLeftWallTop", new[] {2, 7 } },
            {"topLeftWall", new[] {2,8 } },

            {"topRightWallTop", new[] {3, 7 } },
            {"topRightWall", new[] {3, 8 } },

            {"bottomRightWallTop", new[] {3, 9 } },
            {"bottomRightWall", new[] {3, 10 } },

            {"bottomLeftWallTop", new[] {2, 9 } },
            {"bottomLeftWall", new[] {2, 10 } },
        };

        public TileMap(Texture2D spriteSheet)
        {
            SpriteSheet = spriteSheet;
        }
    }

    public class Tile
    {
        Rectangle SourceRect;
        List<Rectangle> CollisionBoxes;
    }





public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    public static class Utils
    {
        public static string Vector2String(Vector2 vector, int precision = 2)
        {
            return $"{Math.Round(vector.X, precision)},{Math.Round(vector.Y, precision)}";
        }
    }

}