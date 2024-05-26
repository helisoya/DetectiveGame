using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class reprenseting an interraction triggered by a collision box
/// </summary>
public class TriggerInteractable : MonoBehaviour
{
    /// <summary>
    /// Callback for OnTriggerEnter events
    /// </summary>
    /// <param name="other">The collider that entered the trigger</param>
    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            OnInterraction();
        }
    }


    /// <summary>
    /// Launches the interraction
    /// </summary>
    public virtual void OnInterraction()
    {
        print("Triggered : " + gameObject.name);
    }
}
