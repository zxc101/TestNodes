using Nodes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private Pet pet;

        private void Start()
        {
            StartCoroutine(CFixedUpdate());
        }

        private IEnumerator CFixedUpdate()
        {
            while (true)
            {
                if (pet.isActiveAndEnabled)
                {
                    pet.GoalManager.ChangeGoal();
                    if (!pet.Goals.IsEmpty)
                    {
                        pet.Path = Pathfinder.FindPath(pet.Transform.position, pet.Goals.First.position);
                        if (pet.Path == null || pet.Path.Count == 0)
                        {
                            pet.MoveManager.Stop();
                            yield return new WaitForFixedUpdate();
                        }
                        else
                        {
                            yield return StartCoroutine(pet.MoveManager.Start());
                        }
                    }
                    else
                    {
                        pet.MoveManager.Stop();
                        yield return new WaitForFixedUpdate();
                    }
                }
                else
                {
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (pet.Path != null)
            {
                Gizmos.color = Color.red;
                Vector3[] gizmosPath = pet.Path.ToArray();
                for (int i = 0; i < gizmosPath.Length; i++)
                {
                    Gizmos.DrawCube(gizmosPath[i], Vector3.one * 0.05f);
                }
            }
        }
    }
}
