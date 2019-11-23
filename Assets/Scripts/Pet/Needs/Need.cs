using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Need
{
    public Transform prefab;
    public string name;
    public float value;
    public float maxValue;
    public float stepConsumption;
    public float criticalValue;
    public float processingTime;
}
