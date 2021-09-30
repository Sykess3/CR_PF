using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using CodeBase.Infrastructure.Services.Dispatcher;
using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public class PathRequestManager : IService
    {
        private readonly Dictionary<GameObject, PathFinder> _cachedPathFinders =
            new Dictionary<GameObject, PathFinder>();

        private readonly PlaneGrid _grid;
        private readonly IDispatcher _dispatcher;

        public PathRequestManager(PlaneGrid grid, IDispatcher dispatcher)
        {
            _grid = grid;
            _dispatcher = dispatcher;
        }

        public void RequestPath(PathRequest request, GameObject requester)
        {
            if (!_cachedPathFinders.Keys.Contains(requester))
                _cachedPathFinders.Add(requester, new PathFinder(_grid));


            Task.Run(() =>
                GeneratePaths(request, _cachedPathFinders[requester]));
        }

        private void GeneratePaths(PathRequest currentRequest, PathFinder pathFinder)
        {
            Vector3[] waypoints = pathFinder.Find(currentRequest.Start, currentRequest.End);

            _dispatcher.Invoke(() =>
                currentRequest.CallBack(new GridPath(
                    waypoints,
                    currentRequest.Start,
                    currentRequest.TurnDistance,
                    currentRequest.StoppingDistance)));
        }
    }
}