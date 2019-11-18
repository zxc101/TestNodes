using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Nodes;

namespace Pets
{
    public class GoalManager
    {
        private Pet pet;
        private Transform goalsParent;

        public GoalManager(Pet _pet)
        {
            pet = _pet;
        }

        public void ChangeGoal()
        {
            if (pet.NeedSleep > 90 && pet.NeedEat > 90)
            {
                pet.RemoveMainGoal();
                return;
            }
            if (pet.NeedSleep < 90 && pet.NeedSleep < pet.NeedEat)
            {
                pet.ChangeMainGoal(pet.Bed);
            }
            if (pet.NeedEat < 90 && pet.NeedEat < pet.NeedSleep)
            {
                pet.ChangeMainGoal(pet.Feeder);
            }
        }

        private Vector3 SelectNewGoal(List<Node> nodeList)
        {
            return nodeList[Random.Range(0, nodeList.Count)].position;
        }
    }
}
