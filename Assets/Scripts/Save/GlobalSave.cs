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

    public GlobalSave(int width, int height, int refreshR, bool fullScr)
    {
        resolutionW = width;
        resolutionH = height;
        refreshRate = refreshR;
        fullscreen = fullScr;

        volumeMaster = 0;
        volumeBGM = 0;
        volumeSFX = 0;
    }

}
