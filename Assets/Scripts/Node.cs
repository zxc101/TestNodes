using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public const int MAX_NEIGHBORS = 4;

    public Vector3 position { get; private set; }
    public List<Node> neighborsList = new List<Node>();

    private Transform box;
    private Color color;

    public Node(Vector3 _position)
    {
        position = _position;
        // Создаём на этом месте куб
        box = Object.Instantiate(NodeSetting.nodeBox, _position, Quaternion.identity, NodeSetting.boxesBase);
        SetColor(NodeSetting.rawNodeColor);
    }

    private void SetColor(Color color)
    {
        box.GetComponent<Renderer>().material.color = color;
    }

    public void ConnectNode(Node neighborNode)
    {
        if(!neighborsList.Exists(x => x.position == neighborNode.position))
        {
            neighborsList.Add(neighborNode);
            box.GetComponent<Test>().Add(neighborNode);
        }

        for(int i = 0, count = 0; i < neighborsList.Count; i++)
        {
            if (neighborsList[i].position.y <= position.y)
            {
                count++;
            }
            if (count == 4)
            {
                SetColor(NodeSetting.nodeColor);
                break;
            }
        }
    }

    public void DisconnectNode()
    {
        // Удалить box
        if(box.GetComponent<Test>() != null)
        {
            box.GetComponent<Test>().Destroy();
        }
        // Оповестить всех соседей об удалении этого Node
        for (int i = 0; i < neighborsList.Count; i++)
        {
            if(neighborsList[i].neighborsList.Exists(x => x.position == position))
            {
                neighborsList[i].neighborsList.Remove(this);
            }
        }
    }
}
