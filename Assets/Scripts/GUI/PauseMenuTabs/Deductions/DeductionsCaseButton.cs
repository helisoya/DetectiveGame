using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class for the Case button in the deductions tab
/// </summary>
public class DeductionsCaseButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private DeductionsMenuTab tab;
    private Case data;

    /// <summary>
    /// Initialize the button
    /// </summary>
    /// <param name="c">The case</param>
    /// <param name="t">The parent tab</param>
    public void Init(Case c, DeductionsMenuTab t)
    {
        data = c;
        buttonText.text = data.caseName;
        tab = t;
    }

    /// <summary>
    /// Handles the button click
    /// </summary>
    public void Event_Click()
    {
        GameGUI.instance.PlayButtonSFX();
        tab.ChangeCase(data);
    }
}
