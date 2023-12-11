using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable objects that changes the current Scene
/// </summary>
public class TravelInteractable : InteractableObject
{
    [Header("Travel Interaction")]
    [SerializeField] private string sceneName;
    [SerializeField] private TravelInteractableOverride[] overrides;

    private bool used = false;

    /// <summary>
    /// Changes scene if there are no conflicts with the overrides
    /// </summary>
    public override void OnInteraction()
    {
        if (used) return;

        int storyVal = int.Parse(GameManager.instance.GetSaveItemValue("STORY"));

        if (overrides != null && overrides.Length > 0)
        {
            foreach (TravelInteractableOverride travelOverride in overrides)
            {
                if (
                    (travelOverride.sign == '=' && storyVal == travelOverride.storyPoint) ||
                    (travelOverride.sign == '<' && storyVal < travelOverride.storyPoint) ||
                    (travelOverride.sign == '>' && storyVal > travelOverride.storyPoint)
                    )
                {

                    DialogMaster.instance.StartDialog(travelOverride.dialogName);
                    return;
                }
            }
        }

        used = true;
        GameManager.instance.LoadMap(sceneName);

    }

    /// <summary>
    /// An override dialog that will be processed if the requirement is met
    /// </summary>
    [System.Serializable]
    public struct TravelInteractableOverride
    {
        public string dialogName;
        public char sign;
        public int storyPoint;

    }
}
