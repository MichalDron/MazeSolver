using System;
using MazeSolver.Domain.Services;
using MazeSolver.Domain.Models;
using MazeSolver.Runner.Services;
using MazeSolver.SolvingAlgorithm;

namespace MazeSolver.Runner
{
    public class Game
    {
        private IMazeRunnerService MazeRunnerService { get; set; }
        private IMazeSolvingAlgorithm MazeSolvingAlgorithm { get; set; }

        public Game(IMazeRunnerService mazeRunnerService, IMazeSolvingAlgorithm mazeSolvingAlgorithm = null)
        {
            this.MazeRunnerService = mazeRunnerService;
            this.MazeSolvingAlgorithm = mazeSolvingAlgorithm;
        }

        public void Start()
        {
            this.ShowGameRules();

            try
            {
                this.MazeRunnerService.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ConsoleKeyInfo input;
            do
            {
                input = Console.ReadKey();
                this.PlayGame(input);
            } while (input.Key != ConsoleKey.Escape);
        }

        private void LogStatePostionAndKey()
        {
            var state = this.MazeRunnerService.GetCurrentState();
            var position = this.MazeRunnerService.GetCurrentPosition();
            var directions = this.MazeRunnerService.GetPossibleDirections();
            Console.WriteLine($"State: {state.State}, Position: X:{position.X} Y:{position.Y}, Possible Directions: East:{directions.East} North:{directions.North} South:{directions.South} West:{directions.West};");
        }

        private void PlayGame(ConsoleKeyInfo input)
        {
            try
            {
                switch (input.Key)
                {
                    case ConsoleKey.R:
                        this.MazeRunnerService.Reset();
                        LogStatePostionAndKey();
                        break;
                    case ConsoleKey.RightArrow:
                        this.MazeRunnerService.MoveEast();
                        LogStatePostionAndKey();
                        break;
                    case ConsoleKey.UpArrow:
                        this.MazeRunnerService.MoveNorth();
                        LogStatePostionAndKey();
                        break;
                    case ConsoleKey.DownArrow:
                        this.MazeRunnerService.MoveSouth();
                        LogStatePostionAndKey();
                        break;
                    case ConsoleKey.LeftArrow:
                        this.MazeRunnerService.MoveWest();
                        LogStatePostionAndKey();
                        break;
                    case ConsoleKey.S:
                        this.MazeSolvingAlgorithm.Solve();
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void ShowGameRules()
        {
            Console.WriteLine("Hello to Maze Solver");
            Console.WriteLine("Left arrow");
            Console.WriteLine("Right arrow");
            Console.WriteLine("Up arrow");
            Console.WriteLine("Down arrow");
            Console.WriteLine("'r' for reset");
            if (this.MazeSolvingAlgorithm != null)
            {
                Console.WriteLine("'s' for auto solving");
            }
        }
    }
}