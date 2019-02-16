using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnDGame.MazeGen.DepthFirst;
using Microsoft.Xna.Framework;

namespace DnDGame.Engine.ECS.Systems.MazeGen
{
    static class MazeCon
    {
        public enum CellType
        {
            Floor,
            NorthWall,
			WallTop,
            EastWall,
            SouthWall,
			SouthWallTop,
            WestWall,
            NorthWestWall,
            NorthEastWall,
            SouthEastWall,
            SouthWestWall
        }
        static Point[] adjacentVectors = new Point[] { new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0) };
		/// <summary>
		/// If a wall tile is in the middle of a larger wall, it will have 1  adjacent cell missing, which we can use to identify which direction it is in.
		/// </summary>
        static Dictionary<Point, CellType> midWallTypes = new Dictionary<Point, CellType>
            {
                { new Point(0, -1), CellType.NorthWall },
                { new Point(1, 0), CellType.EastWall },
                { new Point(0, 1), CellType.SouthWall },
                { new Point(-1, 0), CellType.WestWall }
            };
		/// <summary>
		/// If a wall tile is the corner of a wall, it will have two adjacent cells missing, which we can use to identify which direction it is in.
		/// </summary>
		static readonly Dictionary<Point[], CellType> CornerWallTypes = new Dictionary<Point[], CellType>
		{
			{ new[] { new Point(-1, 0), new Point(0, -1)}, CellType.NorthWestWall },
			{ new[] { new Point(1, 0), new Point(0, -1)}, CellType.NorthEastWall },
			{ new[] { new Point(1, 0), new Point(0, 1)}, CellType.SouthEastWall },
			{ new[] { new Point(-1, 0), new Point(0, 1)}, CellType.SouthWestWall },

		};
        /// <summary>
        /// Given a list of points comprising a maze, convert it to a dictionary of the point and it's type of cell
        /// </summary>
        /// <param name="maze">The generated maze</param>
        /// <param name="width"></param>
        /// <param name="height"></param>

        public static List<Tuple<Point, CellType>> ConvertMaze(List<Point> maze, int width, int height)
        {
  
            var MazeGrid = new int[width, height];
            var points = maze.ToArray();
            for (int i = 0; i < maze.Count(); i++)
            {
                MazeGrid[points[i].X, points[i].Y] = 1;
            }
            var Maze = new List<Tuple<Point, CellType>>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (MazeGrid[x, y] != 1) continue;
                    var currentPoint = new Point(x, y);
                    var adjCells = GetAdjacentCells(new Point(x, y), MazeGrid, width, height);
					CellType cellType;
					switch (adjCells.Count())
                    {

                        case 4:

                            Maze.Add(new Tuple<Point, CellType>(currentPoint, CellType.Floor));
                            break;
                        case 3:
							Maze.Add(new Tuple<Point, CellType>(currentPoint, CellType.Floor));
							cellType = midWallTypes
                                .Where(v => !adjCells.Contains(currentPoint + v.Key))
                                .Select(v => midWallTypes[v.Key]).First();
							Maze.Add(new Tuple<Point, CellType>(currentPoint, cellType));
							if (cellType == CellType.NorthWall || cellType == CellType.SouthWall)
							{
								Maze.Add(new Tuple<Point, CellType>(currentPoint - new Point(0, 1), CellType.WallTop));
							}
							break;

						case 2:
							Maze.Add(new Tuple<Point, CellType>(currentPoint, CellType.Floor));
							cellType = CornerWallTypes
									.Where(v => !adjCells.Contains(currentPoint + v.Key[0]) && !adjCells.Contains(currentPoint + v.Key[1]))
									.Select(v => v.Value)
									.First();
							Maze.Add(new Tuple<Point, CellType>(currentPoint, cellType));
							break;
						default:
							Maze.Add(new Tuple<Point, CellType>(currentPoint, CellType.Floor));
							break;
                    }
                }
            }
            return Maze;
        }

        static Point GetCell(Point startCell,  Point trans, int width, int height)
        {
            var cell = new Point();
    

            return cell;
        }

        static List<Point> GetAdjacentCells(Point cell, int[,] grid, int width, int height)
        {
            var AdjacentCells = new List<Point>();
            for (int i = 0 ; i < adjacentVectors.Length; i++)
            {
                var adjPoint = cell + adjacentVectors[i];
                var mazeRegion = new Rectangle(0, 0, width, height);
                if (!mazeRegion.Contains(adjPoint)) continue;
                var adjCell = grid[adjPoint.X, adjPoint.Y];
                if (adjCell != 0) AdjacentCells.Add(adjPoint);
 
                
            }
            return AdjacentCells;
        }
    }
}
