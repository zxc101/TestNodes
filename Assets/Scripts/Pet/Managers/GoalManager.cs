using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nodes;

namespace Pets
{
    public class GoalManager
    {
        private Pet pet;

        public GoalManager(Pet _pet)
        {
            pet = _pet;
        }

        public void ChangeGoal()
        {
            //Debug.Log(FindMinValInNeed().name);
            if (FindMinValInNeed() == null)
            {
                return;
            }
            else
            {
                if (FindMinValInNeed().value < FindMinValInNeed().criticalValue)
                {
                    if (pet.Need != FindMinValInNeed() && !pet.AnimManager.IsNeed(pet.Need.name))
                    {
                        Debug.Log("2");
                        pet.Need = FindMinValInNeed();
                        ChangeMainGoal(pet.Need.prefab);
                    }
                }
            }
        }

        private Need FindMinValInNeed()
        {
            Need res = null;
            List<Need> activeNeeds = new List<Need>();
            for(int i = 0; i < pet.Needs.Length; i++)
            {
                if (pet.Needs[i].prefab.gameObject.activeSelf)
                {
                    activeNeeds.Add(pet.Needs[i]);
                }
            }
            for(int i = 0; i < activeNeeds.Count; i++)
            {
                if(i == 0)
                {
                    res = activeNeeds[0];
                }
                else
                {
                    if(activeNeeds[i].value < res.value)
                    {
                        res = activeNeeds[i];
                    }
                }
            }
            return res;
        }

        public void ClearAllGoals()
        {
            if (pet.Goals != null)
            {
                RemoveGoals(pet.Goals.Count);
            }
        }

        public void RemoveAllHalperGoals()
        {
            RemoveGoals(pet.Goals.Count - 1);
        }

        public void ChangeMainGoal(Transform newPoint)
        {
            if (pet.Goals != null && !pet.Goals.IsEmpty)
                pet.RemoveLastGoal();
            pet.AddLastGoal(newPoint);
        }

        public void RemoveMainGoal()
        {
            if (pet.Goals != null && !pet.Goals.IsEmpty)
            {
                if (pet.Goals.Last.position == pet.Need.prefab.position)
                {
                    pet.RemoveLastGoal();
                }
            }
        }

        private Vector3 SelectNewGoal(List<Node> nodeList)
        {
            return nodeList[Random.Range(0, nodeList.Count)].position;
        }

        private void RemoveGoals(int count)
        {
            for (int i = 0; i < count; i++)
            {
                pet.RemoveFirstGoal();
            }
        }
    }
}
