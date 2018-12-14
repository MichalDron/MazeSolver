using System;
using MazeSolver.Domain.Models;
using MazeSolver.Domain.Infrastructure;
using MazeSolver.Domain.Models.Consts;

namespace MazeSolver.Domain.Services
{
    public class MazeRunnerService : IMazeRunnerService
    {
        private MazeRunnerActor MazeRunner { get; set; }
        private IMazeService MazeService;

        public MazeRunnerService(IMazeService mazeService)
        {
            this.MazeService = mazeService;
        }

        public CurrentState GetCurrentState()
        {
            this.ValidateStarted();

            return this.MazeRunner.GetCurrentState();
        }

        public Position GetCurrentPosition()
        {
            this.ValidateStarted();

            return this.MazeRunner.GetCurrentPosition();
        }

        public Directions GetPossibleDirections()
        {
            this.ValidateStarted();

            return this.MazeRunner.GetPossibleDirections();
        }

        public void MoveEast()
        {
            this.Move(Direction.East);
        }

        public void MoveNorth()
        {
            this.Move(Direction.North);
        }

        public void MoveSouth()
        {
            this.Move(Direction.South);
        }

        public void MoveWest()
        {
            this.Move(Direction.West);
        }

        public void Reset()
        {
            try
            {
                this.MazeService.Reset();
            }
            catch (Exception)
            {
                //TODO log exception ex
                throw new Exception(ErrorMessages.ErrorWhileReset);
            }

            this.Start();
        }

        public void Start()
        {
            this.MazeRunner = new MazeRunnerActor();
            this.UpdateMazeRunnerActor();
        }

        public bool Finished()
        {
            this.ValidateStarted();

            return this.MazeRunner.Finished();
        }

        private void Move(string direction)
        {
            this.ValidateStarted();

            if (!this.MazeRunner.CanMove(direction))
            {
                throw new Exception(ErrorMessages.ForbiddenDirection);
            }

            try
            {
                this.MazeService.Move(direction);
            }
            catch (Exception)
            {
                //TODO log exception ex
                throw new Exception(ErrorMessages.ErrorWhileMoving);
            }

            this.UpdateMazeRunnerActor();
        }

        private void UpdateMazeRunnerActor()
        {
            this.UpdateMazeRunnerPosition();
            this.UpdateMazeRunnerPossibleDirections();
            this.UpdateMazeRunnerState();
        }

        private void UpdateMazeRunnerPosition()
        {
            try
            {
                Position position = this.MazeService.GetPosition();
                this.MazeRunner.SetPosition(position);
            }
            catch (Exception)
            {
                //TODO log exception ex
                throw new Exception(ErrorMessages.CannotSetPosition);
            }
        }

        private void UpdateMazeRunnerPossibleDirections()
        {
            try
            {
                Directions directions = this.MazeService.GetDirections();
                this.MazeRunner.SetPossibleDirections(directions);
            }
            catch (Exception)
            {
                //TODO log exception ex
                throw new Exception(ErrorMessages.CannotSetPossibleDirections);
            }
        }

        private void UpdateMazeRunnerState()
        {
            try
            {
                CurrentState state = this.MazeService.GetCurrentState();
                this.MazeRunner.SetState(state);
            }
            catch (Exception)
            {
                //TODO log exception ex
                throw new Exception(ErrorMessages.CannotSetState);
            }
        }

        private void ValidateStarted()
        {
            if (this.MazeRunner == null)
            {
                throw new Exception(ErrorMessages.MazeRunnerNotStarted);
            }
        }
    }
}