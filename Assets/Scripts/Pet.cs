using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private bool exploreTerritory;

    private Animator anim;
    private List<Vector3> path;

    private void Awake()
    {
        if(target == null)
        {
            Debug.Log("У питомца отсутствует цель");
            return;
        }

        anim = GetComponent<Animator>();
    }

    void Update()
    {        
        if (target != null)
        {
            path = Pathfinding.FindPath(transform.position, target.position);
            if (path.Count > 1)
            {
                //if(path[0].y > transform.position.y)
                //{

                //}
                //else if(path[0].y < transform.position.y)
                //{

                //}
                //else
                //{
                    MoveToTarget(path[0]);
                //}
            }
            else
            {
                GetOtherTarget();
            }
        }
    }

    private void JumpUp()
    {

    }

    private void JumpDown()
    {

    }

    private void MoveToTarget(Vector3 target)
    {
        anim.SetFloat("Rotate", RotateToTarget(target));
        anim.SetFloat("Distance", DistanceToTarget(target) > 0 ? Random.Range(1, 3) : 0);
    }

    private float RotateToTarget(Vector3 target)
    {
        Vector3 targetPos = target;
        targetPos.y = transform.position.y;

        Vector3 targetDir = targetPos - transform.position;

        float angleRotate = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

        angleRotate = angleRotate * -1;

        if (Mathf.Abs(angleRotate) < 2f)
        {
            angleRotate = 0;
        }

        return angleRotate;
    }

    private float DistanceToTarget(Vector3 target)
    {
        float distance = Vector3.Distance(transform.position, target);

        if (distance < 1 && distance > 0.2f)
        {
            distance = 0.5f;
        }
        if (distance < 0.2f)
        {
            distance = 0;
        }
        return distance;
    }

    private void GetOtherTarget()
    {
        anim.SetFloat("Rotate", 0);
        anim.SetFloat("Distance", 0);

        if (exploreTerritory)
        {
            target.position = SelectNewTarget(NodeList.rawNodeList);
        }
        else
        {
            target.position = SelectNewTarget(NodeList.nodeList);
        }
    }

    private Vector3 SelectNewTarget(List<Node> nodeList)
    {
        return nodeList[Random.Range(0, nodeList.Count)].position;
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < path.Count; i++)
            {
                Gizmos.DrawCube(path[i], Vector3.one * 0.2f);
            }
        }
    }
}
