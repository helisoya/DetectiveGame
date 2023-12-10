using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkyData", menuName = "DetectiveGame/SkyData", order = 2)]
public class SkyData : ScriptableObject
{
    public Material skybox;
    public float sunIntensity;
    public Color sunColor;
    public Vector3 sunRotation;
    public bool rain;
}
