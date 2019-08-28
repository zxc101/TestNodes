using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public const int MAX_NEIGHBORS = 4;

    public Vector3 position { get; private set; }
    public List<Node> neighborsList = new List<Node>();

    public Node parent;

    public float gCost;
    public float hCost;

    public float fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    private Transform box;
    private Color color;

    public bool IsClean {get; private set;}

    public Node(Vector3 _position)
    {
        position = _position;
        // Создаём на этом месте куб
        box = Object.Instantiate(NodeSetting.nodeBox, _position, Quaternion.identity, NodeSetting.boxesBase);
        IsClean = false;
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
            box.GetComponent<NodeView>().Add(neighborNode);
        }

        if (neighborsList.Count == MAX_NEIGHBORS)
        {
            IsClean = true;
            SetColor(NodeSetting.nodeColor);
        }
    }

    public void DisconnectNode()
    {
        // Удалить box
        if(box.GetComponent<NodeView>() != null)
        {
            box.GetComponent<NodeView>().Destroy();
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
