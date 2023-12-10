using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    public override void AddToRepository()
    {
        DialogMaster.instance.RegisterFocusable(referenceName, this);
    }

    public void TurnToward(Transform target)
    {
        _turn.LookAt(target);
        _turn.eulerAngles = new Vector3(0f, _turn.eulerAngles.y, 0f);
    }
}
