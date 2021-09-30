using CodeBase.DataStructures;
using UnityEngine;

namespace CodeBase.Grid
{
    public class Node : IHeapItem<Node>, IBlurItem
    {
        public const float Radius = .5f;
        public const float Diameter = Radius * 2;


        private int h_Cost { get; set; }

        private int f_Cost => h_Cost + G_Cost;

        public Node Parent { get; private set; }

        public bool Walkable { get; }

        public Vector3 WorldPosition { get; }

        public Vector2Int PositionOnBoard { get; }

        int IBlurItem.BlurValue
        {
            get => MovementPenalty;
            set => MovementPenalty = value;
        }


        int IHeapItem<Node>.HeapIndex { get; set; }
        public int MovementPenalty { get; set; }

        public int G_Cost { get; private set; }


        public Node(bool walkable, Vector3 position, Vector2Int positionOnBoard, int movementPenalty)
        {
            Walkable = walkable;
            WorldPosition = position;
            PositionOnBoard = positionOnBoard;
            MovementPenalty = movementPenalty;
        }

        public void Open(Node parent, int g_cost, int h_cost)
        {
            Parent = parent;
            G_Cost = g_cost;
            h_Cost = h_cost;
        }

        public int CompareTo(Node other)
        {
            int compareValue = f_Cost.CompareTo(other.f_Cost);

            if (compareValue == 0)
                return h_Cost.CompareTo(other.h_Cost);

            return -compareValue;
        }
    }

    public interface IBlurItem
    {
        int BlurValue { get; set; }
    }
}