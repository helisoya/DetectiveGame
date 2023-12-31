using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSwapper : MonoBehaviour
{
    private GroundChecker groundChecker;
    [SerializeField] private FootstepSource footstepSource;
    [SerializeField] private LayerMask layerMask;
    private string currentLayer;
    private static Dictionary<string, Footstep> footsteps;

    void Awake()
    {
        groundChecker = new GroundChecker();

        if (footsteps != null && footsteps.Keys.Count > 0) return;
        Footstep[] allFootsteps = Resources.LoadAll<Footstep>("Footsteps");
        footsteps = new Dictionary<string, Footstep>();

        foreach (Footstep footstep in allFootsteps)
        {
            footsteps.Add(footstep.footstepName, footstep);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * 3
        );
    }

    /// <summary>
    /// Checks the current layer for the footstep sound
    /// </summary>
    public void CheckLayer()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, 3, layerMask))
        {
            Terrain terrain = hit.transform.GetComponent<Terrain>();

            if (terrain)
            {
                string newLayer = groundChecker.GetLayerName(transform.position, terrain);
                if (currentLayer != newLayer)
                {
                    currentLayer = newLayer;
                    footstepSource.ChangeClip(footsteps[newLayer].footstepSound);
                }
            }
            else
            {
                FootstepSurface surface = hit.transform.GetComponent<FootstepSurface>();
                if (surface && currentLayer != surface.footstepData.footstepName)
                {
                    currentLayer = surface.footstepData.footstepName;
                    footstepSource.ChangeClip(surface.footstepData.footstepSound);
                }
            }
        }
    }
}
