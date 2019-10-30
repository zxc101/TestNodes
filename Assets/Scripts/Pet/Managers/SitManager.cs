using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class SitManager
    {
        private Pet pet;

        public SitManager(Pet _pet)
        {
            pet = _pet;
        }

        private IEnumerator Start()
        {
            Debug.Log("Sit");
            yield return new WaitForSeconds(1);
        }
    }
}
