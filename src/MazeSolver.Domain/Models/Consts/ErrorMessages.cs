namespace MazeSolver.Domain.Models.Consts
{
    public static class ErrorMessages
    {
        public const string MazeRunnerNotStarted = "Maze runner not started. To proceed run 'Start()' method.";
        public const string CannotSetState = "Cannot set state for maze runner";
        public const string CannotSetPossibleDirections = "Cannot set possible directions for maze runner";
        public const string CannotSetPosition = "Cannot set position for maze runner";
        public const string ErrorWhileMoving = "Problem occured when moving.";
        public const string ForbiddenDirection = "You can't move this direction.";
        public const string ErrorWhileReset = "Problem occured when reseting";
    }
}