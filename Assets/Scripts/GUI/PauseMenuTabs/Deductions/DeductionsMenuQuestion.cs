using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class for the question dropdown in the deductions tab
/// </summary>
public class DeductionsMenuQuestion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private TMP_Dropdown dropdown;
    private DeductionsMenuTab tab;
    private CaseQuestion data;
    private int index;

    /// <summary>
    /// Initialize the dropdown
    /// </summary>
    /// <param name="c">The question</param>
    /// <param name="i">The index of the question</param>
    /// <param name="t">The parent tab</param>
    public void Init(CaseQuestion c, int i, DeductionsMenuTab t)
    {
        index = i;
        data = c;
        tab = t;

        question.text = c.questionName;

        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(c.awnsers));
    }

    /// <summary>
    /// Called whenever the player changes the awnser to  the question
    /// </summary>
    /// <param name="newVal">The index of the awnser</param>
    public void Event_ChangeValue(int newVal)
    {
        tab.SetAwnser(index, newVal);
    }
}
