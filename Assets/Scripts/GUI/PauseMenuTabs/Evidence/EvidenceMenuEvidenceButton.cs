using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvidenceMenuEvidenceButton : MonoBehaviour
{
    [SerializeField] private Image buttonImg;
    [SerializeField] private Button button;
    [SerializeField] private Sprite evidenceNotFoundSprite;
    private Evidence evidence;
    private EvidenceMenuTab tab;

    public void Init(Evidence e, bool evidenceFound, EvidenceMenuTab t)
    {
        button.interactable = evidenceFound;
        evidence = e;
        buttonImg.sprite = evidenceFound ? evidence.evidenceSmall : evidenceNotFoundSprite;
        tab = t;
    }

    public void Event_Click()
    {
        tab.ChangeEvidence(evidence);
    }
}
