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

        instanciatedFollowers = new List<GameObject>();
        foreach (string follower in GameManager.instance.save_currentfollowers)
        {
            instanciatedFollowers.Add(Instantiate(Resources.Load<GameObject>("Followers/" + follower), transform.position, Quaternion.identity));
        }

    }

    void Update()
    {
        if (DialogMaster.instance.inDialog)
        {
            animator.SetBool("Run", false);
            animator.SetFloat("Speed", 0);
            return;
        }

        bool running = Input.GetKey(KeyCode.LeftShift) && canRun;

        currentSpeed = running ? runSpeed : normalSpeed;



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

        if (footstepSource.isPlaying && !moving)
        {
            footstepSource.Stop();
        }
        else if (!footstepSource.isPlaying && moving)
        {
            footstepSource.Play();
        }

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
