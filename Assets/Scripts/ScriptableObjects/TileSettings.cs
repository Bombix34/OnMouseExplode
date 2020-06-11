using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new tile settings", menuName = "Explofun/Tiles Settings")]
public class TileSettings : ScriptableObject
{
    [Header("BOUNCING")]
    [Range(0f,5f)]
    public float BounceSpeed = 1f;

    [Header("SHAKING")]
    [Range(0f, 2f)]
    public float ShakeSpeed = 0.2f;

    [Header("GROWING")]
    [Range(0f, 2f)]
    public float GrowingSpeed = 0.2f;
}
