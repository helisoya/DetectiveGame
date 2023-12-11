using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the evidence button in the evidence menu tab
/// </summary>
public class EvidenceMenuEvidenceButton : MonoBehaviour
{
    [SerializeField] private Image buttonImg;
    [SerializeField] private Button button;
    [SerializeField] private Sprite evidenceNotFoundSprite;
    private Evidence evidence;
    private EvidenceMenuTab tab;

    /// <summary>
    /// Initialize the button
    /// </summary>
    /// <param name="e">The evidence</param>
    /// <param name="evidenceFound">Was the evidence found ?</param>
    /// <param name="t">The parent tab</param>
    public void Init(Evidence e, bool evidenceFound, EvidenceMenuTab t)
    {
        button.interactable = evidenceFound;
        evidence = e;
        buttonImg.sprite = evidenceFound ? evidence.evidenceSmall : evidenceNotFoundSprite;
        tab = t;
    }

    /// <summary>
    /// Handles the button click
    /// </summary>
    public void Event_Click()
    {
        tab.ChangeEvidence(evidence);
    }
}
