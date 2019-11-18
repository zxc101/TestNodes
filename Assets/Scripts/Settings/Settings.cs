using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private NodeValues node;

    private void Start()
    {
        Node();
    }

    private void Node()
    {
        NodeSetting.boxesBase = node.boxesBase;
        NodeSetting.pet = node.pet;
        NodeSetting.wolkDistance = node.wolkDistance;
        NodeSetting.jumpDistance = node.jumpDistance;
        NodeSetting.count = node.count;
        NodeSetting.layerMask = node.layerMask;
    }
}
