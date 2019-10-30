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
            pet.SpeedRotate = 0;

            if (pet.CurrentGoal == null)
            {
                Debug.Log("У питомца отсутствует цель");
                return;
            }

            StartCoroutine(CFixedUpdate());
        }

        private IEnumerator CFixedUpdate()
        {
            while (true)
            {
                if (pet.CurrentGoal != null)
                {
                    if (pet.Path.Peek() == pet.Transform.position)
                    {
                        pet.GoalManager.GetOtherGoal();
                    }
                    yield return StartCoroutine(pet.MoveManager.Start());
                    //if (pet.Path != null && pet.Path.Count > 1)
                    //{
                    //    yield return StartCoroutine(pet.MoveManager.Start(pet.Goal));
                    //}
                    //else
                    //{
                    //    if (pet.Path.Count == 1)
                    //    {
                    //        pet.MoveManager.Start(pet.CurrentGoal.position);
                    //    }
                    //    pet.GoalManager.GetOtherGoal();
                    //    yield return new WaitForFixedUpdate();
                    //    //yield return StartCoroutine(Sit());
                    //}
                }
                //else
                //{
                //        yield return StartCoroutine(Sit());
                //}
            }
        }
    }
}
