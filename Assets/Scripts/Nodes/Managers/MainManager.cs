using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private NodeMatrix matrix;

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
                matrix.MoveManager.Move();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
