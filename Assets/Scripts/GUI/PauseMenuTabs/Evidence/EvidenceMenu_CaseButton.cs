using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles the case button in the evidence menu tab
/// </summary>
public class EvidenceMenu_CaseButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private EvidenceMenuTab tab;
    private Case data;

    /// <summary>
    /// Initialize the button
    /// </summary>
    /// <param name="c">The case</param>
    /// <param name="t">The parent tab</param>
    public void Init(Case c, EvidenceMenuTab t)
    {
        data = c;
        buttonText.text = data.caseName;
        tab = t;
    }

    /// <summary>
    /// Handles the click
    /// </summary>
    public void Event_Click()
    {
        GameGUI.instance.PlayButtonSFX();
        tab.ChangeCase(data);
    }
}
