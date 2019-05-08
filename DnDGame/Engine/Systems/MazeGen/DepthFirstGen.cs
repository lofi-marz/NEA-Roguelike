using System;
using System.Collections.Generic;
using System.Linq;

namespace DnDGame.Engine.Systems.MazeGen
{
	/// <summary>
	/// Randomized Iterative Depth first maze generation.
	/// </summary>
	static class DepthFirst
	{

		/// <summary>
		/// The type of cell to place; used for room creation.
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
			var CurrentPath = new Stack<Pos>(); //Stores the current path the algorithm is travelling down.
			var CurrentMaze = new Stack<Pos>(); //Stores the entire path the algorithm has traversed through.
			int Depth = 0;
			CurrentPath.Push(CurrentCell);
			CurrentMaze.Push(CurrentCell);
			bool Backtracking = false;
			do
			{

				var newCell = new Pos(0, 0);
				var dir = new Pos(0, 0);
				var dirs = RndDirs(rnd);
				var canMove = false;

				var validDirs = dirs.Where(x =>
				IsValidMove(new Pos(CurrentCell.X + x.X, CurrentCell.Y + x.Y),
				width,
				height,
				CurrentMaze,
				initialGrid)).ToList(); //Get the valid directions we can go in.

				if (validDirs.Count > 0) //If there are directions we can go in, since they are already randomized, we can just take the first one.
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
					CurrentMaze.Push(new Pos(CurrentCell.X + dir.X / 2, CurrentCell.Y + dir.Y / 2)); //Add in the cell inbetween.
					CurrentPath.Push(newCell);
					CurrentMaze.Push(newCell);
					CurrentCell = newCell;
					Depth++;
					Backtracking = false;
				}
				else
				{
					var lastMove = CurrentPath.Peek(); //Backtracking, go to the last point in the current path.
					CurrentPath.Pop();
					CurrentCell = lastMove;
					Depth--;
					Backtracking = true;
				}

			} while (!(CurrentPath.Count == 1 && Backtracking));

			var grid = initialGrid;
			foreach (var point in CurrentMaze) //Add the path of the maze to the initial grid.
			{
				grid[point.X, point.Y] = (int)CellType.Path;
			}
			// displayBoard(grid, Maze.ToArray(), sizeX, sizeY);
			DisplayGrid(grid);
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
		public static int[,] GenRooms(int[,] grid, int count = 30, int minSize = 5, int maxSize = 10)
		{
			var rnd = new Random();
			int failures = 0;
			for (int i = 0; i < count; i++) //Attempts to place count rooms; unlikely to reach 30, but the number of rooms generated is unimportant.
			{
				int width, height, x, y;
				do
				{
					width = rnd.Next(minSize, maxSize + 1);
					height = rnd.Next(minSize, maxSize + 1);
					x = rnd.Next(1, grid.GetLength(0) - (width + 1));
					y = rnd.Next(1, grid.GetLength(1) - (height + 1)); //NB: since x and y are indexes in a grid, {0,0} is the first position
				} while ((x % 2 == 1) //x odd
				|| (y % 2 == 1)); //y odd


				bool added = AddRoom(ref grid, x, y, width, height);
				if (!added) failures++;
			}
			return grid;
		}

		/// <summary>
		/// Attempt to add a rectangular room to a grid. The rectangle should have an area of 2 squares around it which are not occupied, but can be nonexistent; i.e. we are allowed to place a rectangle at the edge.
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
					if (i + x >= gridWidth //Border of 2 around the room wherewe  won't place anything
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

					if (grid[i + x, j + y] != (int)CellType.Empty) return false; //Only place a room there if there is nothing there already
					if (i >= 0 && i <= width && j >= 0 && j <= height)
					{
						if (newGrid[i + x, j + y] == (int)CellType.Empty)
						{
							newGrid[i + x, j + y] = (int)CellType.RoomOutside; //Outline of the room
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

		/// <summary>
		/// Display the given grid, assuming it is a dungeon grid.
		/// </summary>
		/// <param name="Grid">The dungeon to display.</param>
		public static void DisplayGrid(int[,] Grid) //Used for debug purposes
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
				Console.ResetColor();
			}
		}

		/// <summary>
		/// Check if the new cell is a valid cell to move to. i.e. within the grid and unvisited.
		/// </summary>
		/// <param name="newCell"></param>
		/// <param name="sizeX"></param>
		/// <param name="sizeY"></param>
		/// <param name="Maze"></param>
		/// <param name="grid"></param>
		/// <returns></returns>
		static bool IsValidMove(Pos newCell, int sizeX, int sizeY, Stack<Pos> Maze, int[,] grid)
		{
			var outOfBounds = newCell.X < 1 || newCell.X > sizeX - 1 || newCell.Y < 1 || newCell.Y > sizeY - 1;
			var visited = Maze.Where(x => x.X == newCell.X && x.Y == newCell.Y).ToArray().Length > 0;
			return (!outOfBounds && !visited && grid[newCell.X, newCell.Y] == (int)CellType.Empty); //If the cell is within the grid, unvisited and not part of a room, then we can move there.
		}

		/// <summary>
		/// Get a random odd x and y positon in a grid
		/// </summary>
		/// <param name="sizeX">The maximum x value.</param>
		/// <param name="sizeY">The maximum y value.</param>
		/// <returns>A random x and y point within the grid.</returns>
		static Pos GetRndOddCell(int sizeX, int sizeY)
		{
			var rand = new Random();
			int randX, randY;

			do 
			{
				randX = rand.Next(1, sizeX);
				randY = rand.Next(1, sizeY);
			} while (randX % 2 != 1 || randY % 2 != 1); //Keep going till we get an odd position
			return new Pos(randX, randY);
		}

		/// <summary>
		/// Shuffles the 4 cardinal direction vectors and returns the randomized array.
		/// </summary>
		/// <param name="rnd">The instance of the random class to use.</param>
		/// <returns>The 4 cardinal directions as vectos in a random order.</returns>
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

