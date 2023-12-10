using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Evidence", order = 1, menuName = "DetectiveGame/Evidence")]
public class Evidence : ScriptableObject
{
    public string ID;
    public Case caseRef;
    public string evidenceName;
    public string evidenceDesc;
    public Sprite evidenceSmall;
    public Sprite evidenceBig;
}
