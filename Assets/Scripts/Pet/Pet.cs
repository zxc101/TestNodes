using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private Transform currentGoal;
    [SerializeField] private Transform eye;
    [SerializeField] private bool exploreTerritory;

    private Animator anim;
    private Stack<Vector3> path;
    private float speed;

    private Vector3 Goal
    {
        get
        {
            path = Pathfinding.FindPath(transform.position, currentGoal.position);
            if (path.Count == 0)
            {
                GetRundomGoal();
                return currentGoal.position;
            }
            if (Vector3.Distance(transform.position, path.Peek()) < 0.2f)
            {
                return path.Pop();
            }
            else
            {
                return path.Peek();
            }
        }
    }

    private void Start()
    {
        if(currentGoal == null)
        {
            Debug.Log("У питомца отсутствует цель");
            return;
        }

        anim = GetComponent<Animator>();
        StartCoroutine(CFixedUpdate());
    }

    private IEnumerator CFixedUpdate()
    {
        while (true)
        {
            Debug.Log(NodeList.nodeList.Count);
            if (currentGoal != null)
            {
                path = Pathfinding.FindPath(transform.position, currentGoal.position);
                if (path.Count > 1)
                {
                    yield return StartCoroutine(MoveToTarget(Goal));
                }
                else
                {
                    if(path.Count == 1)
                    {
                        MoveToTarget(currentGoal.position);
                    }
                    GetOtherTarget();
                    //yield return StartCoroutine(Sit(Goal));
                }
            }
            //else
            //{
            //        yield return StartCoroutine(Sit(Goal));
            //}
        }
    }

    //private IEnumerator Sit(Vector3 target)
    //{
    //    Debug.Log("Sit");
    //    yield return new WaitForSeconds(1);
    //}

    private IEnumerator MoveToTarget(Vector3 target)
    {
        float hight = Goal.y;
        if (transform.position.y < hight + 0.1f && transform.position.y < hight - 0.1f)
        {
            if(RotateToTarget(target) > 0)
            {
                anim.SetFloat("Rotate", RotateToTarget(target));
                anim.SetFloat("Distance", 0);
            }
            anim.SetBool("IsJumpUp", true);
        }
        else if (transform.position.y > hight + 0.1f && transform.position.y > hight - 0.1f)
        {
            if (RotateToTarget(target) > 0)
            {
                anim.SetFloat("Rotate", RotateToTarget(target));
                anim.SetFloat("Distance", 0);
            }
            anim.SetBool("IsJumpDown", true);
        }
        else
        {
            if (transform.position.y != hight)
            {
                anim.SetBool("IsJumpUp", false);
                anim.SetBool("IsJumpDown", false);
                Helper.helpVector.x = transform.position.x;
                Helper.helpVector.y = hight;
                Helper.helpVector.z = transform.position.z;
                transform.position = Helper.helpVector;
            }

            RaycastHit hit;

            if (Physics.Raycast(eye.position , transform.forward, out hit, 1, NodeSetting.layerMask))
            {
                Debug.Log(hit.point);
                float dist = Vector3.Distance(transform.position, hit.point);
                if (dist < 0.2f)
                {
                    anim.SetFloat("Distance", 0);
                    anim.SetFloat("Rotate", RotateToTarget(target));
                }
                else if (dist > 0.2f && dist < 1)
                {
                    Mathf.Clamp(speed, 0, 0.5f);
                    anim.SetFloat("Distance", speed);
                    anim.SetFloat("Rotate", RotateToTarget(target));
                }
                else
                {
                    Mathf.Clamp(speed, 0, 1);
                    anim.SetFloat("Distance", speed);
                }
            }
            else
            {
                anim.SetFloat("Rotate", RotateToTarget(target));
                DistanceToTarget(currentGoal.position);
                anim.SetFloat("Distance", speed);
            }
        }
        yield return new WaitForFixedUpdate();
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

    private void DistanceToTarget(Vector3 target)
    {
        float distance = Vector3.Distance(transform.position, target);

        speed = distance;

        if (distance < 1 && distance > 0.2f)
        {
            speed = 0.5f;
        }
        if (distance < 0.2f)
        {
            speed = 0;
        }
    }

    private void GetOtherTarget()
    {
        anim.SetFloat("Rotate", 0);
        anim.SetFloat("Distance", 0);

        if (exploreTerritory)
        {
            currentGoal.position = SelectNewTarget(NodeList.rawNodeList);
        }
        else
        {
            currentGoal.position = SelectNewTarget(NodeList.nodeList);
        }
    }

    private Vector3 SelectNewTarget(List<Node> nodeList)
    {
        return nodeList[Random.Range(0, nodeList.Count)].position;
    }

    private void GetRundomGoal()
    {
        if (PetSetting.goals.Count != 0)
        {
            int i = Random.Range(0, PetSetting.goals.Count);
            while (currentGoal == PetSetting.goals[i].transform)
            {
                i = Random.Range(0, PetSetting.goals.Count);
            }
            currentGoal = PetSetting.goals[i].transform;
        }
    }

    private void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.red;
            Vector3[] gizmosPath = path.ToArray();
            for (int i = 0; i < gizmosPath.Length; i++)
            {
                Gizmos.DrawCube(gizmosPath[i], Vector3.one * 0.2f);
            }
        }
    }
}
