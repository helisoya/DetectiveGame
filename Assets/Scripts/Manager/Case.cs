using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class representing a Case. A case is a short mystery that has to be solved.
/// </summary>
[CreateAssetMenu(fileName = "Case", order = 0, menuName = "DetectiveGame/Case")]
public class Case : ScriptableObject
{
    public string ID;
    public string caseName;
    public Evidence[] evidences;
    public CaseQuestion[] questions;
    public string dialogToStartAfterwards;

}

/// <summary>
/// Case questions are questions that have to be awnsered by the player, in order to end a case.
/// </summary>
[System.Serializable]
public class CaseQuestion
{
    public string questionName;
    public string[] awnsers;
    public int correctAwnser;
}