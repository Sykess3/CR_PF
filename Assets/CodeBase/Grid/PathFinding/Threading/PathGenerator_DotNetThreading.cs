using System.Threading.Tasks;
using CodeBase.Infrastructure.Services.Dispatcher;
using UnityEngine;

namespace CodeBase.Grid.PathFinding.Threading
{
    class PathGenerator_DotNetThreading : PathGenerator
    {
        private readonly IDispatcher _dispatcher;

        public PathGenerator_DotNetThreading(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public override void Generate(PathRequest currentRequest, PathFinder pathFinder)
        {
           Task.Run(() =>
               InternalGenerate(currentRequest, pathFinder));
        }

        private void InternalGenerate(PathRequest currentRequest, PathFinder pathFinder)
        {
            Vector3[] waypoints = GeneratePaths(currentRequest, pathFinder);

            _dispatcher.Invoke(() =>
                currentRequest.CallBack(new GridPath(
                    waypoints,
                    currentRequest.Start,
                    currentRequest.TurnDistance,
                    currentRequest.StoppingDistance)
                ));
        }
    }
}