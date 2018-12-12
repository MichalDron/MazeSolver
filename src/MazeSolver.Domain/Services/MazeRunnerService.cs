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
            return this.MazeRunner.GetCurrentState();
        }

        public Position GetCurrentPosition()
        {
            return this.MazeRunner.GetCurrentPosition();
        }

        public Directions GetPossibleDirections()
        {
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
            catch (Exception ex)
            {
                //TODO log exception ex
                throw new Exception("Problem occured when moving.");
            }

            this.Start();
        }

        public void Start()
        {
            this.MazeRunner = new MazeRunnerActor();
            this.UpdateMazeRunnerActor();
        }

        private void Move(string direction)
        {
            if (!this.MazeRunner.CanMove(direction))
            {
                throw new Exception("You can't move this direction.");
            }

            try
            {
                this.MazeService.Move(direction);
            }
            catch (Exception ex)
            {
                //TODO log exception ex
                throw new Exception("Problem occured when moving.");
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
            catch (Exception ex)
            {
                //TODO log exception ex
                throw new Exception("Cannot set position for maze runner");
            }
        }

        private void UpdateMazeRunnerPossibleDirections()
        {
            try
            {
                Directions directions = this.MazeService.GetDirections();
                this.MazeRunner.SetPossibleDirections(directions);
            }
            catch (Exception ex)
            {
                //TODO log exception ex
                throw new Exception("Cannot set possible directions for maze runner");
            }
        }

        private void UpdateMazeRunnerState()
        {
            try
            {
                CurrentState state = this.MazeService.GetCurrentState();
                this.MazeRunner.SetState(state);
            }
            catch (Exception ex)
            {
                //TODO log exception ex
                throw new Exception("Cannot set state for maze runner");
            }
        }
    }
}