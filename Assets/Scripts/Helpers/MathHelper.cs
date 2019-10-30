using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class MathHelper
    {
        public static float Angle(Transform start, Vector3 goal)
        {
            Vector3 goalPos = goal;
            goalPos.y = start.position.y;

            Vector3 targetDir = goalPos - start.position;

            float angleRotate = Vector3.SignedAngle(targetDir, start.forward, Vector3.up);

            angleRotate = angleRotate * -1;

            if (Mathf.Abs(angleRotate) < 2f)
            {
                angleRotate = 0;
            }

            return angleRotate;
        }
    }
}
