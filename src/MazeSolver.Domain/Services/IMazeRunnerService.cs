using System;
using MazeSolver.Domain.Models;
using MazeSolver.Domain.Infrastructure;
using MazeSolver.Domain.Models.Consts;

namespace MazeSolver.Domain.Services
{
    public interface IMazeRunnerService
    {
        CurrentState GetCurrentState();
        Position GetCurrentPosition();
        Directions GetPossibleDirections();
        void MoveEast();
        void MoveNorth();
        void MoveSouth();
        void MoveWest();
        void Reset();
        void Start();
        bool Finished();
    }
}