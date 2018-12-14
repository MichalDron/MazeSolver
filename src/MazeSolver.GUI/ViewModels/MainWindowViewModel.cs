using System;

using ReactiveUI;

using MazeSolver.GUI.Models;
using MazeSolver.Domain.Services;
using MazeSolver.Domain.Models;
using MazeSolver.SolvingAlgorithm;

namespace MazeSolver.GUI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IMazeRunnerService MazeRunnerService { get; set; }
        private IMazeSolvingAlgorithm MazeSolvingAlgorithm { get; set; }

        public ReactiveCommand StartCommand { get; }
        public ReactiveCommand RunSolvingAlgorithmCommand { get; }
        public ReactiveCommand MoveEastCommand { get; }
        public ReactiveCommand MoveNorthCommand { get; }
        public ReactiveCommand MoveSouthCommand { get; }
        public ReactiveCommand MoveWestCommand { get; }

        private Directions _possibleDirections;
        public Directions PossibleDirections
        {
            get => _possibleDirections;
            set => this.RaiseAndSetIfChanged(ref _possibleDirections, value);
        }

        private bool _canMove;
        public bool CanMove
        {
            get => _canMove;
            set => this.RaiseAndSetIfChanged(ref _canMove, value);
        }

        private string _infoMessage = "";
        public string InfoMessage
        {
            get => _infoMessage;
            set => this.RaiseAndSetIfChanged(ref _infoMessage, value);
        }

        public MainWindowViewModel()
        {
            this.StartCommand = ReactiveCommand.Create(this.Start);
            this.RunSolvingAlgorithmCommand = ReactiveCommand.Create(this.RunSolvingAlgorithm);
            this.MoveEastCommand = ReactiveCommand.Create(this.MoveEast);
            this.MoveNorthCommand = ReactiveCommand.Create(this.MoveNorth);
            this.MoveSouthCommand = ReactiveCommand.Create(this.MoveSouth);
            this.MoveWestCommand = ReactiveCommand.Create(this.MoveWest);

            this.InitServices();
        }

        private void InitServices()
        {
            IMazeRunnerService mazeRunnerService = new MazeRunnerService(new MazeService());
            IMazeSolvingAlgorithm algorithm = new TremauxAlgorithm(mazeRunnerService);
            this.MazeRunnerService = mazeRunnerService;
            this.MazeSolvingAlgorithm = algorithm;
        }

        private void Start()
        {
            try
            {
                this.MazeRunnerService.Reset();
                this.MazeRunnerService.Start();
                this.PossibleDirections = this.MazeRunnerService.GetPossibleDirections();
                this.CanMove = true;
                this.InfoMessage = "";
            }
            catch (Exception)
            {
                this.InfoMessage = "Error occured. Try again later";
            }
        }

        private void RunSolvingAlgorithm()
        {
            try
            {
                this.MazeSolvingAlgorithm.Solve();
                this.AfterMoveAction();
            }
            catch (Exception)
            {
                this.InfoMessage = "Error occured. Try again later";
            }
        }

        private void MoveEast()
        {
            try
            {
                this.MazeRunnerService.MoveEast();
                this.AfterMoveAction();
            }
            catch (Exception)
            {
                this.InfoMessage = "Error occured. Try again later";
            }
        }

        private void MoveNorth()
        {
            try
            {
                this.MazeRunnerService.MoveNorth();
                this.AfterMoveAction();
            }
            catch (Exception)
            {
                this.InfoMessage = "Error occured. Try again later";
            }
        }

        private void MoveSouth()
        {
            try
            {
                this.MazeRunnerService.MoveSouth();
                this.AfterMoveAction();
            }
            catch (Exception)
            {
                this.InfoMessage = "Error occured. Try again later";
            }
        }

        private void MoveWest()
        {
            try
            {
                this.MazeRunnerService.MoveWest();
                this.AfterMoveAction();
            }
            catch (Exception)
            {
                this.InfoMessage = "Error occured. Try again later";
            }
        }

        private void AfterMoveAction()
        {
            if (this.MazeRunnerService.Finished())
            {
                this.InfoMessage = "Finished";
                this.CanMove = false;
            }
            if (!CanMove)
            {
                this.DisableAllDirectionsMove();
            }
            else
            {
                this.PossibleDirections = this.MazeRunnerService.GetPossibleDirections();
            }
        }

        private void DisableAllDirectionsMove()
        {
            this.PossibleDirections = new Directions
            {
                East = false,
                North = false,
                South = false,
                West = false
            };
        }
    }
}