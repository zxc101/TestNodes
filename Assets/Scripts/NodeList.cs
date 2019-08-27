using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeList
{
    private static List<Node> rawNodeList = new List<Node>();
    private static List<Node> nodeList = new List<Node>();

    /// <summary>
    /// Добавляет Node
    /// </summary>
    /// <param name="position">Позиция добавления Node</param>
    public static void Update(Vector3 position)
    {
        Node node = rawNodeList.Find(x => x.position == position);

        if (node != null)
        {
            if (node.neighborsList.Count < Node.MAX_NEIGHBORS)
            {
                node.DisconnectNode();
                rawNodeList.Remove(node);
                Update(position);
                return;
            }
        }
        else
        {
            node = nodeList.Find(x => x.position == position);
            if (node != null && node.neighborsList.Count < Node.MAX_NEIGHBORS)
            {
                node.DisconnectNode();
                nodeList.Remove(node);
                Update(position);
                return;
            }
        }

        // Если нету в nodeList то ...
        if (!nodeList.Exists(x => x.position == position))
        {
            // Если нету в rawNodeList то ...
            if (!rawNodeList.Exists(x => x.position == position))
            {
                // Добавляет в rawNodeList
                rawNodeList.Add(new Node(position));
                // Находит соседей
                FindNeighbors(position, Vector3.right);
                FindNeighbors(position, Vector3.left);
                FindNeighbors(position, Vector3.forward);
                FindNeighbors(position, Vector3.back);
            }
        }

        if (node != null && node.neighborsList.Count == Node.MAX_NEIGHBORS)
        {
            rawNodeList.Remove(node);
            nodeList.Add(node);
        }
    }

    public static int Count()
    {
        return rawNodeList.Count + nodeList.Count;
    }

    private static void FindNeighbors(Vector3 position, Vector3 direction)
    {
        RaycastHit hit;

        if (!Physics.Raycast(position, direction, out hit, NodeSetting.wolkDistance, NodeSetting.layerMask))
        {
            Node startNode = rawNodeList.Find(x => x.position == position);
            if(startNode == null)
            {
                startNode = nodeList.Find(x => x.position == position);
            }

            Vector3 sidePosition = position + direction * NodeSetting.wolkDistance;
            
            Node neighborNode = rawNodeList.Find(x => x.position == sidePosition);

            if (neighborNode == null)
            {
                neighborNode = nodeList.Find(x => x.position == sidePosition);
            }

            if (neighborNode != null)
            {
                // Записываем в соседей
                startNode.ConnectNode(neighborNode);
                neighborNode.ConnectNode(startNode);
            }
            else
            {
                if (Physics.Raycast(sidePosition, Vector3.down, out hit, NodeSetting.jumpDistance, NodeSetting.layerMask))
                {
                    sidePosition.y = hit.transform.position.y;
                    neighborNode = rawNodeList.Find(x => x.position == sidePosition);

                    if (neighborNode == null)
                    {
                        neighborNode = nodeList.Find(x => x.position == sidePosition);
                    }

                    if (neighborNode != null)
                    {
                        // Записываем в соседей
                        startNode.ConnectNode(neighborNode);
                        neighborNode.ConnectNode(startNode);
                    }
                }
            }
        }
    }


}
