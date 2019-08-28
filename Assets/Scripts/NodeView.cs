using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public List<Vector3> neighborsList = new List<Vector3>();

    public void Add(Node node)
    {
        if (!neighborsList.Exists(x => x == node.position))
        {
            neighborsList.Add(node.position);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
