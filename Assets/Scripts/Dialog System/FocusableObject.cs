using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A focusable object is an object that can be focused on by the camera. It can also turn towards it.
/// </summary>
public class FocusableObject : RepositoryObject
{
    [SerializeField] private Transform _focusOn;
    [SerializeField] private Transform _turn;

    public Transform focusOn
    {
        get
        {
            return _focusOn;
        }
    }


    /// <summary>
    /// Adds the focusable object to the dialog system
    /// </summary>
    public override void AddToRepository()
    {
        DialogMaster.instance.RegisterFocusable(referenceName, this);
    }

    /// <summary>
    /// Turns the object toward the target
    /// </summary>
    /// <param name="target">The target</param>
    public void TurnToward(Transform target)
    {
        _turn.LookAt(target);
        _turn.eulerAngles = new Vector3(0f, _turn.eulerAngles.y, 0f);
    }
}
