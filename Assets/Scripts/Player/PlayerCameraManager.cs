using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

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


    public static PlayerCameraManager instance;

    void Awake()
    {
        instance = this;

        RefreshPriorities();

        _inputAxisNameX = thridPersonCam.m_XAxis.m_InputAxisName;
        _inputAxisNameY = thridPersonCam.m_YAxis.m_InputAxisName;
    }

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
    }

    public void TurnToward(Transform obj)
    {
        transform.LookAt(obj);
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
    }

    public void Rotate(float angle)
    {
        transform.eulerAngles = new Vector3(0f, angle, 0f);
    }
}
