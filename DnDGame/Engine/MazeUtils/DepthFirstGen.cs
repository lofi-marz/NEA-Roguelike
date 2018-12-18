using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DnDGame.Engine;
namespace DnDGame.MazeGen.DepthFirst
{





    static class DepthFirst
    {

        public class Pos
        {
            public int X { get => XY[0]; set => XY[0] = value; }
            public int Y { get => XY[1]; set => XY[1] = value; }
            public int[] XY;
            public Pos(int x, int y)
            {
                XY = new int[2];
                X = x;
                Y = y;
            }
            public Pos(int[] xy)
            {
                XY = xy;

            }
        }
        static Pos[] Dirs = { new Pos(0, 2), new Pos(0, -2), new Pos(-2, 0), new Pos(2, 0) };



        public static Stack<Pos> GenMaze(int sizeX, int sizeY)
        {

            var CurrentCell = rndOddCell(sizeX, sizeY);
            var Path = new Stack<Pos>();
            var Maze = new Stack<Pos>();
            int Depth = 0;
            Path.Push(CurrentCell);
            Maze.Push(CurrentCell);
            bool Backtracking = false;

            do
            {
                if (Path.Count == 1 && Backtracking)
                {
                    break;
                }
                var newCell = new Pos(0, 0);
                var dir = new Pos(0, 0);
                var dirs = rndDirs();
                var canMove = false;
                var validDirs = dirs.Where(x => isValidMove(new Pos(CurrentCell.X + x.X, CurrentCell.Y + x.Y), sizeX, sizeY, Maze)).ToList();
                if (validDirs.Count > 0)
                {
                    dir = validDirs[0];
                    newCell = new Pos(CurrentCell.X + dir.X, CurrentCell.Y + dir.Y);
                    canMove = true;
                } else
                {
                    canMove = false;
                }
               
                if (canMove)
                {
                    Maze.Push(new Pos(CurrentCell.X + dir.X / 2, CurrentCell.Y + dir.Y / 2));
                    Path.Push(newCell);
                    Maze.Push(newCell);
                    CurrentCell = newCell;
                    Depth++;
                    Backtracking = false;
                }
                else
                {
                    var lastMove = Path.Peek();
                    Path.Pop();
                    CurrentCell = lastMove;
                    Depth--;
                    Backtracking = true;
                }
                /*displayBoard(grid, Maze.ToArray(), sizeX, sizeY);
                Console.ReadKey();
                Console.Clear();*/
                /*var grid = new int[size, size];
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        grid[x, y] = 0;
                    }
                }
                if (!Backtracking)
                {
                    Console.Clear();
                    displayBoard(grid, Maze.ToArray());
                }*/


            } while (true);
            return Maze;
        }

        static bool isValidMove(Pos newCell, int sizeX, int sizeY, Stack<Pos> Maze)
        {
            var outOfBounds  = newCell.X < 1 || newCell.X > sizeX - 1 || newCell.Y < 1 || newCell.Y > sizeY - 1;
            var visited = Maze.Where(x => x.X == newCell.X && x.Y == newCell.Y).ToArray().Length > 0;
            return (!outOfBounds && !visited);
        }


        static void displayBoard(int[,] board, Pos[] maze, int sizeX, int sizeY)
        {
            for (int i = 0; i < maze.Length; i++)
            {
                Pos cell = maze[i];
                board[cell.X, cell.Y] = 1;
            }
            for (var y = 0; y < sizeY; y++)
            {
                for (var x = 0; x < sizeX; x++)
                {

                    var cell = board[x, y];
                    if (cell > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.White;
                    }
                    else if ((x + y) % 2 == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Black;
                    }


                    Console.Write(cell);

                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        static Pos rndOddCell(int sizeX, int sizeY)
        {

            var point = new Pos(new int[] { new Random().Next(0, (sizeX - 1) / 2), new Random().Next(0, (sizeY - 1) / 2) }.Select(x => (2 * x) + 1).ToArray());
            Console.WriteLine($"{point.X}, {point.Y} ");
            return point;

        }

        static Pos[] rndDirs()
        {
            var dirsList = Dirs.ToList();
            var rnd = new Random();
            

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

