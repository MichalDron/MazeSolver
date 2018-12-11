using System;

using RestSharp;

using MazeSolver.Domain.Infrastructure;
using MazeSolver.Domain.Models;

namespace MazeSolver.Runner.Services
{
    public class MazeService : IMazeService
    {
        private const string ServiceUrl = "http://localhost:3000/";

        public CurrentState GetCurrentState()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "state";

            return this.Execute<CurrentState>(request);
        }

        public Directions GetDirections()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "directions";

            return this.Execute<Directions>(request);
        }

        public Position GetPosition()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "position";

            return this.Execute<Position>(request);
        }

        public void Move(string direction)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "move";
            request.AddJsonBody(direction);

            this.Execute<object>(request);
        }

        public void Reset()
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "reset";

            this.Execute<object>(request);
        }

        private T Execute<T>(RestRequest request) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = new System.Uri(ServiceUrl);

            var response = client.Execute<T>(request);

            if (response.ErrorException != null)
            {
                //TODO log exception response.ErrorException
                throw new Exception("An exception occured while calling service.");
            }
            return response.Data;
        }
    }
}