using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private NodeValues node;
    [SerializeField] private PetValues pet;

    private void Start()
    {
        Node();
        Pet();
    }

    private void Node()
    {
        NodeSetting.nodeBox = node.nodeBox;
        NodeSetting.boxesBase = node.boxesBase;
        NodeSetting.phone = node.phone;
        NodeSetting.rawNodeColor = node.rawNodeColor;
        NodeSetting.nodeColor = node.nodeColor;
        NodeSetting.wolkDistance = node.wolkDistance;
        NodeSetting.jumpDistance = node.jumpDistance;
        NodeSetting.count = node.count;
        NodeSetting.layerMask = node.layerMask;
    }

    private void Pet()
    {
        PetSetting.maxMoveSpeed = pet.maxMoveSpeed;
        PetSetting.midMoveSpeed = pet.midMoveSpeed;

        PetSetting.maxRotateSpeed = pet.maxRotateSpeed;
        PetSetting.maxJumpUpSpeed = pet.maxJumpUpSpeed;
        PetSetting.maxJumpDownSpeed = pet.maxJumpDownSpeed;

        PetSetting.maxAngle = pet.maxAngle;
        PetSetting.midAngle = pet.midAngle;
        PetSetting.minAngle = pet.minAngle;

        PetSetting.stopDistance = pet.stopDistance;
        PetSetting.moveDistance = pet.moveDistance;

        PetSetting.goals = pet.goals;
}
}
