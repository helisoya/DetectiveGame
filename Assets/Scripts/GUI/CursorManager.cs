using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Cursor details modification
/// </summary>
public class CursorManager
{
    public static Texture2D currentTex = null;

    /// <summary>
    /// Changes the current cursor texture
    /// </summary>
    /// <param name="tex">The new texture</param>
    public static void ChangeCursorTex(Texture2D tex)
    {
        currentTex = tex;
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }
}
