using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


/// <summary>
/// Handles the GUI
/// </summary>
public class GameGUI : MonoBehaviour
{
    [Header("Dialog System")]
    [SerializeField] private GameObject dialogRoot;
    [SerializeField] private TextMeshProUGUI textDialog;
    [SerializeField] private TextMeshProUGUI textCharacter;
    [SerializeField] private GameObject continueDialogMark;
    [HideInInspector] public bool dialogPassed;
    [SerializeField] private float typingSpeed;
    private bool dialogSkip;
    private Coroutine routineTyping;


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

    [Header("Sound Effects")]
    [SerializeField] private AudioClip sfxMenuOpen;
    [SerializeField] private AudioClip sfxTypewritter;
    [SerializeField] private AudioClip sfxTabChange;
    [SerializeField] private AudioClip sfxMenuButton;


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

    /// <summary>
    /// Fades the Background layer (doesn't hide dialogs)
    /// </summary>
    /// <param name="alpha">Fade to (0 = transparent, 1 = opaque)</param>
    /// <param name="speed">Fade speed</param>
    public void FadeBackgroundTo(float alpha, float speed)
    {
        fadeBg.FadeTo(alpha, speed);
    }

    /// <summary>
    /// Fades the Foreground layer (hides dialogs too)
    /// </summary>
    /// <param name="alpha">Fade to (0 = transparent, 1 = opaque)</param>
    /// <param name="speed">Fade speed</param>
    public void FadeForegroundTo(float alpha, float speed)
    {
        fadeFg.FadeTo(alpha, speed);
    }

    /// <summary>
    /// Shows a CG
    /// </summary>
    /// <param name="sprite">CG sprite</param>
    /// <param name="speed">CG appearence speed</param>
    public void ShowCG(Sprite sprite, float speed)
    {
        cgFade.SetImage(sprite);
        cgFade.FadeTo(1, speed);
    }

    /// <summary>
    /// Hides a CG
    /// </summary>
    /// <param name="speed">Hide speed</param>
    public void HideCG(float speed)
    {
        cgFade.FadeTo(0, speed);
    }

    /**
    ===============================================================================================
    ============================== Detective Vision ===============================================
    ===============================================================================================
    **/

    /// <summary>
    /// Enables the detective vision (only for the GUI)
    /// </summary>
    /// <param name="activated">Detective Vision is active ?</param>
    public void SetDetectiveVisionActive(bool activated)
    {
        detectiveVisionObj.SetActive(activated);
    }

    /**
    ===============================================================================================
    ============================= Mini Games ======================================================
    ===============================================================================================
    **/

    /// <summary>
    /// Starts the lockpick minigame
    /// </summary>
    /// <param name="dialogAfterward">Dialog to load afterwards if won</param>
    public void StartLockPickMiniGame(string dialogAfterward)
    {
        lockpickMiniGame.StartMiniGame(dialogAfterward);
    }

    /**
    ===============================================================================================
    ============================= Pause Menu ======================================================
    ===============================================================================================
    **/

    /// <summary>
    /// Opens the pause menu
    /// </summary>
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
        AudioManager.instance.PlaySFX(sfxMenuOpen);
    }

    /// <summary>
    /// Closes the pause menu
    /// </summary>
    /// <param name="ignoreMouseValues">Should mouse details be restored ? (default = false)</param>
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
        AudioManager.instance.PlaySFX(sfxMenuOpen);
    }

    /// <summary>
    /// Changes the pause menu tab
    /// </summary>
    /// <param name="newTab">The new tab</param>
    public void ChangeCurrentPauseMenuTab(PauseMenuTab newTab)
    {
        lastOpenedTab.Close();
        newTab.Open();
        lastOpenedTab = newTab;
        AudioManager.instance.PlaySFX(sfxTabChange);
    }

    /// <summary>
    /// Saves the last cursor texture known
    /// </summary>
    /// <param name="tex">The last texture known</param>
    public void SetLastCursorTex(Texture2D tex)
    {
        lastCursorImg = tex;
    }

    /// <summary>
    /// Plays the button SFX
    /// </summary>
    public void PlayButtonSFX()
    {
        AudioManager.instance.PlaySFX(sfxMenuButton);
    }

    /**
    ===============================================================================================
    ================================ Dialogs ======================================================
    ===============================================================================================
    **/

    /// <summary>
    /// Shows a dialog (on GUI)
    /// </summary>
    /// <param name="speaker">Who speaks ?</param>
    /// <param name="dialog">What is told ?</param>
    public void ShowDialog(string speaker, string dialog)
    {
        dialogPassed = false;
        textDialog.text = dialog;
        textCharacter.text = speaker;
        textDialog.maxVisibleCharacters = 0;
        dialogRoot.SetActive(true);
        continueDialogMark.SetActive(false);

        if (routineTyping != null)
        {
            StopCoroutine(routineTyping);
        }
        routineTyping = StartCoroutine(Routine_Typing());
    }

    /// <summary>
    /// Routine for the typewritter effect
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator Routine_Typing()
    {
        string dialog = Utils.RemoveRichText(textDialog.text);

        int textLength = dialog.Length;
        int currentIndex = 0;

        while (currentIndex < textLength && !dialogSkip)
        {
            currentIndex++;
            AudioManager.instance.PlaySFX(sfxTypewritter);
            textDialog.maxVisibleCharacters = currentIndex;
            yield return new WaitForSeconds(Time.deltaTime * typingSpeed);
        }

        textDialog.maxVisibleCharacters = textLength;
        dialogSkip = false;
        continueDialogMark.SetActive(true);
        routineTyping = null;
    }

    /// <summary>
    /// Handles the click that passes the dialog
    /// </summary>
    public void Click_Dialog()
    {
        if (routineTyping == null)
        {
            dialogPassed = true;
        }
        else
        {
            dialogSkip = true;
        }

    }

    /// <summary>
    /// Hides the dialog
    /// </summary>
    public void HideDialog()
    {
        dialogPassed = false;
        dialogRoot.SetActive(false);
    }

    /// <summary>
    /// Shows a choice to the player
    /// </summary>
    /// <param name="choices">List of the possible choices</param>
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

    /// <summary>
    /// Handles the click that determines the choice made
    /// </summary>
    /// <param name="choice">Index of the chosen choice</param>
    public void Event_Choice(int choice)
    {
        DeleteChoices();
        dialogRoot.gameObject.SetActive(false);

        DialogMaster.instance.EndChoice(choice);
    }

    /// <summary>
    /// Deletes all choice buttons
    /// </summary>
    void DeleteChoices()
    {
        foreach (Transform child in choiceRoot)
        {
            Destroy(child.gameObject);
        }
    }

}
