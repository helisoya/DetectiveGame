using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeductionsMenuQuestion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private TMP_Dropdown dropdown;
    private DeductionsMenuTab tab;
    private CaseQuestion data;
    private int index;

    public void Init(CaseQuestion c, int i, DeductionsMenuTab t)
    {
        index = i;
        data = c;
        tab = t;

        question.text = c.questionName;

        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>(c.awnsers));
    }

    public void Event_ChangeValue(int newVal)
    {
        tab.SetAwnser(index, newVal);
    }
}
