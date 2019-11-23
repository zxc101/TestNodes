using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class NeedsManager
    {
        private Pet pet;

        public NeedsManager(Pet _pet)
        {
            pet = _pet;
        }

        public IEnumerator Processing()
        {
            while (pet.Need.value < pet.Need.maxValue)
            {
                yield return new WaitForSeconds(pet.Need.processingTime);
                pet.Need.value++;
            }
            pet.AnimManager.Need(pet.Need.name, false);
        }

        public IEnumerator Consumptions()
        {
            while (true)
            {
                yield return new WaitForSeconds(pet.TimeConsumptionNeeds);
                for (int i = 0; i < pet.Needs.Length; i++)
                {
                    if (!pet.AnimManager.IsNeed(pet.Needs[i].name))
                    {
                        pet.Needs[i].value -= pet.Needs[i].stepConsumption;
                    }
                }
            }
        }
    }
}
