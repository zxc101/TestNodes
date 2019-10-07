using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PetValues
{
    [Header("Speed")]
    public float maxMoveSpeed;
    [Range(0, 100)]
    [Tooltip("Percent")]
    public int midMoveSpeed;

    public float maxRotateSpeed;
    public float maxJumpUpSpeed;
    public float maxJumpDownSpeed;

    [Header("Angle")]
    public float maxAngle;
    [Range(0, 100)]
    [Tooltip("Percent")]
    public int midAngle;
    [Range(0, 100)]
    [Tooltip("Percent")]
    public int minAngle;

    [Header("Distance")]
    public float stopDistance;
    public float moveDistance;

    [Header("Other")]
    public List<Goal> goals;
}
