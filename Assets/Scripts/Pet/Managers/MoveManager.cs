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
            float hight = pet.Goal.y;
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

        private void Move(float hight)
        {
            StabilizationY(hight);

            RaycastHit hit;

            if (Physics.Raycast(pet.Eye.position, pet.transform.forward, out hit, 1, NodeSetting.layerMask))
            {
                SlowdownFrontItem(hit.point);
            }
            else
            {
                CorrectMoveSpeed();
            }
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

        private void SlowdownFrontItem(Vector3 itemPosition)
        {
            float dist = Vector3.Distance(pet.transform.position, itemPosition);

            if (dist < 0.5f)
            {
                pet.SpeedMove = 0;
                //float angle;

                //if (pet.Goal != null) angle = MathHelper.Angle(pet.transform, pet.Goal) == 0 ? 1 : MathHelper.Angle(pet.transform, pet.Goal);
                //else angle = MathHelper.Angle(pet.transform, itemPosition) == 0 ? 1 : MathHelper.Angle(pet.transform, itemPosition);

                //pet.SpeedRotate = Mathf.Lerp(pet.SpeedRotate, angle, Time.fixedDeltaTime * pet.TIME_ROTATE);
            }
            else
            {
                pet.SpeedMove = 1;
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
                pet.SpeedRotate = Mathf.Lerp(pet.SpeedRotate, pet.AngleToGoal, Time.fixedDeltaTime * pet.TIME_ROTATE);
            }

            pet.SpeedMove = pet.MaxSpeed;
        }
    }
}
