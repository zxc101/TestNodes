using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeList
{
    private static List<Node> rawNodeList = new List<Node>();
    public static List<Node> nodeList = new List<Node>();

    public static int Count
    {
        get => rawNodeList.Count + nodeList.Count;
    }

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
                node = new Node(position);
                // Находит соседей
                FindNeighbors(node, Vector3.right);
                FindNeighbors(node, Vector3.left);
                FindNeighbors(node, Vector3.forward);
                FindNeighbors(node, Vector3.back);
                rawNodeList.Add(node);
            }
        }
    }

    private static void FindNeighbors(Node node, Vector3 direction)
    {
        RaycastHit hit;

        if (!Physics.Raycast(node.position, direction, out hit, NodeSetting.wolkDistance, NodeSetting.layerMask))
        {
            Vector3 sidePosition = node.position + direction * NodeSetting.wolkDistance;
            
            Node neighborNode = rawNodeList.Find(x => x.position == sidePosition);

            if (neighborNode == null)
            {
                neighborNode = nodeList.Find(x => x.position == sidePosition);
            }

            if (neighborNode != null)
            {
                // Записываем в соседей
                node.ConnectNode(neighborNode);
                if (node.IsClean && !nodeList.Exists(n => n == node))
                {
                    rawNodeList.Remove(node);
                    nodeList.Add(node);
                }
                neighborNode.ConnectNode(node);
                if (neighborNode.IsClean && !nodeList.Exists(n => n == neighborNode))
                {
                    rawNodeList.Remove(neighborNode);
                    nodeList.Add(neighborNode);
                }
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
                        node.ConnectNode(neighborNode);
                        if (node.IsClean && !nodeList.Exists(n => n == node))
                        {
                            rawNodeList.Remove(node);
                            nodeList.Add(node);
                        }
                        neighborNode.ConnectNode(node);
                        if (neighborNode.IsClean && !nodeList.Exists(n => n == neighborNode))
                        {
                            rawNodeList.Remove(neighborNode);
                            nodeList.Add(neighborNode);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Находит ближайший Node или создаёт Node соответствующий заданной позиции
    /// </summary>
    /// <param name="position">Проверяемая позиция</param>
    /// <returns>Node соответствующий заданной позиции</returns>
    public static Node NearestNode(Vector3 position)
    {
        Node nearestNode = nodeList[0];
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (Vector3.Distance(nodeList[i].position, position) < Vector3.Distance(nearestNode.position, position))
            {
                nearestNode = nodeList[i];
            }
        }
        return nearestNode;
    }

    public static List<Node> GetNeighbors(Vector3 position)
    {
        return NearestNode(position).neighborsList;
    }
}
