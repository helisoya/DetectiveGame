using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    public void SetAwnser(int index, int val)
    {
        currentAwnsers[index] = val;
    }

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


    void StartFailedRoutine()
    {
        if (failed != null)
        {
            StopCoroutine(failed);
        }
        failed = StartCoroutine(Routine_Failed());
    }

    IEnumerator Routine_Failed()
    {
        failedObj.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        failedObj.SetActive(false);
    }

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
