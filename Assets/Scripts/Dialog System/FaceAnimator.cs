using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The face animator registers a face animator to the dialog system
/// </summary>
[RequireComponent(typeof(Animator))]
public class FaceAnimator : RepositoryObject
{
    /// <summary>
    /// Add the face animator to the dialog system
    /// </summary>
    public override void AddToRepository()
    {
        DialogMaster.instance.RegisterFaceAnimation(referenceName, GetComponent<Animator>());
    }
}
