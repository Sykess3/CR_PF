using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public class PathRequestManager : IService
    {
        private readonly Dictionary<GameObject, PathFinder> _cachedPathFinders =
            new Dictionary<GameObject, PathFinder>();

        private readonly PlaneGrid _grid;
        private readonly PathGenerator _pathGenerator;

        public PathRequestManager(PlaneGrid grid, PathGenerator pathGenerator)
        {
            _grid = grid;
            _pathGenerator = pathGenerator;
        }

        public void RequestPath(PathRequest request, GameObject requester)
        {
            if (!_cachedPathFinders.Keys.Contains(requester))
                _cachedPathFinders.Add(requester, new PathFinder(_grid));


            _pathGenerator.Generate(request, _cachedPathFinders[requester]); 
        }
        
    }
}