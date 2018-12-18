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
        static Pos[] Dirs = { new Pos(-2, 0), new Pos(2, 0), new Pos(0, 2), new Pos(0, -2) };



        public static Stack<Pos> GenMaze(int sizeX, int sizeY)
        {
            var rnd = new Random();
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
                var dirs = rndDirs(rnd);
                
                var canMove = false;
                var validDirs = dirs.Where(x => IsValidMove(new Pos(CurrentCell.X + x.X, CurrentCell.Y + x.Y), sizeX, sizeY, Maze)).ToList();
                Console.WriteLine(validDirs.Count);
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
     


            } while (true);
            var grid = new int[sizeX, sizeY];
            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    grid[x, y] = 0;
                }
            }
                displayBoard(grid, Maze.ToArray(), sizeX, sizeY);
            return Maze;
        }

        static bool IsValidMove(Pos newCell, int sizeX, int sizeY, Stack<Pos> Maze)
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
            var rand = new Random();
            int randX, randY;

            do
            {
                randX = rand.Next(1, sizeX);
                randY = rand.Next(1, sizeY);
            } while (randX % 2 != 1 || randY % 2 != 1);
            var point = new Pos(randX, randY);
            Console.WriteLine($"{point.X}, {point.Y} ");
            return point;

        }

        static Pos[] rndDirs(Random rnd)
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

