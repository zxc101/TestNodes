using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePhone : MonoBehaviour
{
    [SerializeField] private Transform nodesMatrix;
    [SerializeField] private float distance;

    private NodeMatrix nodeCreater;

    private void Init()
    {
        nodeCreater = nodesMatrix.GetComponent<NodeMatrix>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Vector3 phoneDistance = transform.position - nodesMatrix.position;

        if (Mathf.Abs(transform.position.x  - nodesMatrix.position.x) >= distance)
        {
            nodesMatrix.position += Vector3.right * distance * (phoneDistance.x / Mathf.Abs(phoneDistance.x));
            nodeCreater.CreateNodes();
        }

        if (Mathf.Abs(transform.position.z - nodesMatrix.position.z) >= distance)
        {
            nodesMatrix.position += Vector3.forward * distance * (phoneDistance.z / Mathf.Abs(phoneDistance.z));
            nodeCreater.CreateNodes();
        }
    }
}
