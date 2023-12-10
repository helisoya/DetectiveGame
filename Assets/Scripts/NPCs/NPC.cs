using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private string referenceName;
    [SerializeField] private bool followPlayer;
    [SerializeField] private float startFollowingAfterMeters;
    [SerializeField] private bool hiddenAtStart;
    private Transform playerRef;
    private bool ignoreUpdate;
    private Coroutine routineEvent;

    [Header("Components")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Animator animator;
    [SerializeField] private NPCInteractable interaction;
    [SerializeField] private StoryObject storyObj;

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

    void Start()
    {
        agent.isStopped = true;
        ignoreUpdate = false;
        playerRef = PlayerCameraManager.instance.transform;

        if (storyObj == null || storyObj.GetCanBeEnabled())
        {
            DialogMaster.instance.RegisterNPC(referenceName, this);
        }
        SetHidden(hiddenAtStart);
    }

    public void SetRotation(float value)
    {
        transform.eulerAngles = new Vector3(
            0,
            value,
            0
        );
    }


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

    public void SetDestination(Vector3 destination)
    {
        animator.SetFloat("Speed", 1);
        agent.SetDestination(destination);
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }
    }

    public void SetHidden(bool hidden)
    {
        gameObject.SetActive(!hidden);
    }

    public void Teleport(Vector3 position)
    {
        agent.Warp(position);
    }


    void Update()
    {
        if (ignoreUpdate) return;

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



    public void StartEvent(string fileName)
    {
        if (routineEvent != null)
        {
            StopCoroutine(routineEvent);
        }
        routineEvent = DialogMaster.instance.StartCoroutine(Routine_Event(fileName));
    }

    IEnumerator Routine_Event(string filename)
    {
        List<string> file = FileManager.ReadTextAsset(Resources.Load<TextAsset>("NPCsEvent/" + filename));

        ignoreUpdate = true;

        string[] split;
        string line;
        string[] args;


        for (int i = 0; i < file.Count; i++)
        {
            line = file[i];

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


        ignoreUpdate = false;
        routineEvent = null;
    }
}
