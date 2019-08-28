using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private Transform target;
    private List<Node> path;

    private void Awake()
    {
        if(target == null)
        {
            Debug.Log("У питомца отсутствует цель");
        }
    }

    void Update()
    {
        //Debug.Log(NodeList.NodeFromWorldPosition(transform.position).position);
        //Debug.Log(NodeList.nodeList.Count);

        //Debug.Log(NodeList.NearestNode(transform.position).position);
        
        if (target != null)
        {
            path = Pathfinding.FindPath(transform.position, target.position);
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.DrawCube(path[i].position, Vector3.one * 0.2f);
            }
        }
    }
}
