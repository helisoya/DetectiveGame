using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeductionsCaseButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private DeductionsMenuTab tab;
    private Case data;


    public void Init(Case c, DeductionsMenuTab t)
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
