using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObject : MonoBehaviour
{
    [SerializeField] private string[] enablingConditions;
    [SerializeField] private bool needAll;

    private bool computed = false;
    private bool value;

    void Start()
    {
        StoryObjectsManager.instance.RegisterStoryObject(this);
        RefreshIsVisible();
    }

    public bool GetCanBeEnabled()
    {
        if (computed) return value;

        value = CanBeEnabled();
        computed = true;
        return value;
    }

    bool CanBeEnabled()
    {
        string[] args;
        foreach (string condition in enablingConditions)
        {
            args = condition.Split(";");

            string keyValue = GameManager.instance.GetSaveItemValue(args[0]);

            bool value = false;

            switch (args[1])
            {
                case "=":
                    value = args[2].Equals(keyValue);

                    break;
                case "<":
                    value = int.Parse(keyValue) < int.Parse(args[2]);
                    break;
                case ">":
                    value = int.Parse(keyValue) > int.Parse(args[2]);
                    break;
            }

            if (value && !needAll)
            {
                return true;
            }
            else if (!value && needAll)
            {
                return false;
            }
        }

        return needAll;
    }


    public void RefreshIsVisible()
    {
        computed = false;
        gameObject.SetActive(GetCanBeEnabled());
    }
}
