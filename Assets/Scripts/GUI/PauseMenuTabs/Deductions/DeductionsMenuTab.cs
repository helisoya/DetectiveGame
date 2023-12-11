using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Class for the deduction menu tab
/// </summary>
public class DeductionsMenuTab : PauseMenuTab
{
    [Header("Deductions Screen")]
    [SerializeField] private GameObject prefabCaseButton;
    [SerializeField] private Transform casesRoot;
    [SerializeField] private GameObject failedObj;

    [Header("Case Specifics")]
    [SerializeField] private GameObject caseInfosRoot;
    [SerializeField] private GameObject notEnoughEvidenceRoot;
    [SerializeField] private GameObject enoughEvidenceRoot;
    [SerializeField] private TextMeshProUGUI currentCaseNameText;
    [SerializeField] private GameObject prefabDropdown;
    [SerializeField] private Transform dropDownsRoot;
    [SerializeField] private Button validButton;


    private Coroutine failed;




    private int[] currentAwnsers;
    private Case currentCase;

    /// <summary>
    /// Finds if the player owns all the evidence related to a case
    /// </summary>
    /// <param name="c">The case</param>
    /// <returns>Does the player owns all evidence related to this case ?</returns>
    bool HasAllEvidenceForCase(Case c)
    {
        foreach (Evidence evidence in c.evidences)
        {
            if (!GameManager.instance.HasUnlockedEvidence(evidence))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Changes the current case
    /// </summary>
    /// <param name="newCase">The new case</param>
    public void ChangeCase(Case newCase)
    {
        caseInfosRoot.SetActive(true);
        currentCase = newCase;

        currentCaseNameText.text = currentCase.caseName;

        if (HasAllEvidenceForCase(newCase))
        {
            enoughEvidenceRoot.SetActive(true);
            notEnoughEvidenceRoot.SetActive(false);
            currentAwnsers = new int[newCase.questions.Length];
            validButton.interactable = !DialogMaster.instance.inDialog;

            foreach (Transform child in dropDownsRoot)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < newCase.questions.Length; i++)
            {
                Instantiate(prefabDropdown, dropDownsRoot).GetComponent<DeductionsMenuQuestion>().Init(newCase.questions[i], i, this);
            }
        }
        else
        {
            enoughEvidenceRoot.SetActive(false);
            notEnoughEvidenceRoot.SetActive(true);
        }

    }

    /// <summary>
    /// Changes the awnser for a questions
    /// </summary>
    /// <param name="index">The question index</param>
    /// <param name="val">The awnser index</param>
    public void SetAwnser(int index, int val)
    {
        currentAwnsers[index] = val;
    }

    /// <summary>
    /// Handles the confirmation of the deductions
    /// </summary>
    public void Event_ClickConfirm()
    {
        for (int i = 0; i < currentCase.questions.Length; i++)
        {
            if (currentAwnsers[i] != currentCase.questions[i].correctAwnser)
            {
                StartFailedRoutine();
                return;
            }
        }

        StartCoroutine(Routine_StartEndCaseDialog(currentCase));
    }

    /// <summary>
    /// Closes the pause menu and ends the case
    /// </summary>
    /// <param name="c">The case to end</param>
    /// <returns>IEnumerator</returns>
    IEnumerator Routine_StartEndCaseDialog(Case c)
    {
        currentCase = null;
        GameGUI.instance.ClosePauseMenu(false);
        yield return new WaitForEndOfFrame();
        GameManager.instance.Save_EndCase(c.ID);

        if (!string.IsNullOrEmpty(c.dialogToStartAfterwards))
        {
            DialogMaster.instance.StartDialog(c.dialogToStartAfterwards);
        }
    }

    /// <summary>
    /// Starts the deduction failed routine
    /// </summary>
    void StartFailedRoutine()
    {
        if (failed != null)
        {
            StopCoroutine(failed);
        }
        failed = StartCoroutine(Routine_Failed());
    }

    /// <summary>
    /// Shows a text for a few seconds, then hide it
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator Routine_Failed()
    {
        failedObj.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        failedObj.SetActive(false);
    }

    /// <summary>
    /// Refreshes the tab
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();

        if (currentCase == null)
        {
            caseInfosRoot.SetActive(false);
        }
        else
        {
            ChangeCase(currentCase);
        }

        foreach (Transform child in casesRoot)
        {
            Destroy(child.gameObject);
        }

        foreach (string currentCase in GameManager.instance.save_currentCases)
        {
            Case c = Resources.Load<Case>("Cases/" + currentCase);
            if (c.questions.Length > 0)
            {
                Instantiate(prefabCaseButton, casesRoot).GetComponent<DeductionsCaseButton>().Init(
                    c,
                    this
                );
            }

        }

        casesRoot.GetComponent<RectTransform>().sizeDelta = new Vector2(
            casesRoot.GetComponent<RectTransform>().sizeDelta.x,
            (prefabCaseButton.GetComponent<RectTransform>().sizeDelta.y + 5) * GameManager.instance.save_currentCases.Count
        );
    }
}
