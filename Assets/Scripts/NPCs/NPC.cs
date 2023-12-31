using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An NPC is a character that move around the map and do things in custcenes. Followers follow the player accross maps.
/// </summary>
public class NPC : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string referenceName;
    [SerializeField] private bool followPlayer;
    [SerializeField] private float startFollowingAfterMeters;
    [SerializeField] private bool hiddenAtStart;
    [SerializeField] private bool ignoreSaveSystem;
    private Transform playerRef;
    private bool ignoreUpdate;
    private Coroutine routineEvent;
    private string _currentEvent;
    private int _currentEventIndex;

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private NPCInteractable interaction;
    [SerializeField] private StoryObject storyObj;
    [SerializeField] private FootstepSwapper footstepSwapper;

    public bool isProcessingEvent
    {
        get
        {
            return routineEvent != null;
        }
    }

    public bool arrivedAtDestination
    {
        get
        {
            return agent.remainingDistance <= 0.1f;
        }
    }

    public string currentEvent
    {
        get
        {
            return _currentEvent;
        }
    }

    public int currentEventIndex
    {
        get
        {
            return _currentEventIndex;
        }
    }

    public Vector3 position
    {
        get
        {
            return transform.position;
        }
    }

    public float rotation
    {
        get
        {
            return transform.eulerAngles.y;
        }
    }

    public bool hidden
    {
        get
        {
            return !gameObject.activeInHierarchy;
        }
    }


    void Start()
    {
        if (ignoreSaveSystem)
        {
            Init();
        }
    }

    void Awake()
    {
        ignoreUpdate = true;
    }


    /// <summary>
    /// Initialize the NPC
    /// </summary>
    public void Init()
    {
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        ignoreUpdate = false;
        playerRef = PlayerCameraManager.instance.transform;

        if (storyObj == null || storyObj.GetCanBeEnabled())
        {
            DialogMaster.instance.RegisterNPC(referenceName, this);
        }

        if ((!GameManager.instance.wasLoaded || ignoreSaveSystem) && hiddenAtStart)
        {
            SetHidden(hiddenAtStart);
        }
    }

    /// <summary>
    /// Refreshes the NPC with a SaveStoryObject
    /// </summary>
    /// <param name="save">The SaveStoryObject to sync with</param>
    public void RefreshFromSaveStoryObject(SaveStoryObject save)
    {
        SetRotation(save.objectRotation);
        Teleport(save.objectPosition);
        SetHidden(save.npcHidden);
        if (!string.IsNullOrEmpty(save.npcEventName))
        {
            StartEvent(save.npcEventName, save.currentNpcEventIndex);
        }
        return;
    }

    /// <summary>
    /// Changes the rotation of the NPC
    /// </summary>
    /// <param name="value">The new rotation</param>
    public void SetRotation(float value)
    {
        transform.eulerAngles = new Vector3(
            0,
            value,
            0
        );
    }

    /// <summary>
    /// Changes the follow status of the NPC
    /// </summary>
    /// <param name="value">Is the NPC following the player ?</param>
    public void ChangeFollowPlayer(bool value)
    {

        if (followPlayer && !value)
        {
            agent.SetDestination(transform.position);
        }

        followPlayer = value;

        if (followPlayer)
        {
            GameManager.instance.Save_AddFollower(referenceName);
        }
        else
        {
            GameManager.instance.Save_RemoveFollower(referenceName);
        }
    }

    /// <summary>
    /// Changes the destination of the NPC
    /// </summary>
    /// <param name="destination">The new destination</param>
    public void SetDestination(Vector3 destination)
    {
        animator.SetFloat("Speed", 1);
        agent.SetDestination(destination);
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
    }

    /// <summary>
    /// Sets if the NPC is hidden or not
    /// </summary>
    /// <param name="hidden">Is the NPC hidden ?</param>
    public void SetHidden(bool hidden)
    {
        gameObject.SetActive(!hidden);
    }

    /// <summary>
    /// Teleport the NPC to a given location
    /// </summary>
    /// <param name="position">The target location</param>
    public void Teleport(Vector3 position)
    {
        agent.Warp(position);
    }


    void Update()
    {
        if (ignoreUpdate) return;

        footstepSwapper.CheckLayer();

        if (followPlayer)
        {
            if (agent.isStopped && Vector3.Distance(transform.position, playerRef.position) >= startFollowingAfterMeters)
            {
                animator.SetFloat("Speed", 1);
                agent.SetDestination(playerRef.position);
                agent.isStopped = false;
            }
            else if (!agent.isStopped)
            {
                agent.SetDestination(playerRef.position);

                if (Vector3.Distance(transform.position, playerRef.position) < startFollowingAfterMeters)
                {
                    animator.SetFloat("Speed", 0);
                    agent.isStopped = true;
                }
            }
        }
        else if (!agent.isStopped && arrivedAtDestination)
        {
            agent.isStopped = true;
            animator.SetFloat("Speed", 0);
        }
    }


    /// <summary>
    /// Starts an NPC event (only applies to the NPC)
    /// </summary>
    /// <param name="fileName">The filename of the event</param>
    /// <param name="startIndex">The starting index</param>
    public void StartEvent(string fileName, int startIndex = 0)
    {
        StopEvent();
        routineEvent = DialogMaster.instance.StartCoroutine(Routine_Event(fileName, startIndex));
    }

    /// <summary>
    /// Stop the current event if it exists
    /// </summary>
    public void StopEvent()
    {
        if (routineEvent != null)
        {
            StopCoroutine(routineEvent);
        }
    }

    /// <summary>
    /// Routine for the Interpreter for the NPC event
    /// </summary>
    /// <param name="filename">The filename of the event</param>
    /// <param name="startIndex">The starting index</param>
    /// <returns>IEnumerator</returns>
    IEnumerator Routine_Event(string filename, int startIndex = 0)
    {
        _currentEvent = filename;
        yield return new WaitForEndOfFrame();

        List<string> file = FileManager.ReadTextAsset(Resources.Load<TextAsset>("NPCsEvent/" + filename));

        ignoreUpdate = true;

        string[] split;
        string line;
        string[] args;


        for (_currentEventIndex = startIndex; _currentEventIndex < file.Count; _currentEventIndex++)
        {
            line = file[_currentEventIndex];

            if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line))
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            split = line.Split('(');
            split[1] = split[1].Split(')')[0];
            switch (split[0])
            {
                case "SetFollowPlayer":
                    ChangeFollowPlayer(bool.Parse(split[1]));
                    break;
                case "SetDestination":
                    while (!agent.isOnNavMesh)
                    {
                        yield return new WaitForEndOfFrame();
                    }

                    SetDestination(DialogMaster.instance.npcWaypoints[split[1]].position);
                    yield return new WaitForEndOfFrame();
                    while (!arrivedAtDestination)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                    break;
                case "SetRotation":
                    SetRotation(float.Parse(split[1]));
                    break;
                case "ChangeInteractionDialog":
                    if (interaction != null)
                    {
                        interaction.ChangeDialogToLoad(split[1]);
                    }
                    break;
                case "SetSaveItem":
                    args = split[1].Split(";");
                    GameManager.instance.SetSaveItem(args[0], args[1]);
                    break;
                case "Hide":
                    SetHidden(true);
                    break;
                case "Show":
                    SetHidden(false);
                    break;
                case "RefreshStoryObjects":
                    StoryObjectsManager.instance.RefreshAllStoryObjects();
                    break;
            }
            yield return new WaitForEndOfFrame();

        }

        _currentEvent = "";
        _currentEventIndex = 0;
        ignoreUpdate = false;
        routineEvent = null;
    }
}
