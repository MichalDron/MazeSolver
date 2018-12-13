using System;
using MazeSolver.Domain.Services;
using MazeSolver.Domain.Models;
using MazeSolver.Runner.Services;
using MazeSolver.SolvingAlgorithm;

namespace MazeSolver.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            IMazeRunnerService mazeRunnerService = new MazeRunnerService(new MazeService());
            IMazeSolvingAlgorithm algorithm = new TremauxAlgorithm(mazeRunnerService);
            Game game = new Game(mazeRunnerService, algorithm);

            game.Start();
        }
    }
}
