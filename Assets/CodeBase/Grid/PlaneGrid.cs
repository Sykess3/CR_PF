using System;
using System.Collections.Generic;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Grid
{
    public class PlaneGrid : MonoBehaviour
    {
        private int _obstaclePenalty;

        //TODO: Node state review
        private Vector2Int _gridSizeInWorldSpace = new Vector2Int(40, 40);
        private int _unwalkableMask;
        private Node[,] _grid;
        private LayerMask _walkableMask;

        private Vector2Int _nodesCount { get; set; }
        private readonly Dictionary<int, int> _walkableRegionsKVP = new Dictionary<int, int>();
        private TerrainType[] _walkableRegions;

        public int Size => _nodesCount.x * _nodesCount.y;

#if UNITY_EDITOR
        private int _minPenalty;
        private int _maxPenalty;
#endif

        public void Awake()
        {
            _unwalkableMask = (1 << LayerMask.NameToLayer("Unwalkable"));
            DefineWalkableRegions();
            DefineWalkableMask();
        }

        private void Start()
        {
            Physics.SyncTransforms();
            Create();
        }


        public Node NodeFromWorldPosition(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + _gridSizeInWorldSpace.x * 0.5f) / _gridSizeInWorldSpace.x;
            float percentY = (worldPosition.z + _gridSizeInWorldSpace.y * 0.5f) / _gridSizeInWorldSpace.y;

            if (percentX > 1 || percentY > 1)
                throw new ArgumentException("Invalid node position(not within grid position)");

            int xIndex = Mathf.RoundToInt((_nodesCount.x - 1) * percentX);
            int yIndex = Mathf.RoundToInt((_nodesCount.y - 1) * percentY);

            return _grid[xIndex, yIndex];
        }

        public List<Node> NeighboursFor(Node node)
        {
            List<Node> neighbours = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (ArgumentNode(x, y))
                        continue;
                    int neighbourXBoardPosition = node.PositionOnBoard.x + x;
                    int neighbourYBoardPosition = node.PositionOnBoard.y + y;

                    if (NotBeyondTheBoard(neighbourXBoardPosition, neighbourYBoardPosition))
                        neighbours.Add(_grid[neighbourXBoardPosition, neighbourYBoardPosition]);
                }
            }

            return neighbours;

            bool ArgumentNode(int x, int y)
            {
                return x == 0 && y == 0;
            }

            bool NotBeyondTheBoard(int neighbourXBoardPosition, int neighbourYBoardPosition)
            {
                return neighbourXBoardPosition >= 0 && neighbourXBoardPosition < _nodesCount.x
                                                    && neighbourYBoardPosition >= 0 &&
                                                    neighbourYBoardPosition < _nodesCount.y;
            }
        }

        private void Create()
        {
            _nodesCount = Capacity();
            _grid = new Node[_nodesCount.x, _nodesCount.y];

            Vector3 worldBottomLeft = GridOrigin();

            InitGridArray(worldBottomLeft);
            SmoothGridPenalties();
        }

        private void InitGridArray(Vector3 worldBottomLeft)
        {
            for (int x = 0; x < _nodesCount.x; x++)
            {
                for (int y = 0; y < _nodesCount.y; y++)
                {
                    Vector3 nodeInWorldSpace = NodePosition(worldBottomLeft, x, y);

                    bool walkable = !Physics.CheckSphere(nodeInWorldSpace, Node.Radius, _unwalkableMask);
                    int regionMovementPenalty = 0;

                    regionMovementPenalty = walkable
                        ? PenaltyForNode(nodeInWorldSpace) 
                        : _obstaclePenalty;

                    _grid[x, y] = new Node(walkable, nodeInWorldSpace, positionOnBoard: new Vector2Int(x, y),
                        regionMovementPenalty);
                }
            }
        }

        private void SmoothGridPenalties()
        {
            int[,] smoothPenalties = Blur.Execute(_grid, 3);

            for (int i = 0; i < _nodesCount.x; i++)
            {
                for (int j = 0; j < _nodesCount.y; j++)
                {
                    _grid[i, j].MovementPenalty = smoothPenalties[i, j];

#if UNITY_EDITOR
                    if (smoothPenalties[i, j] > _maxPenalty)
                        _maxPenalty = smoothPenalties[i, j];
                    else if (smoothPenalties[i, j] < _minPenalty)
                        _minPenalty = smoothPenalties[i, j];
#endif
                }
            }
        }

        private static Vector3 NodePosition(Vector3 worldBottomLeft, int x, int y) =>
            worldBottomLeft + Vector3.right * (x * Node.Diameter + Node.Radius) +
            Vector3.forward * (y * Node.Diameter + Node.Radius);

        private Vector2Int Capacity() =>
            new Vector2Int(Mathf.RoundToInt((_gridSizeInWorldSpace.x / Node.Diameter)),
                Mathf.RoundToInt((_gridSizeInWorldSpace.y / Node.Diameter)));

        private Vector3 GridOrigin() =>
            transform.position - Vector3.right * _nodesCount.x * 0.5f - Vector3.forward * _nodesCount.y * 0.5f;

        private int PenaltyForNode(Vector3 nodeInWorldSpace)
        {
            Ray ray = new Ray(nodeInWorldSpace + Vector3.up * 50, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, 100, _walkableMask))
            {
                _walkableRegionsKVP.TryGetValue(hit.collider.gameObject.layer, out int penalty);
                return penalty;
            }

            return 0;
        }

        private void DefineWalkableRegions()
        {
            _walkableRegions = new TerrainType[]
            {
                new TerrainType(1 << LayerMask.NameToLayer("Grass"), 5),
                new TerrainType(1 << LayerMask.NameToLayer("Road"), 0)
            };
        }

        private void DefineWalkableMask()
        {
            foreach (TerrainType region in _walkableRegions)
            {
                _walkableMask.value |= region.Mask.value;
                _walkableRegionsKVP.Add((int) Mathf.Log(region.Mask.value, 2), region.Penalty);

                if (_obstaclePenalty < region.Penalty) 
                    _obstaclePenalty = region.Penalty;
            }
        }

        private class TerrainType
        {
            public LayerMask Mask { get; }
            public int Penalty { get; }

            public TerrainType(LayerMask mask, int penalty)
            {
                Mask = mask;
                Penalty = penalty;
            }
        }
        
    }
}