using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class NodeList
    {
        public static List<Node> rawNodeList = new List<Node>();
        public static List<Node> nodeList = new List<Node>();
        
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

            // Если нету в nodeList и в rawNodeList то ...
            if (!nodeList.Exists(x => x.position == position) &&
                !rawNodeList.Exists(x => x.position == position))
            {
                BuildNode(position);
            }
        }

        private static void BuildNode(Vector3 position)
        {
            Node node = new Node(position);
            // Находит соседей
            FindNeighbors(node, Vector3.forward);
            FindNeighbors(node, Vector3.forward + Vector3.right);
            FindNeighbors(node, Vector3.right);
            FindNeighbors(node, Vector3.right + Vector3.back);
            FindNeighbors(node, Vector3.back);
            FindNeighbors(node, Vector3.back + Vector3.left);
            FindNeighbors(node, Vector3.left);
            FindNeighbors(node, Vector3.left + Vector3.forward);

            AddNodeToList(ref rawNodeList, node);
        }

        private static void FindNeighbors(Node node, Vector3 direction)
        {
            RaycastHit hit;

            Vector3 sidePosition = node.position + direction * NodeSetting.wolkDistance;

            // Если не сталкивается со стенкой
            if (!Physics.Raycast(node.position, direction, out hit, NodeSetting.wolkDistance, NodeSetting.layerMask))
            {
                // Проверяем на присутствие Node с не сырых нодах с определённой стороны
                Node neighborNode = rawNodeList.Find(x => x.position == sidePosition);

                // Если соседа нет ищем в готовых нодах
                if (neighborNode == null)
                {
                    neighborNode = nodeList.Find(x => x.position == sidePosition);
                }

                // Если всё же найден
                if (neighborNode != null)
                {
                    // Записываем в соседей
                    ConnectSideNodes(node, neighborNode);
                }
                // Если сбоку не найден, то ищем сбоку - ниже и проделываем тоже самое
                else
                {
                    ConnectSideDownNodes(sidePosition, node, neighborNode);
                }
            }
            else
            {
                // Удаляем ноды из готовых если рядом появляется стенка
                if (nodeList.Exists(x => x.position == node.position))
                {
                    nodeList.Remove(node);
                    AddNodeToList(ref rawNodeList, node);
                }

                // Если скозь стенку есть нод стоящий в nodeList
                Node neighborNode = nodeList.Find(x => x.position == sidePosition);

                // Удаляем и его
                if (neighborNode != null)
                {
                    nodeList.Remove(neighborNode);
                    AddNodeToList(ref rawNodeList, neighborNode);
                }
            }
        }

        private static void ConnectSideDownNodes(Vector3 sidePosition, Node node1, Node node2)
        {
            RaycastHit hit;

            if (Physics.Raycast(sidePosition, Vector3.down, out hit, NodeSetting.jumpDistance, NodeSetting.layerMask))
            {
                sidePosition.y = hit.transform.position.y;
                node2 = rawNodeList.Find(x => x.position == sidePosition);

                if (node2 == null)
                {
                    node2 = nodeList.Find(x => x.position == sidePosition);
                }

                if (node2 != null)
                {
                    // Записываем в соседей
                    ConnectSideNodes(node1, node2);
                }
            }
        }

        private static void ConnectSideNodes(Node node1, Node node2)
        {
            // Записываем в соседей neighborNode для node
            node1.ConnectNode(node2);

            // Если у node присутствуют все стороны, но всё ещё отсутствует в nodeList
            if (node1.IsClean && !nodeList.Exists(n => n == node1))
            {
                // Переносим его из rawNodeList в nodeList
                rawNodeList.Remove(node1);
                AddNodeToList(ref nodeList, node1);
            }

            // Записываем в соседей node для neighborNode
            node2.ConnectNode(node1);

            // Если у neighborNode присутствуют все стороны, но всё ещё отсутствует в nodeList
            if (node2.IsClean && !nodeList.Exists(n => n == node2))
            {
                // Переносим его из rawNodeList в nodeList
                rawNodeList.Remove(node2);
                AddNodeToList(ref nodeList, node2);
            }
        }

        private static void AddNodeToList(ref List<Node> list, Node node)
        {
            if (list.Count > 1000)
            {
                list.Remove(list.Find(x => Vector3.Distance(x.position, node.position) > 1));
            }

            list.Add(node);
        }
    }
}
