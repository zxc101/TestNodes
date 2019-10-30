using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    public class Helper
    {
        public static Vector3 helpVector;

        public static float Percent(float max, int percent)
        {
            float res = -1;
            if (percent >= 0 && percent <= 100)
            {
                res = percent * max / 100;
            }
            return res;
        }
    }
}
