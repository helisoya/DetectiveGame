using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the rain. Making sure it is always on top of it's target. (The player)
/// </summary>
public class RainMovements : MonoBehaviour
{
    private Transform target;
    void Start()
    {
        target = PlayerCameraManager.instance.transform;
    }

    void Update()
    {
        transform.position = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z
        );
    }
}
