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
    private float speedMove;
    private float speedRoteta;
    private float timeRotate = 4;

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
        speedRoteta = 0;

        if (currentGoal == null)
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
                    yield return StartCoroutine(Sit(Goal));
                }
            }
            //else
            //{
            //        yield return StartCoroutine(Sit(Goal));
            //}
        }
    }

    private IEnumerator Sit(Vector3 target)
    {
        Debug.Log("Sit");
        yield return new WaitForSeconds(1);
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        float hight = Goal.y;
        if (transform.position.y < hight + 0.1f && transform.position.y < hight - 0.1f)
        {
            if(Mathf.Abs(RotateToTarget(Goal)) > 12)
            {
                speedRoteta = Mathf.Lerp(speedRoteta, RotateToTarget(Goal), Time.fixedDeltaTime * timeRotate);
                anim.SetFloat("Rotate", speedRoteta);
                anim.SetFloat("Speed", 0);
            }
            else
            {
                anim.SetBool("IsJumpUp", true);
            }
        }
        else if (transform.position.y > hight + 0.1f && transform.position.y > hight - 0.1f)
        {
            if (Mathf.Abs(RotateToTarget(Goal)) > 12)
            {
                speedRoteta = Mathf.Lerp(speedRoteta, RotateToTarget(Goal), Time.fixedDeltaTime * timeRotate);
                anim.SetFloat("Rotate", speedRoteta);
                anim.SetFloat("Speed", 0);
            }
            else
            {
                anim.SetBool("IsJumpDown", true);
            }
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
                float dist = Vector3.Distance(transform.position, hit.point);
                speedRoteta = Mathf.Lerp(speedRoteta, RotateToTarget(target), Time.fixedDeltaTime * timeRotate);
                if (dist < 0.2f)
                {
                    anim.SetFloat("Speed", 0);
                    anim.SetFloat("Rotate", speedRoteta);
                }
                else if (dist > 0.2f && dist < 1)
                {
                    Mathf.Clamp(speedMove, 0, 2f);
                    anim.SetFloat("Speed", speedMove);
                    anim.SetFloat("Rotate", speedRoteta);
                }
                else
                {
                    speedMove = 4;
                    anim.SetFloat("Speed", speedMove);
                }
            }
            else
            {
                if ((Mathf.Abs(transform.forward.x) == 0.7f && Mathf.Abs(transform.forward.z) == 0.7f) ||
                    (Mathf.Abs(transform.forward.x) == 1 && Mathf.Abs(transform.forward.z) == 1))
                {
                    speedRoteta = 0;
                }
                else
                {
                    speedRoteta = Mathf.Lerp(speedRoteta, RotateToTarget(target), Time.fixedDeltaTime * timeRotate);
                }
                anim.SetFloat("Rotate", speedRoteta);
                anim.SetFloat("Speed", Speed);
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

    private float Speed
    {
        get
        {
            float speed = Vector3.Distance(transform.position, currentGoal.position);
            speed = Mathf.Clamp(speed, 1, 2);
            return speed;
        }
    }

    private void GetOtherTarget()
    {
        speedRoteta = 0;
        anim.SetFloat("Rotate", speedRoteta);
        anim.SetFloat("Speed", 0);

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
