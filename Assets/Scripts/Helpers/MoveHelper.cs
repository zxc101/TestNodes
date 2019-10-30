using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class MoveHelper
    {
        public static void Move(Transform transform, float speed)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public static void Rotate()
        {

        }

        public static void JumpUp()
        {

        }

        public static void JumpDown()
        {

        }
    }
}
