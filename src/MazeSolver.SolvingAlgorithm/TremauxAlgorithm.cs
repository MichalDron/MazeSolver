using System;
using System.Collections.Generic;
using System.Linq;

using MazeSolver.SolvingAlgorithm.Models;
using MazeSolver.Domain.Models;
using MazeSolver.Domain.Services;

namespace MazeSolver.SolvingAlgorithm
{
    public class TremauxAlgorithm : IMazeSolvingAlgorithm
    {
        private const int NonVisited = 0;
        private const int VisitedOnce = 1;
        private const int VisitedTwice = 2;
        private IMazeRunnerService MazeRunnerService { get; set; }
        private Dictionary<Position, int> VisitedPositions = new Dictionary<Position, int>();

        public TremauxAlgorithm(IMazeRunnerService mazeRunnerService)
        {
            this.MazeRunnerService = mazeRunnerService;
        }

        public void Solve()
        {
            this.ClearState();
            var previousDirection = DirectionsEnum.East;

            while (!this.Solved())
            {
                Position position = this.MazeRunnerService.GetCurrentPosition();
                var possibleDirections = this.MapPossibleDirectionsToEnum(this.MazeRunnerService.GetPossibleDirections());
                var possibleDirectionsVisitedDictionary = this.GetPossibleDirections(position, possibleDirections);
                var chosenDirection = this.ChooseMovingDirection(position, possibleDirectionsVisitedDictionary, previousDirection);
                this.SetCurrentPositionAsVisited(position, possibleDirections, possibleDirectionsVisitedDictionary);
                this.Move(chosenDirection);

                previousDirection = chosenDirection;
            }
        }

        private void MarkDeadEnd(Position position)
        {
            if (!this.VisitedPositions.ContainsKey(position))
            {
                this.VisitedPositions.Add(position, VisitedTwice);
            }
            else
            {
                this.VisitedPositions[position] = VisitedTwice;
            }
        }

        private bool IsDeadJunction(Position position, Dictionary<DirectionsEnum, int> directionsNotVisitedTwice)
        {
            return directionsNotVisitedTwice.Count() == 1;
        }

        private void SetCurrentPositionAsVisited(Position position, List<DirectionsEnum> possibleDirections, Dictionary<DirectionsEnum, int> directionsNotVisitedTwice)
        {
            if (possibleDirections.Count() > 2)
            {
                if (this.IsDeadJunction(position, directionsNotVisitedTwice))
                {
                    this.MarkDeadEnd(position);
                }
                else
                {
                    if (!this.VisitedPositions.ContainsKey(position))
                    {
                        this.VisitedPositions.Add(position, VisitedOnce);
                    }
                }
            }
            else if (possibleDirections.Count() < 2)
            {
                this.MarkDeadEnd(position);
            }
            else
            {
                if (this.VisitedPositions.ContainsKey(position))
                {
                    this.VisitedPositions[position] = VisitedTwice;
                }
                else
                {
                    this.VisitedPositions.Add(position, VisitedOnce);
                }

            }

        }

        private Dictionary<DirectionsEnum, int> GetPossibleDirections(Position currentPosition, List<DirectionsEnum> possibleDirections)
        {
            Dictionary<DirectionsEnum, int> possibleDirectionsVisitedDictionary = this.MapDirectionsToVisitedPossitions(currentPosition, possibleDirections);

            var result = this.FilterDirectionsVisitedTwice(possibleDirectionsVisitedDictionary);

            return result;
        }

        private Dictionary<DirectionsEnum, int> FilterDirectionsVisitedTwice(Dictionary<DirectionsEnum, int> possibleDirectionsVisitedDictionary)
        {
            return possibleDirectionsVisitedDictionary.Where(dv => dv.Value != VisitedTwice).ToDictionary(dv => dv.Key, dv => dv.Value);
        }

        private Dictionary<DirectionsEnum, int> MapDirectionsToVisitedPossitions(Position currentPosition, List<DirectionsEnum> possibleDirections)
        {
            var possibleDirectionsVisitedDictionary = new Dictionary<DirectionsEnum, int>();

            possibleDirections.ForEach(direction =>
            {
                possibleDirectionsVisitedDictionary.Add(direction, this.GetVisitationNumber(currentPosition, direction));
            });

            return possibleDirectionsVisitedDictionary;
        }

        private int GetVisitationNumber(Position currentPosition, DirectionsEnum direction)
        {
            var futurePosition = this.CalculateFuturePosition(currentPosition, direction);

            if (!this.VisitedPositions.ContainsKey(futurePosition))
            {
                return NonVisited;
            }

            return this.VisitedPositions[futurePosition];
        }

        private List<DirectionsEnum> MapPossibleDirectionsToEnum(Directions directions)
        {
            var possibleDirections = new List<DirectionsEnum>();

            if (directions.East)
            {
                possibleDirections.Add(DirectionsEnum.East);
            }

            if (directions.North)
            {
                possibleDirections.Add(DirectionsEnum.North);
            }

            if (directions.South)
            {
                possibleDirections.Add(DirectionsEnum.South);
            }

            if (directions.West)
            {
                possibleDirections.Add(DirectionsEnum.West);
            }

            return possibleDirections;
        }

        private DirectionsEnum ChooseMovingDirection(Position currentPosition, Dictionary<DirectionsEnum, int> possibleDirections, DirectionsEnum previousDirection)
        {
            if (!possibleDirections.Any())
            {
                throw new Exception("Maze does not have any solution!");
            }

            if (possibleDirections.Values.Any(v => v == NonVisited))
            {
                return possibleDirections.First(kv => kv.Value == NonVisited).Key;
            }

            if (!this.VisitedPositions.ContainsKey(currentPosition) || !possibleDirections.ContainsKey(previousDirection))
            {
                if (possibleDirections.Values.Count(v => v == VisitedOnce) == possibleDirections.Count() && possibleDirections.Count() > 1)
                {
                    var oppositeDirection = this.GetOppositeDirection(previousDirection);
                    if (possibleDirections.ContainsKey(oppositeDirection))
                    {
                        return oppositeDirection;
                    }
                }

                return possibleDirections.Keys.OrderBy(d => (int)d).First();
            }
            else
            {
                return previousDirection;
            }
        }

        private DirectionsEnum GetOppositeDirection(DirectionsEnum direction)
        {
            switch (direction)
            {
                case DirectionsEnum.East: return DirectionsEnum.West;
                case DirectionsEnum.North: return DirectionsEnum.South;
                case DirectionsEnum.South: return DirectionsEnum.North;
                case DirectionsEnum.West: return DirectionsEnum.East;
                default:
                    throw new Exception("Enum out of range");
            }
        }

        private Position CalculateFuturePosition(Position currentPosition, DirectionsEnum direction)
        {
            var newPosition = new Position();

            switch (direction)
            {
                case DirectionsEnum.East:
                    newPosition.X = currentPosition.X + 1;
                    newPosition.Y = currentPosition.Y;
                    break;
                case DirectionsEnum.North:
                    newPosition.X = currentPosition.X;
                    newPosition.Y = currentPosition.Y - 1;
                    break;
                case DirectionsEnum.South:
                    newPosition.X = currentPosition.X;
                    newPosition.Y = currentPosition.Y + 1;
                    break;
                case DirectionsEnum.West:
                    newPosition.X = currentPosition.X - 1;
                    newPosition.Y = currentPosition.Y;
                    break;
            }

            return newPosition;
        }

        private void Move(DirectionsEnum chosenDirection)
        {
            switch (chosenDirection)
            {
                case DirectionsEnum.East:
                    this.MazeRunnerService.MoveEast();
                    break;
                case DirectionsEnum.North:
                    this.MazeRunnerService.MoveNorth();
                    break;
                case DirectionsEnum.South:
                    this.MazeRunnerService.MoveSouth();
                    break;
                case DirectionsEnum.West:
                    this.MazeRunnerService.MoveWest();
                    break;
            }
        }

        private void ClearState()
        {
            this.VisitedPositions = new Dictionary<Position, int>();
        }

        private bool Solved()
        {
            return this.MazeRunnerService.Finished();
        }
    }
}
