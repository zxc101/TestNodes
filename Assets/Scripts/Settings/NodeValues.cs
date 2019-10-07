using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NodeValues
{
    public Transform nodeBox;
    public Transform boxesBase;
    public Transform phone;
    public Color rawNodeColor;
    public Color nodeColor;
    public float wolkDistance;
    public float jumpDistance;
    public int count;
    public LayerMask layerMask;
}
