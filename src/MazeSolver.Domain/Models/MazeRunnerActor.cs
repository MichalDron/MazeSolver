using System;
using MazeSolver.Domain.Models.Consts;

namespace MazeSolver.Domain.Models
{
    internal class MazeRunnerActor
    {
        private Position CurrentPosition { get; set; }
        private Directions PossibleDirections { get; set; }
        private CurrentState CurrentState { get; set; }

        internal bool CanMove(string direction)
        {
            switch (direction)
            {
                case Direction.East: return PossibleDirections.East;
                case Direction.North: return PossibleDirections.North;
                case Direction.South: return PossibleDirections.South;
                case Direction.West: return PossibleDirections.West;
                default:
                    {
                        //TODO log exception
                        throw new Exception($"This {direction} is not an option");
                    }
            }
        }

        internal void SetPosition(Position position)
        {
            this.CurrentPosition = position;
        }

        internal void SetPossibleDirections(Directions directions)
        {
            this.PossibleDirections = directions;
        }

        internal void SetState(CurrentState state)
        {
            this.CurrentState = state;
        }

        internal Directions GetPossibleDirections()
        {
            return this.PossibleDirections;
        }

        internal Position GetCurrentPosition()
        {
            return this.CurrentPosition;
        }

        internal CurrentState GetCurrentState()
        {
            return this.CurrentState;
        }

        internal bool Finished()
        {
            return this.CurrentState.State == State.TargetReached;
        }
    }
}