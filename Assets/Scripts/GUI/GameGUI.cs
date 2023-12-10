using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class GameGUI : MonoBehaviour
{
    [Header("Dialog System")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private TextMeshProUGUI textCharacter;
    [SerializeField] private GameObject continueDialogMark;
    [HideInInspector] public bool dialogPassed;


    [Header("Mini Games")]
    [SerializeField] private LockpickMiniGame lockpickMiniGame;



    [Header("Choice System")]
    [SerializeField] private Transform choiceRoot;
    [SerializeField] private GameObject choicePrefab;

    [Header("Fadings")]
    [SerializeField] private FadeManager fadeBg;
    [SerializeField] private FadeManager fadeFg;
    public bool isFadingBackground
    {
        get
        {
            return fadeBg.isFading;
        }
    }

    public bool isFadingForeground
    {
        get
        {
            return fadeFg.isFading;
        }
    }

    [Header("Detective Vision")]
    [SerializeField] private GameObject detectiveVisionObj;


    [Header("CG")]
    [SerializeField] private FadeImage cgFade;
    public bool isFadingCG
    {
        get
        {
            return cgFade.isFading;
        }
    }


    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuRoot;
    [SerializeField] private PauseMenuTab lastOpenedTab;
    private CursorLockMode lastCursorLockMode;
    private Texture2D lastCursorImg;
    private bool lastVisible;

    public bool inMenu
    {
        get
        {
            return pauseMenuRoot.activeInHierarchy;
        }
    }

    public bool playingMiniGame
    {
        get
        {
            return lockpickMiniGame.isPlaying;
        }
    }


    public static GameGUI instance;


    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (pauseMenuRoot.activeInHierarchy) ClosePauseMenu();
            else OpenPauseMenu();
        }
    }

    /**
    ===============================================================================================
    ================================ Fadings ======================================================
    ===============================================================================================
    **/


    public void FadeBackgroundTo(float alpha, float speed)
    {
        fadeBg.FadeTo(alpha, speed);
    }

    public void FadeForegroundTo(float alpha, float speed)
    {
        fadeFg.FadeTo(alpha, speed);
    }


    public void ShowCG(Sprite sprite, float speed)
    {
        cgFade.SetImage(sprite);
        cgFade.FadeTo(1, speed);
    }

    public void HideCG(float speed)
    {
        cgFade.FadeTo(0, speed);
    }

    /**
    ===============================================================================================
    ============================== Detective Vision ===============================================
    ===============================================================================================
    **/

    public void SetDetectiveVisionActive(bool activated)
    {
        detectiveVisionObj.SetActive(activated);
    }

    /**
    ===============================================================================================
    ============================= Mini Games ======================================================
    ===============================================================================================
    **/

    public void StartLockPickMiniGame(string dialogAfterward)
    {
        lockpickMiniGame.StartMiniGame(dialogAfterward);
    }

    /**
    ===============================================================================================
    ============================= Pause Menu ======================================================
    ===============================================================================================
    **/


    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenuRoot.SetActive(true);
        lastOpenedTab.Refresh();

        lastCursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;

        lastCursorImg = CursorManager.currentTex;
        CursorManager.ChangeCursorTex(null);

        lastVisible = Cursor.visible;
        Cursor.visible = true;
    }

    public void ClosePauseMenu(bool ignoreMouseValues = false)
    {
        Time.timeScale = 1;
        pauseMenuRoot.SetActive(false);

        if (!ignoreMouseValues)
        {
            Cursor.lockState = lastCursorLockMode;
            CursorManager.ChangeCursorTex(lastCursorImg);
            Cursor.visible = lastVisible;
        }

    }

    public void ChangeCurrentPauseMenuTab(PauseMenuTab newTab)
    {
        lastOpenedTab.Close();
        newTab.Open();
        lastOpenedTab = newTab;
    }

    public void SetLastCursorTex(Texture2D tex)
    {
        lastCursorImg = tex;
    }


    /**
    ===============================================================================================
    ================================ Dialogs ======================================================
    ===============================================================================================
    **/

    public void ShowDialog(string speaker, string dialog)
    {
        dialogPassed = false;
        textDialog.text = dialog;
        textCharacter.text = speaker;
        dialogRoot.SetActive(true);
    }

    public void Click_Dialog()
    {
        dialogPassed = true;
    }

    public void HideDialog()
    {
        dialogPassed = false;
        dialogRoot.SetActive(false);
    }


    public void ShowChoice(List<string[]> choices)
    {
        DeleteChoices();
        dialogRoot.gameObject.SetActive(false);
        choiceRoot.gameObject.SetActive(true);

        for (int i = 0; i < choices.Count; i++)
        {
            Instantiate(choicePrefab, choiceRoot).GetComponent<ChoiceButton>().SetChoice(choices[i][0], i);
        }
    }


    public void Event_Choice(int choice)
    {
        DeleteChoices();
        dialogRoot.gameObject.SetActive(false);

        DialogMaster.instance.EndChoice(choice);
    }

    void DeleteChoices()
    {
        foreach (Transform child in choiceRoot)
        {
            Destroy(child.gameObject);
        }
    }

}
