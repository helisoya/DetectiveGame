using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles all events on the main menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject mainRoot;
    [SerializeField] private GameObject newGameOverwriteRoot;
    [SerializeField] private Button continueButton;

    [Header("Options")]
    [SerializeField] private OptionsMenuTab optionsMenuTab;

    [Header("Fading")]
    [SerializeField] private FadeManager fade;

    [Header("Audio")]
    [SerializeField] private AudioClip buttonPressClick;

    public static MainMenuManager instance;

    public bool isFading
    {
        get
        {
            return fade.isFading;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        continueButton.interactable = GameManager.instance.saveFileExists;
    }


    /// <summary>
    /// Plays the button click sound effect
    /// </summary>
    public void PlayButtonSFX()
    {
        AudioManager.instance.PlaySFX(buttonPressClick);
    }

    /// <summary>
    /// Fades the screen
    /// </summary>
    /// <param name="alpha">Fade to (0 = transparent, 1 = opaque)</param>
    /// <param name="speed">Fade speed</param>
    public void FadeTo(float alpha, float speed)
    {
        fade.FadeTo(alpha, speed);
    }

    /// <summary>
    /// Handles the click to start a new game
    /// </summary>
    public void Click_NewGame()
    {
        PlayButtonSFX();
        mainRoot.SetActive(false);
        if (GameManager.instance.saveFileExists)
        {
            newGameOverwriteRoot.SetActive(true);
        }
        else
        {
            Click_ConfirmNewGame();
        }
    }

    /// <summary>
    /// Handles the click to confirm the overwrite of the existing save file
    /// </summary>
    public void Click_ConfirmNewGame()
    {
        PlayButtonSFX();
        newGameOverwriteRoot.SetActive(false);
        GameManager.instance.NewGame();
    }

    /// <summary>
    /// Handles the click to cancel the ovewrite of the existing save file
    /// </summary>
    public void Click_CancelNewGame()
    {
        PlayButtonSFX();
        newGameOverwriteRoot.SetActive(false);
        mainRoot.SetActive(true);
    }

    /// <summary>
    /// Handles the click to load the game
    /// </summary>
    public void Click_Load()
    {
        PlayButtonSFX();
        mainRoot.SetActive(false);
        GameManager.instance.Load(true);
    }

    /// <summary>
    /// Handles the click to open the options
    /// </summary>
    public void Click_Options()
    {
        PlayButtonSFX();
        mainRoot.SetActive(false);
        optionsMenuTab.Open();
    }

    /// <summary>
    /// Handles the click to close the options
    /// </summary>
    public void Click_CloseOptions()
    {
        PlayButtonSFX();
        optionsMenuTab.Close();
        mainRoot.SetActive(true);
    }

    /// <summary>
    /// Handles the click to exit the game
    /// </summary>
    public void Click_Exit()
    {
        Application.Quit();
    }
}
