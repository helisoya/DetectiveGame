using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Special Fade class that can also change the sprite of the fade image. (Used for CGs)
/// </summary>
public class FadeImage : FadeManager
{

    /// <summary>
    /// Changes the sprite fo the fade image
    /// </summary>
    /// <param name="sprite">The new sprite</param>
    public void SetImage(Sprite sprite)
    {
        fadeImg.sprite = sprite;
    }
}
