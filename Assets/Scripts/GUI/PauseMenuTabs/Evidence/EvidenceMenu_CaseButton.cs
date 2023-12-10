using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EvidenceMenu_CaseButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private EvidenceMenuTab tab;
    private Case data;


    public void Init(Case c, EvidenceMenuTab t)
    {
        data = c;
        buttonText.text = data.caseName;
        tab = t;
    }

    public void Event_Click()
    {
        tab.ChangeCase(data);
    }
}
