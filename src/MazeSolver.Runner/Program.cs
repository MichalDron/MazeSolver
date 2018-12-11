using System;
using MazeSolver.Domain.Services;
using MazeSolver.Domain.Models;
using MazeSolver.Runner.Services;

namespace MazeSolver.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var mazeRunnerService = new MazeRunnerService(new MazeService());
            Console.WriteLine("Hello to Maze Solver");
            Console.WriteLine("Left arrow");
            Console.WriteLine("Right arrow");
            Console.WriteLine("Up arrow");
            Console.WriteLine("Down arrow");
            Console.WriteLine("'r' for reset");

            try{
                mazeRunnerService.Start();
            }
            catch(Exception ex){
                Console.WriteLine(ex.Message);
            }

            ConsoleKeyInfo input;
            do
            {
                input = Console.ReadKey();
                switch (input.Key) {
                    case ConsoleKey.R: { mazeRunnerService.Reset(); LogStatePostionAndKey(mazeRunnerService); break; }
                    case ConsoleKey.RightArrow: { mazeRunnerService.MoveEast(); LogStatePostionAndKey(mazeRunnerService); break; }
                    case ConsoleKey.UpArrow: { mazeRunnerService.MoveNorth(); LogStatePostionAndKey(mazeRunnerService); break; }
                    case ConsoleKey.DownArrow: { mazeRunnerService.MoveSouth(); LogStatePostionAndKey(mazeRunnerService); break; }
                    case ConsoleKey.LeftArrow: { mazeRunnerService.MoveWest(); LogStatePostionAndKey(mazeRunnerService); break; }
                }
            } while (input.Key != ConsoleKey.Escape);
        }

        static void LogStatePostionAndKey(MazeRunnerService mazeRunnerService){
            var state = mazeRunnerService.GetCurrentState();
            var position = mazeRunnerService.GetCurrentPosition();
            var directions = mazeRunnerService.GetPossibleDirections();
            Console.WriteLine($"State: {state.State}, Position: X:{position.X} Y:{position.Y}, Possible Directions: East:{directions.East} North:{directions.North} South:{directions.South} West:{directions.West};");
        }
    }
}
