using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inputs
{
    public class MainManager : MonoBehaviour
    {
        [SerializeField] private Pets.Pet pet;
        [SerializeField] private Transform goal;
        [SerializeField] private Transform goalsBase;

        void Start()
        {
            StartCoroutine(Inputs());
        }

        private IEnumerator Inputs()
        {
            while (true)
            {
                Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                if (Input.GetMouseButtonDown(1))
                {
                    AddNewGoal(mousePos);
                }
                yield return new WaitForFixedUpdate();
            }
        }

        private void AddNewGoal(Vector2 mousePos)
        {
            Vector3 worldPos;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                worldPos = hit.point;
            }
            else
            {
                worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            }
            pet.Goals.AddFirst(Instantiate(goal, worldPos, Quaternion.identity, goalsBase));
        }
    }
}
