using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DnDGame.Engine;
namespace DnDGame.MazeGen.DepthFirst
{




	/// <summary>
	/// Randomized Depth first maze generation.
	/// </summary>
	static class DepthFirst
	{

		/// <summary>
		/// The type of cell to place; used for room creation
		/// </summary>
		public enum CellType
		{
			Empty,
			Path,
			RoomInside,
			RoomOutside
		}


		
		/// <summary>
		/// A simple structure to store the 2D integer position of an object.
		/// </summary>
		public struct Pos
		{
			public int X;
			public int Y;
			public Pos(int x, int y)
			{

				X = x;
				Y = y;
			}

		}

		/// <summary>
		/// 4 Cardinal directions; West, North, East, South
		/// </summary>
		static Pos[] Dirs = { new Pos(-2, 0), new Pos(2, 0), new Pos(0, 2), new Pos(0, -2) };

		public static (int[,] Grid, Stack<Pos> Maze) GenDungeon(int width, int height)
		{
			var emptyGrid = new int [width, height];
			var roomGrid = GenRooms(emptyGrid, 30, 2, width / 4);
			return  GenMaze(roomGrid);

		}

		/// <summary>
		/// Given an initial grid, fill in a maze around it using a randomized depth first algorithm.
		/// </summary>
		/// <param name="initialGrid">The initial grid to fill in a maze around.</param>
		/// <returns>Returns the initial grid with the maze filled in around it.</returns>
		public static (int[,] Grid, Stack<Pos> Maze) GenMaze(int[,] initialGrid)
		{
			var rnd = new Random();
			int width = initialGrid.GetLength(0);
			int height = initialGrid.GetLength(1);
			var CurrentCell = GetRndOddCell(width, height);
			while (initialGrid[CurrentCell.X, CurrentCell.Y] != (int)CellType.Empty)
			{
				CurrentCell = GetRndOddCell(width, height);
			}
			var CurrentPath = new Stack<Pos>();
			var CurrentMaze = new Stack<Pos>();
			int Depth = 0;
			CurrentPath.Push(CurrentCell);
			CurrentMaze.Push(CurrentCell);
			bool Backtracking = false;
			do
			{
				if (CurrentPath.Count == 1 && Backtracking)
				{
					break;
				}
				var newCell = new Pos(0, 0);
				var dir = new Pos(0, 0);
				var dirs = RndDirs(rnd);

				var canMove = false;
				var validDirs = dirs.Where(x => IsValidMove(new Pos(CurrentCell.X + x.X, CurrentCell.Y + x.Y), width, height, CurrentMaze, initialGrid)).ToList();
				//Console.WriteLine(validDirs.Count);
				if (validDirs.Count > 0)
				{
					dir = validDirs[0];
					newCell = new Pos(CurrentCell.X + dir.X, CurrentCell.Y + dir.Y);
					canMove = true;
				}
				else
				{
					canMove = false;
				}

				if (canMove)
				{
					CurrentMaze.Push(new Pos(CurrentCell.X + dir.X / 2, CurrentCell.Y + dir.Y / 2));
					CurrentPath.Push(newCell);
					CurrentMaze.Push(newCell);
					CurrentCell = newCell;
					Depth++;
					Backtracking = false;
				}
				else
				{
					var lastMove = CurrentPath.Peek();
					CurrentPath.Pop();
					CurrentCell = lastMove;
					Depth--;
					Backtracking = true;
				}
				/*displayBoard(grid, Maze.ToArray(), sizeX, sizeY);
				Console.ReadKey();
				Console.Clear();*/



			} while (true);
			var grid = initialGrid;
			foreach (var point in CurrentMaze)
			{
				grid[point.X, point.Y] = (int)CellType.Path;
			}
			// displayBoard(grid, Maze.ToArray(), sizeX, sizeY);
			return (grid, CurrentMaze);
		}

		/// <summary>
		/// Attempt to fill the given grid with random rectangular rooms.
		/// </summary>
		/// <param name="grid">The initial grid to place rectanlges in.</param>
		/// <param name="count">The number of times to attempt placing a rectangle.</param>
		/// <param name="minSize">The minimum size of the rectangle.</param>
		/// <param name="maxSize">The maximum size of the rectangle.</param>
		/// <returns>Returns a boolean showing whether or not the addition succeeded.</returns>
		public static int[,] GenRooms(int[,] grid, int count = 30, int minSize = 3, int maxSize = 10)
		{
			var rnd = new Random();
			int failures = 0;
			for (int i = 0; i < count; i++)
			{

				int width = rnd.Next(minSize, maxSize + 1);
				int height = rnd.Next(minSize, maxSize + 1);
				int x = rnd.Next(1, grid.GetLength(0) - (width+1));
				int y = rnd.Next(1, grid.GetLength(1) - (height+1));
				bool added = AddRoom(ref grid, x, y, width, height);
				if (!added)
				{
					failures++;
				}
			}
			return grid;
		}

		/// <summary>
		/// Attempt to add a rectangular room to a grid. The rectangle should have an area of 2 squares around it which are not occupied, but can be nonexistent; i.e. we are allowed to place a rectangle at the edge..
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns>Returns whether or not the operation was a success, and the room was able to be placed in the grid.</returns>
		public static bool AddRoom(ref int[,] grid, int x, int y, int width, int height)
		{
			int[,] newGrid = (int[,])grid.Clone();
			int gridWidth = grid.GetLength(0);
			int gridHeight = grid.GetLength(1);
			for (int i = -2; i < width + 2; i++)
			{
				for (int j = -2; j < height + 2; j++)
				{
					if (i + x >= gridWidth
						|| j + y >= gridHeight
						|| i + x < 0
						|| j + y < 0)
					{
						if (i >= 0 && i <= width && j >= 0 && j <= height)
						{
							return false;
						}
						else
						{
							continue;
						}

					}

					if (grid[i + x, j + y] != (int)CellType.Empty) return false;
					if (i >= 0 && i <= width && j >= 0 && j <= height)
					{
						if (newGrid[i + x, j + y] == (int)CellType.Empty)
						{
							newGrid[i + x, j + y] = (int)CellType.RoomOutside;
						}
						else
						{
							return false;
						}
					}

				}
			}

			grid = newGrid;
			return true;
		}


		public static void DisplayGrid(int[,] Grid)
		{
			int width = Grid.GetLength(0);
			int height = Grid.GetLength(1);
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					ConsoleColor cellColour;
					switch ((CellType)Grid[x, y])
					{

						case CellType.Path:
							cellColour = ConsoleColor.White;
							break;
						case CellType.RoomOutside:
							cellColour = ConsoleColor.DarkBlue;
							break;
						default:

							cellColour = ((x + y) % 2 == 0) ? ConsoleColor.Black : ConsoleColor.DarkGray;
							break;
					}
					Console.ForegroundColor = cellColour;
					Console.BackgroundColor = cellColour;
					Console.Write(Grid[x, y]);
					Console.ResetColor();
				}
				Console.WriteLine();
			}
		}

		static bool IsValidMove(Pos newCell, int sizeX, int sizeY, Stack<Pos> Maze, int[,] grid)
		{
			var outOfBounds = newCell.X < 1 || newCell.X > sizeX - 1 || newCell.Y < 1 || newCell.Y > sizeY - 1;
			var visited = Maze.Where(x => x.X == newCell.X && x.Y == newCell.Y).ToArray().Length > 0;
			return (!outOfBounds && !visited && grid[newCell.X, newCell.Y] == (int)CellType.Empty);
		}

		static Pos GetRndOddCell(int sizeX, int sizeY)
		{
			var rand = new Random();
			int randX, randY;

			do
			{
				randX = rand.Next(1, sizeX);
				randY = rand.Next(1, sizeY);
			} while (randX % 2 != 1 || randY % 2 != 1);
			var point = new Pos(randX, randY);
			//Console.WriteLine($"{point.X}, {point.Y} ");
			return point;

		}

		static Pos[] RndDirs(Random rnd)
		{
			var dirsList = Dirs.ToList();
			for (int i = 0; i < 10; i++)
			{
				var index = rnd.Next(0, 4);
				var item = dirsList[index];
				dirsList.RemoveAt(index);
				dirsList.Insert(rnd.Next(0, 3), item);
			};
			return dirsList.ToArray();
		}
	}

}

