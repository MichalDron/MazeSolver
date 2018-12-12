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
            IMazeRunnerService mazeRunnerService = new MazeRunnerService(new MazeService());
            Game game = new Game(mazeRunnerService);

            game.Start();
        }
    }
}
