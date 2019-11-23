using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pets
{
    public class AnimManager
    {
        private Pet pet;

        public AnimManager(Pet _pet)
        {
            pet = _pet;
        }

        public void Jump(DirectionY direction, bool isJump)
        {
            switch (direction)
            {
                case DirectionY.Up:
                    pet.Animator.SetBool("IsJumpUp", isJump);
                    break;
                case DirectionY.Down:
                    pet.Animator.SetBool("IsJumpDown", isJump);
                    break;
            }
        }

        public void Need(string name, bool isNeed)
        {
            pet.Animator.SetBool(name, isNeed);
        }

        public bool IsNeed(string name)
        {
            return pet.Animator.GetBool(name);
        }
    }
}
