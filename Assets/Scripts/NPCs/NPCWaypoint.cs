using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypoint : MonoBehaviour
{
    [SerializeField] private string referenceName;

    void Start()
    {
        DialogMaster.instance.RegisterNPCWaypoints(referenceName, transform);
    }
}
