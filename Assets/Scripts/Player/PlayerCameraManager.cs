using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Handles the Player Camera
/// </summary>
public class PlayerCameraManager : MonoBehaviour
{

    [Header("General")]
    [SerializeField] private int disabledCamValue;
    [SerializeField] private int enabledCamValue;
    public bool isInFirstPerson = true;
    private string _inputAxisNameX;
    private string _inputAxisNameY;


    [Header("FPS Cam")]
    [SerializeField] private CinemachineVirtualCamera firstPersonCam;
    [SerializeField] private Transform fpsCamLookAt;
    [SerializeField] private float sensibility;
    [SerializeField] private float maxXRot;
    [SerializeField] private LayerMask fpsLayers;
    private float xrot = 0;



    [Header("TPS Cam")]
    [SerializeField] private CinemachineFreeLook thridPersonCam;
    [SerializeField] private LayerMask tpsLayers;

    private Camera[] overlayCams;


    public static PlayerCameraManager instance;


    public float rotation
    {
        get
        {
            return transform.eulerAngles.y;
        }
    }

    void Awake()
    {
        instance = this;

        RefreshPriorities();

        _inputAxisNameX = thridPersonCam.m_XAxis.m_InputAxisName;
        _inputAxisNameY = thridPersonCam.m_YAxis.m_InputAxisName;
    }

    void Start()
    {
        if (GameManager.instance.wasLoaded)
        {
            Rotate(GameManager.instance.save_playerRotation);
            return;
        }

        overlayCams = Camera.main.GetComponentsInChildren<Camera>();
    }

    /// <summary>
    /// Refreshes the clipping planes and FOV of overlay cameras
    /// </summary>
    void RefreshOverlayCameras()
    {
        if (overlayCams == null || (overlayCams.Length != 0 && !overlayCams[0])) return;

        float fov = Camera.main.fieldOfView;
        float clipNear = Camera.main.nearClipPlane;
        float clipFar = Camera.main.farClipPlane;

        foreach (Camera cam in overlayCams)
        {
            cam.fieldOfView = fov;
            cam.nearClipPlane = clipNear;
            cam.farClipPlane = clipFar;
        }
    }

    /// <summary>
    /// Refreshes the priorities of the cameras and the cursor state
    /// </summary>
    void RefreshPriorities()
    {
        Camera.main.cullingMask = isInFirstPerson ? fpsLayers : tpsLayers;
        Cursor.lockState = isInFirstPerson ? CursorLockMode.Locked : CursorLockMode.Confined;
        firstPersonCam.Priority = isInFirstPerson ? enabledCamValue : disabledCamValue;
        thridPersonCam.Priority = !isInFirstPerson ? enabledCamValue : disabledCamValue;



    }

    void Update()
    {
        if (DialogMaster.instance.inDialog || GameGUI.instance.inMenu || GameGUI.instance.playingMiniGame) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            isInFirstPerson = !isInFirstPerson;
            RefreshPriorities();
            RefreshOverlayCameras();
            return;
        }

        if (!isInFirstPerson)
        {
            bool rightClick = Input.GetMouseButton(1);
            thridPersonCam.m_XAxis.m_InputAxisName = rightClick ? _inputAxisNameX : "";
            thridPersonCam.m_YAxis.m_InputAxisName = rightClick ? _inputAxisNameY : "";
            thridPersonCam.m_XAxis.m_InputAxisValue *= rightClick ? 1 : 0;
            thridPersonCam.m_YAxis.m_InputAxisValue *= rightClick ? 1 : 0;
        }
        else
        {
            Vector2 PlayerCamMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            transform.Rotate(0f, PlayerCamMovement.x * sensibility * Time.timeScale, 0f);
            xrot = Mathf.Clamp(xrot - PlayerCamMovement.y * sensibility * Time.timeScale, -maxXRot, maxXRot);
            fpsCamLookAt.transform.localRotation = Quaternion.Euler(xrot, 0f, 0f);
        }

        RefreshOverlayCameras();
    }

    /// <summary>
    /// Turns the player toward a transform
    /// </summary>
    /// <param name="obj">The target transform</param>
    public void TurnToward(Transform obj)
    {
        transform.LookAt(obj);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }


    /// <summary>
    /// Sets the rotation of the player
    /// </summary>
    /// <param name="angle">The target angle</param>
    public void Rotate(float angle)
    {
        transform.eulerAngles = new Vector3(0f, angle, 0f);
    }
}
