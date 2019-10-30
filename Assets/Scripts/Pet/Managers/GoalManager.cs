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

        public void GetOtherGoal()
        {
            pet.SpeedMove = 0;
            pet.SpeedRotate = 0;

            SetGoal();
        }

        private void SetGoal()
        {
            Debug.Log($"ExploreTerritory == {pet.ExploreTerritory}");
            if (pet.ExploreTerritory)
            {
                pet.CurrentGoal.position = SelectNewGoal(NodeList.rawNodeList);
            }
            else
            {
                pet.CurrentGoal.position = SelectNewGoal(NodeList.nodeList);
            }
        }

        private Vector3 SelectNewGoal(List<Node> nodeList)
        {
            return nodeList[Random.Range(0, nodeList.Count)].position;
        }
    }
}
