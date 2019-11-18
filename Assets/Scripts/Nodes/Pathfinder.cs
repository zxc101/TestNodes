using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class Pathfinder
    {
        public static Stack<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            Node startNode = NodeList.NearestNode(startPos);
            Node targetNode = NodeList.NearestNode(targetPos);

            Heap<Node> openSet = new Heap<Node>(NodeList.nodeList.Count);
            HashSet<Node> closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    Stack<Vector3> path = RetracePath(startNode, targetNode, targetPos) ?? new Stack<Vector3>();
                    if (path.Count == 0)
                    {
                        path.Push(startPos);
                    }
                    return path;
                }

                List<Node> neighbors = currentNode.neighborsList;

                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (closedSet.Contains(neighbors[i]))
                    {
                        continue;
                    }

                    float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbors[i]);
                    if (newMovementCostToNeighbor < neighbors[i].gCost || !openSet.Contains(neighbors[i]))
                    {
                        neighbors[i].gCost = newMovementCostToNeighbor;
                        neighbors[i].hCost = GetDistance(neighbors[i], targetNode);
                        neighbors[i].parent = currentNode;

                        if (!openSet.Contains(neighbors[i]))
                        {
                            openSet.Add(neighbors[i]);
                        }
                    }
                }
            }

            return null;
        }

        private static Stack<Vector3> RetracePath(Node startNode, Node endNode, Vector3 targetPos)
        {
            Stack<Vector3> path = new Stack<Vector3>();
            Node currentNode = endNode;

            //if(startNode != endNode)
            //{
            //    path.Push(targetPos);
            //    if (currentNode.parent != null)
            //    {
            //        currentNode = currentNode.parent;
            //    }
            //}

            while (currentNode != startNode)
            {
                path.Push(currentNode.position);
                currentNode = currentNode.parent;
            }

            return path;
        }

        private static float GetDistance(Node nodeA, Node nodeB)
        {
            return Vector3.Distance(nodeA.position, nodeB.position);
        }
    }
}
