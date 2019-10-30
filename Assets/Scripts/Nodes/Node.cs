using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class Node : IHeapItem<Node>
    {
        public const int MAX_NEIGHBORS = 8;

        public Vector3 position { get; private set; }
        public List<Node> neighborsList = new List<Node>();

        public Node parent;

        public float gCost;
        public float hCost;

        private Color color;
        private int heapIndex;

        public float fCost { get => gCost + hCost; }
        public bool IsClean { get; private set; }
        public int HeapIndex
        {
            get => heapIndex;
            set { heapIndex = value; }
        }

        public Node(Vector3 _position)
        {
            position = _position;
            IsClean = false;
        }

        public void ConnectNode(Node neighborNode)
        {
            if (!neighborsList.Exists(x => x.position == neighborNode.position))
            {
                neighborsList.Add(neighborNode);
            }

            if (neighborsList.Count == MAX_NEIGHBORS)
            {
                IsClean = true;
            }
        }

        public void DisconnectNode()
        {
            for (int i = 0; i < neighborsList.Count; i++)
            {
                if (neighborsList[i].neighborsList.Exists(x => x.position == position))
                {
                    neighborsList[i].neighborsList.Remove(this);
                    IsClean = false;
                }
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = fCost.CompareTo(nodeToCompare.fCost);
            if (compare == 0)
            {
                compare = hCost.CompareTo(nodeToCompare.hCost);
            }
            return -compare;
        }
    }
}
