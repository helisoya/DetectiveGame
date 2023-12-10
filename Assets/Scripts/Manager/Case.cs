using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Case", order = 0, menuName = "DetectiveGame/Case")]
public class Case : ScriptableObject
{
    public string ID;
    public string caseName;
    public Evidence[] evidences;
    public CaseQuestion[] questions;
    public string dialogToStartAfterwards;

}

[System.Serializable]
public class CaseQuestion
{
    public string questionName;
    public string[] awnsers;
    public int correctAwnser;
}