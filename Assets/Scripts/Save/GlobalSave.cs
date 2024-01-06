using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalSave
{
    public float volumeMaster;
    public float volumeBGM;
    public float volumeSFX;
    public bool fullscreen;
    public int resolutionW;
    public int resolutionH;
    public int refreshRate;
    public int quality;
    public float typewritterSpeed;

    /// <summary>
    /// Creates a new Global Save
    /// </summary>
    /// <param name="width">Current screen width</param>
    /// <param name="height">Current screen height</param>
    /// <param name="refreshR">Current refresh rate</param>
    /// <param name="fullScr">Is in fullscreen ?</param>
    /// <param name="qual">Current quality Setting's index</param>
    public GlobalSave(int width, int height, int refreshR, bool fullScr, int qual)
    {
        resolutionW = width;
        resolutionH = height;
        refreshRate = refreshR;
        fullscreen = fullScr;
        quality = qual;

        volumeMaster = 0;
        volumeBGM = 0;
        volumeSFX = 0;
        typewritterSpeed = 3;
    }

}
