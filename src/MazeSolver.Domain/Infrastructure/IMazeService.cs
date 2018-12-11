using System;
using MazeSolver.Domain.Models;

namespace MazeSolver.Domain.Infrastructure
{
    public interface IMazeService
    {
        void Move(string direction);
        void Reset();
        Directions GetDirections();
        CurrentState GetCurrentState();
        Position GetPosition();
    }
}
