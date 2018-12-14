using System;
using Xunit;
using Moq;
using System.Collections.Generic;

using MazeSolver.Domain.Models;
using MazeSolver.Domain.Models.Consts;
using MazeSolver.Domain.Services;
using MazeSolver.Domain.Infrastructure;

namespace MazeSolver.Domain.Tests.Services
{
    public class MazeRunnerServiceTests
    {
        private Mock<IMazeService> MazeServiceMock;
        private IMazeRunnerService _sut;

        public MazeRunnerServiceTests()
        {
            this.MazeServiceMock = new Mock<IMazeService>();
            this._sut = new MazeRunnerService(MazeServiceMock.Object);
        }

        [Theory]
        [MemberData(nameof(MethodsThrowingExceptionWithoutStartTestData))]
        public void GivenNotStartedMazeRunnerService_WhenMove_ThenThrowException(Action<IMazeRunnerService> action)
        {
            // Arrange
            this._sut = new MazeRunnerService(MazeServiceMock.Object);

            // Assert & Act
            var exception = Assert.Throws<Exception>(() => action(this._sut));
            Assert.Equal(ErrorMessages.MazeRunnerNotStarted, exception.Message);
        }

        public static IEnumerable<object[]> MethodsThrowingExceptionWithoutStartTestData()
        {
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.MoveEast() };
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.MoveNorth() };
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.MoveSouth() };
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.MoveWest() };
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.Finished() };
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.GetCurrentState() };
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.GetCurrentPosition() };
            yield return new Action<IMazeRunnerService>[] { (sut) => sut.GetPossibleDirections() };
        }
    }
}
