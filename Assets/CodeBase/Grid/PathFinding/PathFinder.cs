using System;
using System.Collections.Generic;
using CodeBase.DataStructures;
using UnityEngine;

namespace CodeBase.Grid.PathFinding
{
    public class PathFinder 
    {
        public const int DiagonalCost = 14;
        public const int HorizontalOrVerticalCost = 10;

        private readonly HashSet<Node> _closedNodes = new HashSet<Node>();
        private readonly Heap<Node> _openNodes;

        private readonly PlaneGrid _grid;

        public PathFinder(PlaneGrid grid)
        {
            _grid = grid;
            _openNodes = new Heap<Node>(grid.Size);
        }
        

        public Vector3[] Find(Vector3 from, Vector3 to)
        {
            Node start = _grid.NodeFromWorldPosition(from);
            Node target = _grid.NodeFromWorldPosition(to);
            if (start.Walkable && target.Walkable)
            {
                _openNodes.Add(start);

                AStartFind(target);
                return RetracePath(start, target);
            }

            return new Vector3[0];
        }

        private void AStartFind(Node target)
        {
            while (_openNodes.Count > 0)
            {
                Node current = _openNodes.RemoveFirst();
                _closedNodes.Add(current);

                if (current == target)
                    return;

                foreach (Node neighbour in _grid.NeighboursFor(current))
                {
                    if (TraversableOrClosed(neighbour))
                        continue;

                    OpenNodeOrMinimizeF_Cost(current, neighbour, target);
                }
            }

            bool TraversableOrClosed(Node node)
            {
                return !node.Walkable || _closedNodes.Contains(node);
            }
        }

        private void OpenNodeOrMinimizeF_Cost(Node current, Node neighbour, Node target)
        {
            int newNeighbourMovementCostToStart = current.G_Cost + current.Distance(to: neighbour) + neighbour.MovementPenalty;

            if (newNeighbourMovementCostToStart < neighbour.G_Cost || !_openNodes.Contains(neighbour))
            {
                neighbour.Open(
                    parent: current,
                    g_cost: newNeighbourMovementCostToStart,
                    h_cost: neighbour.Distance(to: target));
                
                if (!_openNodes.Contains(neighbour))
                    _openNodes.Add(neighbour);
                else
                {
                    _openNodes.UpdateItem(neighbour);
                }
            }
        }

        private Vector3[] RetracePath(Node startNode, Node endNode)
        {
            List<Node> path = new List<Node>();

            Node current = endNode;

            while (current != startNode)
            {
                path.Add(current);
                current = current.Parent;
            }

            Vector3[] waypoints = SimplifyPath(path);
            Array.Reverse(waypoints);

            _closedNodes.Clear();
            _openNodes.Clear();
            return waypoints;
        }

        private static Vector3[] SimplifyPath(List<Node> path)
        {
            List<Vector3> waypoints = new List<Vector3>();
            Vector2 directionOld = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                Vector3 nextNodePosition = path[i - 1].WorldPosition;
                Vector3 currentNodePosition = path[i].WorldPosition;
                Vector2 directionNew = currentNodePosition.Direction(to: nextNodePosition);

                if (directionOld != directionNew) 
                    waypoints.Add(nextNodePosition);

                directionOld = directionNew;
            }
            
            return waypoints.ToArray();
        }
        
    }
}