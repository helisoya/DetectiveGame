using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Handles player movements
/// </summary>
public class PlayerMovements : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float runSpeed;
    private float currentSpeed;
    private bool canRun = true;


    [Header("Components")]
    private Transform cam;
    [SerializeField] private Transform body;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform startingPointsRoot;
    [SerializeField] private Transform root;
    private PlayerCameraManager camManager;
    private List<GameObject> instanciatedFollowers;

    [Header("Sounds")]
    [SerializeField] private FootstepSwapper footstepSwapper;
    [SerializeField] private AudioSource footstepSource;

    public static PlayerMovements instance;

    public Vector3 position
    {
        get
        {
            return body.position;
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        cam = Camera.main.transform;
        camManager = GetComponent<PlayerCameraManager>();
        FindStartingPoint();
    }

    /// <summary>
    /// Find the correct starting point and teleports the player to it. 
    /// Otherwise, it teleports them to the DEFAULT starting point.
    /// </summary>
    void FindStartingPoint()
    {

        if (GameManager.instance.wasLoaded)
        {
            root.position = GameManager.instance.save_playerPosition;
            InstantiateFollowers();
            return;
        }

        Transform defaultStart = null;
        Transform chosen = null;
        string mapName = GameManager.instance.save_lastMap;



        foreach (Transform child in startingPointsRoot)
        {
            if (child.name == "DEFAULT")
            {
                defaultStart = child;
            }
            else if (child.name.Equals(mapName))
            {
                chosen = child;
                break;
            }
        }

        if (chosen == null && defaultStart != null)
        {
            chosen = defaultStart;
        }

        if (chosen != null)
        {
            root.position = chosen.position;
            root.eulerAngles = chosen.eulerAngles;
        }

        InstantiateFollowers();
    }

    /// <summary>
    /// Instantiates the followers to the player's position
    /// </summary>
    void InstantiateFollowers()
    {
        instanciatedFollowers = new List<GameObject>();
        foreach (string follower in GameManager.instance.save_currentfollowers)
        {
            instanciatedFollowers.Add(Instantiate(Resources.Load<GameObject>("Followers/" + follower), transform.position, Quaternion.identity));
        }
    }

    /// <summary>
    /// Regulate if the footsteps sound effect should be played
    /// </summary>
    /// <param name="active">Is the founstep SFX active ?</param>
    void SetFootstepSoundActive(bool active)
    {
        if (active && !footstepSource.isPlaying)
        {
            footstepSource.Play();
        }
        else if (!active && footstepSource.isPlaying)
        {
            footstepSource.Stop();
        }
    }

    void Update()
    {
        if (DialogMaster.instance.inDialog || GameGUI.instance.inMenu)
        {
            animator.SetBool("Run", false);
            animator.SetFloat("Speed", 0);
            SetFootstepSoundActive(false);
            return;
        }

        bool running = Input.GetKey(KeyCode.LeftShift) && canRun;

        currentSpeed = running ? runSpeed : normalSpeed;
        footstepSource.pitch = running ? 1.2f : 1;


        Vector3 moveDirection = cam.forward * Input.GetAxis("Vertical");
        moveDirection += cam.right * Input.GetAxis("Horizontal");
        moveDirection.Normalize();
        moveDirection *= currentSpeed;
        moveDirection.y = rb.velocity.y;

        rb.velocity = moveDirection;

        bool moving = moveDirection.x != 0 || moveDirection.z != 0;
        animator.SetBool("Run", running);
        animator.SetFloat("Speed", moving ? 2 : 0);

        footstepSwapper.CheckLayer();
        SetFootstepSoundActive(moving);

        if (!camManager.isInFirstPerson && moving)
        {
            body.forward = moveDirection;
        }

    }

    void LateUpdate()
    {
        body.eulerAngles = new Vector3(0, body.eulerAngles.y, 0);
    }

    /// <summary>
    /// Teleports the player to a given location
    /// </summary>
    /// <param name="position">The target location</param>
    public void SetPosition(Vector3 position)
    {
        body.position = position;
    }

}
