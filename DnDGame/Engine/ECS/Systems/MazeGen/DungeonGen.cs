﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DnDGame.MazeGen.DepthFirst;
using Microsoft.Xna.Framework;

namespace DnDGame.Engine.ECS.Systems.MazeGen
{
    static class DungeonGen
    {
        public enum CellType
        {
            Floor,
			WallTop,
			NorthWall,
            EastWall,
            SouthWall,
			SouthWallTop,
            WestWall,
            NorthWestWall,
			NorthWestWallTop,
            NorthEastWall,
			NorthEastWallTop,
            SouthEastWall,
			SouthEastWallTop,
            SouthWestWall,
			SouthWestWallTop
        }
        static Point[] adjacentVectors = new Point[] { new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0) };
		static Point[] diagAdjacentVectors = new Point[] { new Point(-1, -1), new Point(1, -1), new Point(1, 1), new Point(-1, 1) };
		/// <summary>
		/// If a wall tile is in the middle of a larger wall, it will have 1  adjacent cell missing, which we can use to identify which direction it is in.
		/// </summary>
        static Dictionary<Point, string> midWallTypes = new Dictionary<Point, string>
            {
                { new Point(0, -1), "wall_mid" },
                { new Point(-1, 0), "wall_side_mid_left" },
                { new Point(0, 1), "wall_mid" },
                { new Point(1, 0), "wall_side_mid_right" }
            };
		/// <summary>
		/// If a wall tile is the corner of a wall, it will have two adjacent cells missing, which we can use to identify which direction it is in.
		/// </summary>
		static readonly Dictionary<Point[], string> CornerWallTypes = new Dictionary<Point[], string>
		{
			{ new[] { new Point(-1, 0), new Point(0, -1)}, "wall_corner_left" },
			{ new[] { new Point(1, 0), new Point(0, -1)}, "wall_corner_right" },
			{ new[] { new Point(1, 0), new Point(0, 1)}, "wall_corner_front_right" },
			{ new[] { new Point(-1, 0), new Point(0, 1)}, "wall_corner_front_left"},

		};
		/// <summary>
		/// Given a list of points comprising a maze, convert it to a dictionary of the point and it's type of cell
		/// </summary>
		/// <param name="maze">The generated maze</param>
		/// <param name="width"></param>
		/// <param name="height"></param>

		public static List<Tuple<Point, string>> ConvertMaze(List<Point> maze, int width, int height)
		{

			var MazeGrid = new int[width, height];
			var points = maze.ToArray();
			for (int i = 0; i < maze.Count(); i++)
			{
				MazeGrid[points[i].X, points[i].Y] = 1;
			}
			var Maze = new List<Tuple<Point, string>>();
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (MazeGrid[x, y] != 1) continue;
					var currentPoint = new Point(x, y);
					var adjCells = GetAdjacentCells(new Point(x, y), MazeGrid, width, height);
					string cellType;
					bool isTop = false;
					switch (adjCells.Count())
					{

						case 4:
							var diagAdjCells = GetDiagAdjacentCells(currentPoint, MazeGrid, width, height);
							if (diagAdjCells.Count == 3)
							{
								var missingCell = diagAdjacentVectors.Where(v => !diagAdjCells.Contains(currentPoint + v)).First() + currentPoint;
								if (missingCell.X > currentPoint.X && missingCell.Y > currentPoint.Y)
								{
									Maze.Add(new Tuple<Point, string>(currentPoint, "wall_side_mid_right"));
								}
								if (missingCell.X < currentPoint.X && missingCell.Y > currentPoint.Y)
								{
									Maze.Add(new Tuple<Point, string>(currentPoint, "wall_side_mid_left"));
								}
							}
							break;
						case 3:
							
							var midTypePair = midWallTypes
								.Where(v => !adjCells.Contains(currentPoint + v.Key))
								.First();
							cellType = midTypePair.Value;
							isTop = midTypePair.Key == new Point(0, -1);
		
							Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, isTop?1:0), cellType));

							if (cellType == "wall_mid" || cellType == "wall_corner_right")
							{
								Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, isTop?2:1), "wall_top_mid"));
							}
							break;

						case 2:

							var cornerTypePair = CornerWallTypes
									.Where(v => !adjCells.Contains(currentPoint + v.Key[0]) && !adjCells.Contains(currentPoint + v.Key[1]))
									.First();
							cellType = cornerTypePair.Value;
							isTop = cornerTypePair.Key.Contains(new Point(0, -1));
							Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, isTop?1:0), cellType));
							if (cellType == "wall_corner_left")
							{
								Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, 2), "wall_corner_top_left"));
								Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, 0), "wall_side_mid_left"));
							}
							if (cellType == "wall_corner_right")
							{
								Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, 2), "wall_corner_top_right"));
								Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, 0), "wall_side_mid_right"));
							}
							if (cellType == "wall_corner_front_left")
							{
								Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, 1), "wall_corner_bottom_left"));
							}
							if (cellType == "wall_corner_front_right")
							{
								Maze.Add(new Tuple<Point, string>(currentPoint - new Point(0, 1), "wall_corner_bottom_right"));
							}
							var cellName = cellType.ToString();
							//var cellTop = (CellType)Enum.Parse(typeof(CellType), cellType + "Top");
							//Maze.Add(new Tuple<Point, CellType>(currentPoint - new Point(0,1) , cellTop));
							break;
						default:
							break;

                    }
					Maze.Add(new Tuple<Point, string>(currentPoint, "floor_1"));
				}
            }
            return Maze;
        }

        static Point GetCell(Point startCell,  Point trans, int width, int height)
        {
            var cell = new Point();
    

            return cell;
        }

        static List<Point> GetDiagAdjacentCells(Point cell, int[,] grid, int width, int height)
        {
            var DiagAdjacentCells = new List<Point>();
            for (int i = 0 ; i < diagAdjacentVectors.Length; i++)
            {
                var adjPoint = cell + diagAdjacentVectors[i];
                var mazeRegion = new Rectangle(0, 0, width, height);
                if (!mazeRegion.Contains(adjPoint)) continue;
                var adjCell = grid[adjPoint.X, adjPoint.Y];
                if (adjCell != 0) DiagAdjacentCells.Add(adjPoint);
 
                
            }
            return DiagAdjacentCells;
        }

		static List<Point> GetAdjacentCells(Point cell, int[,] grid, int width, int height)
		{
			var AdjacentCells = new List<Point>();
			for (int i = 0; i < adjacentVectors.Length; i++)
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
