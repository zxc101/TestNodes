using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] private Transform nodeBox;
    [SerializeField] private Transform boxesBase;
    [SerializeField] private Transform phone;
    [SerializeField] private Color rawNodeColor;
    [SerializeField] private Color nodeColor;
    [SerializeField] private float wolkDistance;
    [SerializeField] private float jumpDistance;
    [SerializeField] private int count;
    [SerializeField] private LayerMask layerMask;

    void Awake()
    {
        NodeSetting.nodeBox = nodeBox;
        NodeSetting.boxesBase = boxesBase;
        NodeSetting.phone = phone;
        NodeSetting.rawNodeColor = rawNodeColor;
        NodeSetting.nodeColor = nodeColor;
        NodeSetting.wolkDistance = wolkDistance;
        NodeSetting.jumpDistance = jumpDistance;
        NodeSetting.count = count;
        NodeSetting.layerMask = layerMask;
}
}
