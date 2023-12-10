using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDetectiveVision : MonoBehaviour
{
    private float currentValue = 0f;
    [SerializeField] private float speed;
    [SerializeField] private List<Material> materials;
    private bool inDetectiveMode = false;
    private bool foundRenderers = false;

    void Start()
    {
        materials = new List<Material>();
    }

    void FindRenderers()
    {
        Renderer[] renderers = (Renderer[])(Object.FindObjectsOfType(typeof(Renderer)));
        materials.Clear();

        foreach (Renderer renderer in renderers)
        {
            if (renderer.material.shader.name.Contains("DetectiveVision"))
            {
                materials.Add(renderer.material);
            }
        }
    }

    void RefreshMaterials()
    {
        foreach (Material mat in materials)
        {
            mat.SetFloat("_LerpValue", currentValue);
        }
    }

    void Update()
    {
        if (DialogMaster.instance.inDialog || GameGUI.instance.inMenu || GameGUI.instance.playingMiniGame)
        {
            if (inDetectiveMode)
            {
                inDetectiveMode = false;
                GameGUI.instance.SetDetectiveVisionActive(false);
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            inDetectiveMode = true;
            GameGUI.instance.SetDetectiveVisionActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            inDetectiveMode = false;
            GameGUI.instance.SetDetectiveVisionActive(false);
        }

        if (inDetectiveMode && currentValue < 1)
        {
            currentValue = Mathf.Clamp(currentValue + Time.deltaTime * speed, 0f, 1f);
            RefreshMaterials();
        }
        else if (!inDetectiveMode && currentValue > 0)
        {
            currentValue = Mathf.Clamp(currentValue - Time.deltaTime * speed, 0f, 1f);
            RefreshMaterials();
        }
    }


    void LateUpdate()
    {
        if (!foundRenderers)
        {
            foundRenderers = true;
            FindRenderers();
        }
    }
}
