

using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public abstract class PathGenerator
    {
        public abstract void Generate(PathRequest currentRequest, PathFinder pathFinder);
        
        protected Vector3[] GeneratePaths(PathRequest currentRequest, PathFinder pathFinder, out int pathLengthCost) => 
            pathFinder.Find(currentRequest.Start, currentRequest.End, out pathLengthCost);
    }
}