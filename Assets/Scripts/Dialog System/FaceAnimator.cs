using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FaceAnimator : RepositoryObject
{
    public override void AddToRepository()
    {
        DialogMaster.instance.RegisterFaceAnimation(referenceName, GetComponent<Animator>());
    }
}
