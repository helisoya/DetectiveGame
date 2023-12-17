using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Footstep", menuName = "DetectiveGame/Footstep", order = 2)]
public class Footstep : ScriptableObject
{
    public string footstepName;
    public AudioClip footstepSound;
}
