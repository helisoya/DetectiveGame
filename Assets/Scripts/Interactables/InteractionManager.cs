using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Interactable Objects & Interactions
/// </summary>
public class InteractionManager : MonoBehaviour
{
    [SerializeField] private PlayerCameraManager cameraManager;
    [SerializeField] private float minimumDistance;
    [SerializeField] private LayerMask exitMask;
    private InteractableObject currentInteractable;
    private Transform playerPos;
    private Transform cam;
    public static InteractionManager instance;


    void Awake()
    {
        playerPos = cameraManager.transform;
        instance = this;
        CursorManager.ChangeCursorTex(null);
    }

    void Start()
    {
        cam = Camera.main.transform;
    }

    /// <summary>
    /// Changes the currently focused on object if possible
    /// </summary>
    /// <param name="current">The new object</param>
    public void SetCurrentObject(InteractableObject current)
    {
        if (DialogMaster.instance.inDialog || GameGUI.instance.inMenu || GameGUI.instance.playingMiniGame) return;

        currentInteractable = current;
        CursorManager.ChangeCursorTex(current.hoverSprite);
        Cursor.visible = true;
    }

    /// <summary>
    /// Remove the current object
    /// </summary>
    /// <param name="old">The current object</param>
    /// <param name="force">Force the removal ? (if in dialog)</param>
    public void RemoveCurrentObject(InteractableObject old, bool force = false)
    {
        if (!force && DialogMaster.instance.inDialog) return;

        if (currentInteractable == old)
        {
            currentInteractable = null;

            if (GameGUI.instance.inMenu)
            {
                GameGUI.instance.SetLastCursorTex(null);
            }
            else
            {
                CursorManager.ChangeCursorTex(null);
                if (cameraManager.isInFirstPerson && !force)
                {
                    Cursor.visible = false;
                }
            }


        }
    }

    /// <summary>
    /// Checks if the distance is good for an interaction
    /// </summary>
    /// <returns>Is the distance good ?</returns>
    bool DistanceRequired()
    {
        return Vector3.Distance(playerPos.position, currentInteractable.transform.position) <= minimumDistance;
    }

    void Update()
    {
        if (DialogMaster.instance.inDialog || GameGUI.instance.inMenu)
        {
            Cursor.visible = true;
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,
            minimumDistance + Vector3.Distance(playerPos.position, cam.position), exitMask))
        {
            InteractableObject obj = hit.transform.GetComponent<InteractableObject>();

            if ((!obj || obj != currentInteractable) && currentInteractable)
            {
                currentInteractable.OnExit();
            }

            if (obj)
            {
                obj.OnEnter();
            }
        }
        else
        {
            if (currentInteractable)
            {
                currentInteractable.OnExit();
            }
        }


        if (currentInteractable != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentInteractable.OnInteraction();
                RemoveCurrentObject(currentInteractable, true);
            }
        }
        else
        {
            Cursor.visible = cameraManager.isInFirstPerson ? false : true;
        }
    }
}
