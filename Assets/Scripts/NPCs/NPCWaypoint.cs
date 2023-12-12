using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// NPCWaypoints are Waypoints that the NPC can move towards to.
/// </summary>
public class NPCWaypoint : MonoBehaviour
{
    [SerializeField] private string referenceName;

    /// <summary>
    /// Register the Waypoint to the dialog system
    /// </summary>
    void Start()
    {
        DialogMaster.instance.RegisterNPCWaypoints(referenceName, transform);
    }
}
