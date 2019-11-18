using System.Collections;
using UnityEngine;
using Helpers;

namespace Pets
{
    public class MoveManager
    {
        private Pet pet;

        public MoveManager(Pet _pet)
        {
            pet = _pet;
        }

        public IEnumerator Start()
        {
            if (pet.Path == null) yield return new WaitForFixedUpdate();

            float hight = pet.NextPosition.y;
            if (pet.Transform.position.y < hight + 0.1f && pet.Transform.position.y < hight - 0.1f)
            {
                pet.JumpManager.Jump(DirectionY.Up);
            }
            else if (pet.Transform.position.y > hight + 0.1f && pet.Transform.position.y > hight - 0.1f)
            {
                pet.JumpManager.Jump(DirectionY.Down);
            }
            else
            {
                Move(hight);
            }
            yield return new WaitForFixedUpdate();
        }

        public void Stop()
        {
            pet.SpeedMove = 0;
            pet.SpeedRotate = 0;
        }

        private void Move(float hight)
        {
            StabilizationY(hight);
            CorrectMoveSpeed();
        }

        private bool IsDirectionWall(out RaycastHit hit, Vector3 direction)
        {
            return Physics.Raycast(pet.Eye.position, direction, out hit, 1, NodeSetting.layerMask);
        }

        private void StabilizationY(float hight)
        {
            if (pet.transform.position.y != hight)
            {
                pet.AnimManager.Jump(DirectionY.Up, false);
                pet.AnimManager.Jump(DirectionY.Down, false);
                Helper.helpVector.x = pet.transform.position.x;
                Helper.helpVector.y = hight;
                Helper.helpVector.z = pet.transform.position.z;
                pet.transform.position = Helper.helpVector;
            }
        }

        private void CorrectMoveSpeed()
        {
            if ((Mathf.Abs(pet.transform.forward.x) == 0.7f && Mathf.Abs(pet.transform.forward.z) == 0.7f) ||
                (Mathf.Abs(pet.transform.forward.x) == 1 && Mathf.Abs(pet.transform.forward.z) == 1))
            {
                pet.SpeedRotate = 0;
            }
            else
            {
                pet.SpeedRotate = Mathf.Lerp(pet.SpeedRotate, MathHelper.Angle(pet.Transform, pet.NextPosition), Time.fixedDeltaTime * pet.TIME_ROTATE);
            }

            pet.SpeedMove = 2;
        }
    }
}
