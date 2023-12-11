using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the Evidence menu tab
/// </summary>
public class EvidenceMenuTab : PauseMenuTab
{
    [Header("Evidence Tab")]
    [SerializeField] private Transform doneCasesRoot;
    [SerializeField] private Transform currentCasesRoot;
    [SerializeField] private GameObject evidenceButtonPrefab;
    [SerializeField] private GameObject caseButtonPrefab;


    [Header("Left Tab")]
    [SerializeField] private GameObject leftRootTab;
    [SerializeField] private Transform evidenceRoot;
    [SerializeField] private TextMeshProUGUI caseText;


    [Header("Right Tab")]
    [SerializeField] private GameObject rightRootTab;
    [SerializeField] private TextMeshProUGUI evidenceNameText;
    [SerializeField] private TextMeshProUGUI evidenceDescText;
    [SerializeField] private Image evidenceImg;


    private Case currentCase = null;
    private Evidence currentEvidence = null;

    /// <summary>
    /// Changes the current case
    /// </summary>
    /// <param name="newID">The new case</param>
    public void ChangeCase(Case newID)
    {
        currentCase = newID;
        leftRootTab.SetActive(true);
        rightRootTab.SetActive(false);

        caseText.text = currentCase.caseName;
        foreach (Transform child in evidenceRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (Evidence evidence in currentCase.evidences)
        {
            Instantiate(evidenceButtonPrefab, evidenceRoot).GetComponent<EvidenceMenuEvidenceButton>().Init(
                evidence,
                GameManager.instance.HasUnlockedEvidence(evidence),
                this);
        }
    }

    /// <summary>
    /// Changes the current evidence
    /// </summary>
    /// <param name="newEvidence">The new evidence</param>
    public void ChangeEvidence(Evidence newEvidence)
    {
        currentEvidence = newEvidence;
        rightRootTab.SetActive(true);

        evidenceNameText.text = currentEvidence.evidenceName;
        evidenceDescText.text = currentEvidence.evidenceDesc;
        evidenceImg.sprite = currentEvidence.evidenceBig;
    }

    /// <summary>
    /// Refreshes the tab
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();

        foreach (Transform child in doneCasesRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (Transform child in currentCasesRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (string caseID in GameManager.instance.save_currentCases)
        {
            Instantiate(caseButtonPrefab, currentCasesRoot).GetComponent<EvidenceMenu_CaseButton>().Init(
                Resources.Load<Case>("Cases/" + caseID),
                this);
        }

        foreach (string caseID in GameManager.instance.save_pastCases)
        {
            Instantiate(caseButtonPrefab, doneCasesRoot).GetComponent<EvidenceMenu_CaseButton>().Init(
                Resources.Load<Case>("Cases/" + caseID),
                this);
        }

        doneCasesRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            doneCasesRoot.GetComponent<RectTransform>().sizeDelta.x,
            (caseButtonPrefab.GetComponent<RectTransform>().sizeDelta.y + 5) * GameManager.instance.save_pastCases.Count
            );

        currentCasesRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            currentCasesRoot.GetComponent<RectTransform>().sizeDelta.x,
            (caseButtonPrefab.GetComponent<RectTransform>().sizeDelta.y + 5) * GameManager.instance.save_currentCases.Count
            );


        if (currentCase == null)
        {
            leftRootTab.SetActive(false);
            rightRootTab.SetActive(false);
        }
        else
        {
            ChangeCase(currentCase);

            if (currentEvidence != null)
            {
                ChangeEvidence(currentEvidence);
            }
        }
    }
}
