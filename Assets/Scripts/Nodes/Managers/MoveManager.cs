using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Helpers;

namespace Nodes
{
    public class MoveManager
    {
        private NodeMatrix matrix;

        public MoveManager(NodeMatrix _matrix)
        {
            matrix = _matrix;
        }

        public void Move()
        {
            Vector3 phoneDistance = NodeSetting.pet.position - matrix.Transform.position;

            if (Mathf.Abs(NodeSetting.pet.position.x - matrix.Transform.position.x) >= NodeSetting.wolkDistance)
            {
                matrix.Transform.position += Vector3.right * NodeSetting.wolkDistance * (phoneDistance.x / Mathf.Abs(phoneDistance.x));
                matrix.CreateManager.Create();
            }

            if (Mathf.Abs(NodeSetting.pet.position.z - matrix.Transform.position.z) >= NodeSetting.wolkDistance)
            {
                matrix.Transform.position += Vector3.forward * NodeSetting.wolkDistance * (phoneDistance.z / Mathf.Abs(phoneDistance.z));
                matrix.CreateManager.Create();
            }

            Helper.helpVector.x = matrix.Transform.position.x;
            Helper.helpVector.y = NodeSetting.pet.position.y + 3;
            Helper.helpVector.z = matrix.Transform.position.z;

            matrix.Transform.position = Helper.helpVector;
        }
    }
}
