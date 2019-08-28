using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = NodeList.NearestNode(startPos);
        Node targetNode = NodeList.NearestNode(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if(openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            List<Node> neighbors = currentNode.neighborsList;

            for (int i = 0; i < neighbors.Count; i++)
            {
                if (closedSet.Contains(neighbors[i]))
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbors[i]);
                if(newMovementCostToNeighbor < neighbors[i].gCost || !openSet.Contains(neighbors[i]))
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

    private static List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;


    }

    private static float GetDistance(Node nodeA, Node nodeB)
    {
        return Vector3.Distance(nodeA.position, nodeB.position);
    }
}
