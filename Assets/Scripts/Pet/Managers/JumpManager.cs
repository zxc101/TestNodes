using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class JumpManager
    {
        private Pet pet;

        public JumpManager(Pet _pet)
        {
            pet = _pet;
        }

        public void Jump(DirectionY direction)
        {
            if (Mathf.Abs(pet.AngleToGoal) > 14)
            {
                RotateBeforeJump();
            }
            else
            {
                pet.AnimManager.Jump(direction, true);
            }
        }

        private void RotateBeforeJump()
        {
            pet.SpeedRotate = Mathf.Lerp(pet.SpeedRotate, pet.AngleToGoal, Time.fixedDeltaTime * pet.TIME_ROTATE);
            pet.SpeedMove = Mathf.Lerp(pet.SpeedMove, 0, Time.fixedDeltaTime * pet.TIME_MOVE);
        }
    }
}
