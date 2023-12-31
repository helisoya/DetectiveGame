using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


/// <summary>
/// Handles Dialogs (Cutscenes technicaly)
/// </summary>
public class DialogMaster : MonoBehaviour
{
    public static DialogMaster instance;
    private Dictionary<string, FocusableObject> focusableObjects;
    private Dictionary<string, Animator> faceAnimators;
    private Dictionary<string, Animator> animations;
    private Dictionary<string, NPC> npcs;
    public Dictionary<string, Transform> npcWaypoints;
    [SerializeField] private CinemachineVirtualCamera cutsceneCam;
    [HideInInspector] public bool inDialog = false;
    private Coroutine dialogRoutine;
    private CursorLockMode lastLockMode;
    private bool startedDialog;
    private int lastMask;

    [SerializeField] private string dialogToStartOnLaunch;

    private List<string[]> currentChoices;

    /// <summary>
    /// Register a NPC to the dialog system
    /// </summary>
    /// <param name="name">Reference name</param>
    /// <param name="value">The NPC</param>
    public void RegisterNPC(string name, NPC value)
    {
        npcs.Add(name, value);
    }

    /// <summary>
    /// Register Focusable Objects to the dialog system
    /// </summary>
    /// <param name="name">Reference name</param>
    /// <param name="value">The Focusable Object</param>
    public void RegisterFocusable(string name, FocusableObject value)
    {
        focusableObjects.Add(name, value);
    }

    /// <summary>
    /// Register Waypoints to the dialog system
    /// </summary>
    /// <param name="name">Reference name</param>
    /// <param name="value">The Waypoint</param>
    public void RegisterNPCWaypoints(string name, Transform value)
    {
        npcWaypoints.Add(name, value);
    }

    /// <summary>
    /// Register Animators to the dialog system
    /// </summary>
    /// <param name="name">Reference name</param>
    /// <param name="value">The Animator</param>
    public void RegisterAnimation(string name, Animator value)
    {
        animations.Add(name, value);
    }

    /// <summary>
    /// Register Facial Animators to the dialog system
    /// </summary>
    /// <param name="name">Reference name</param>
    /// <param name="value">The Animator</param>
    public void RegisterFaceAnimation(string name, Animator value)
    {
        faceAnimators.Add(name, value);
    }

    void Awake()
    {
        startedDialog = false;
        instance = this;
        focusableObjects = new Dictionary<string, FocusableObject>();
        animations = new Dictionary<string, Animator>();
        faceAnimators = new Dictionary<string, Animator>();
        npcs = new Dictionary<string, NPC>();
        npcWaypoints = new Dictionary<string, Transform>();
    }

    void Start()
    {
        if (!string.IsNullOrEmpty(dialogToStartOnLaunch))
        {
            StartCoroutine(Routine_StartingDialog());
        }

    }

    /// <summary>
    /// Starts starting dialog after a delay of 2 frames 
    /// </summary>
    /// <returns></returns>
    IEnumerator Routine_StartingDialog()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForEndOfFrame();
        }

        StartDialog(dialogToStartOnLaunch);
    }

    /// <summary>
    /// Ends a choice segment
    /// </summary>
    /// <param name="indexChoice">The choice index</param>
    public void EndChoice(int indexChoice)
    {
        string choice = currentChoices[indexChoice][1];


        if (choice.Equals("EXIT"))
        {
            Camera.main.cullingMask = lastMask;
            startedDialog = false;
            Cursor.lockState = lastLockMode;
            cutsceneCam.Priority = 10;
            inDialog = false;
        }
        else
        {
            StartDialog(choice);
        }
    }


    /// <summary>
    /// Starts a dialog
    /// </summary>
    /// <param name="filePath">Filename of the dialog</param>
    public void StartDialog(string filePath)
    {
        if (dialogRoutine != null)
        {
            StopCoroutine(dialogRoutine);
        }

        dialogRoutine = StartCoroutine(Routine_Dialog(filePath));
    }


    /// <summary>
    /// Interpreter for the dialog system
    /// </summary>
    /// <param name="filePath">The filename of the dialog</param>
    /// <returns>IEnumerator</returns>
    IEnumerator Routine_Dialog(string filePath)
    {
        List<string> file = FileManager.ReadTextAsset(Resources.Load<TextAsset>("Dialogs/" + filePath));

        bool isLastFile = true;

        if (!startedDialog)
        {
            startedDialog = true;
            lastMask = Camera.main.cullingMask;
            lastLockMode = Cursor.lockState;
            Cursor.lockState = CursorLockMode.Confined;
            CursorManager.ChangeCursorTex(null);
        }

        inDialog = true;
        string[] split;
        string[] args;
        string line;


        for (int i = 0; i < file.Count; i++)
        {
            if (!isLastFile) break;

            line = file[i];

            if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }




            if (line.Equals("Choice")) // Handling Choice
            {
                currentChoices = new List<string[]>();
                int y = i + 1;
                while (y < file.Count)
                {
                    currentChoices.Add(file[y].Split(";"));
                    y++;
                }

                GameGUI.instance.ShowChoice(currentChoices);

                yield break;
            }
            else // Not handling a choice
            {
                split = line.Split('(');

                if (split[0].Equals("If")) // Handling If
                {
                    split = split[1].Split(")");
                    args = split[0].Split(";");

                    string keyValue = GameManager.instance.GetSaveItemValue(args[0]);

                    bool ok = false;
                    switch (args[1])
                    {
                        case "=":
                            if (args[2].Equals(keyValue))
                            {
                                ok = true;
                            }
                            break;
                        case "<":
                            if (int.Parse(keyValue) < int.Parse(args[2]))
                            {
                                ok = true;
                            }
                            break;
                        case ">":
                            if (int.Parse(keyValue) < int.Parse(args[2]))
                            {
                                ok = true;
                            }
                            break;
                    }

                    if (ok)
                    {
                        isLastFile = false;
                        StartDialog(split[1]);
                    }
                }
                else // Handling normal line
                {
                    split[1] = split[1].Split(')')[0];
                    switch (split[0])
                    {
                        case "Dialog":
                            args = split[1].Split(';');
                            // 0 Speaker, 1 Dialog, 2 FaceAnimation (Optional), 3 HideAfterward 

                            GameGUI.instance.ShowDialog(args[0], args[1]);

                            bool useSpeak = false;
                            if (faceAnimators.ContainsKey(args[2]))
                            {
                                useSpeak = true;
                                faceAnimators[args[2]].SetBool("Speak", true);
                            }

                            while (!GameGUI.instance.dialogPassed)
                            {
                                if (useSpeak && GameGUI.instance.typewritterFinished)
                                {
                                    faceAnimators[args[2]].SetBool("Speak", false);
                                }
                                yield return new WaitForEndOfFrame();
                            }

                            /*
                            if (useSpeak)
                            {
                                faceAnimators[args[2]].SetBool("Speak", false);
                            }
                            */

                            if (args[3].Equals("true"))
                            {
                                GameGUI.instance.HideDialog();
                            }

                            break;

                        case "PositionPlayer":
                            PlayerMovements.instance.SetPosition(npcWaypoints[split[1]].position);
                            break;

                        case "RotationPlayer":
                            PlayerCameraManager.instance.Rotate(float.Parse(split[1]));
                            break;

                        case "FocusCamera":
                            cutsceneCam.LookAt = focusableObjects[split[1]].focusOn;
                            break;

                        case "PositionCamera":
                            Camera.main.cullingMask = -1;
                            cutsceneCam.Priority = 100;
                            cutsceneCam.transform.position = focusableObjects[split[1]].focusOn.position;
                            break;

                        case "SetCameraInFrontOf":
                            Camera.main.cullingMask = -1;
                            cutsceneCam.Priority = 100;

                            Transform from = focusableObjects[split[1]].focusOn;
                            cutsceneCam.transform.position = from.position + from.forward * 3;
                            break;

                        case "SetCameraBetween":
                            args = split[1].Split(';');
                            Camera.main.cullingMask = -1;
                            cutsceneCam.Priority = 100;

                            Vector3 middlePos = Vector3.Lerp(focusableObjects[args[0]].focusOn.position, focusableObjects[args[1]].focusOn.position, 0.5f);
                            cutsceneCam.transform.position = middlePos;

                            focusableObjects[args[0]].TurnToward(focusableObjects[args[1]].focusOn);
                            focusableObjects[args[1]].TurnToward(focusableObjects[args[0]].focusOn);
                            break;

                        case "LoadFile":
                            isLastFile = false;
                            StartDialog(split[1]);
                            break;
                        case "UnlockBio":
                            GameManager.instance.Save_UnlockNewBio(split[1]);
                            break;
                        case "UnlockEvidence":
                            GameManager.instance.Save_AddEvidence(split[1]);
                            break;
                        case "StartCase":
                            GameManager.instance.Save_StartNewCase(split[1]);
                            break;
                        case "EndCase":
                            GameManager.instance.Save_EndCase(split[1]);
                            break;
                        case "ChangeWeather":
                            SkyManager.instance.ChangeSkybox(System.Enum.Parse<SkyManager.SkyType>(split[1]));
                            break;
                        case "StartLockPickMiniGame":
                            GameGUI.instance.StartLockPickMiniGame(split[1]);
                            break;
                        case "SetNPCFollowPlayer":
                            args = split[1].Split(';');
                            npcs[args[0]].ChangeFollowPlayer(bool.Parse(args[1]));
                            break;
                        case "SetNPCRotation":
                            args = split[1].Split(';');
                            npcs[args[0]].SetRotation(float.Parse(args[1]));
                            break;
                        case "SetNPCDestination":
                            args = split[1].Split(';');
                            npcs[args[0]].SetDestination(npcWaypoints[args[1]].position);
                            yield return new WaitForEndOfFrame();
                            while (!npcs[args[0]].arrivedAtDestination)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            break;
                        case "TeleportNPC":
                            args = split[1].Split(';');
                            npcs[args[0]].Teleport(npcWaypoints[args[1]].position);
                            break;
                        case "TurnNPCToward":
                            args = split[1].Split(';');
                            focusableObjects[args[0]].TurnToward(focusableObjects[args[1]].focusOn);
                            break;
                        case "SetNPCHidden":
                            args = split[1].Split(';');
                            npcs[args[0]].SetHidden(bool.Parse(args[1]));
                            break;
                        case "StartNPCEvent":
                            args = split[1].Split(';');
                            npcs[args[0]].StartEvent(args[1]);
                            break;
                        case "SetSaveItem":
                            args = split[1].Split(';');
                            GameManager.instance.SetSaveItem(args[0], args[1]);
                            break;
                        case "FadeBg":
                            args = split[1].Split(';');
                            GameGUI.instance.FadeBackgroundTo(float.Parse(args[0]), float.Parse(args[1]));
                            while (GameGUI.instance.isFadingBackground)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            break;
                        case "FadeFg":
                            args = split[1].Split(';');
                            GameGUI.instance.FadeForegroundTo(float.Parse(args[0]), float.Parse(args[1]));
                            while (GameGUI.instance.isFadingForeground)
                            {
                                yield return new WaitForEndOfFrame();
                            }
                            break;
                        case "ShowCG":
                            args = split[1].Split(';');
                            GameGUI.instance.ShowCG(
                                Resources.Load<Sprite>("CG/" + args[0]),
                                float.Parse(args[1])
                            );
                            break;
                        case "HideCG":
                            GameGUI.instance.HideCG(float.Parse(split[1]));
                            break;
                        case "RefreshStoryObjects":
                            StoryObjectsManager.instance.RefreshAllStoryObjects();
                            break;
                        case "LoadMap":
                            GameManager.instance.LoadMap(split[1]);
                            break;
                        case "Wait":
                            yield return new WaitForSeconds(float.Parse(split[1]));
                            break;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }

        if (isLastFile)
        {
            inDialog = false;
            startedDialog = false;
            Camera.main.cullingMask = lastMask;
            Cursor.lockState = lastLockMode;
            cutsceneCam.Priority = 10;
        }

    }


}
