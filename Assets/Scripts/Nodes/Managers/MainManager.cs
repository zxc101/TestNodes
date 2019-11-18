using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private NodeMatrix matrix;
        [SerializeField] private bool isDrawNodes;

        private void Start()
        {
            matrix.Transform.position = matrix.Pet.position + Vector3.up * 3;
            matrix.CreateManager.Create();
            StartCoroutine(MainDoes());
        }

        private IEnumerator MainDoes()
        {
            while (true)
            {
                if (matrix.isActiveAndEnabled)
                {
                    matrix.MoveManager.Move();
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private void OnDrawGizmos()
        {
            if (isDrawNodes)
            {
                if (NodeList.nodeList != null)
                {
                    Gizmos.color = Color.green;
                    for (int i = 0; i < NodeList.nodeList.Count; i++)
                    {
                        Gizmos.DrawCube(NodeList.nodeList[i].position, Vector3.one * 0.05f);
                    }
                }

                if (NodeList.rawNodeList != null)
                {
                    Gizmos.color = Color.yellow;
                    for (int i = 0; i < NodeList.rawNodeList.Count; i++)
                    {
                        Gizmos.DrawCube(NodeList.rawNodeList[i].position, Vector3.one * 0.05f);
                    }
                }
            }
        }
    }
}
