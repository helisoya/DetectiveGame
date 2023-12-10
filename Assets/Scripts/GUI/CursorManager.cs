using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager
{
    public static Texture2D currentTex = null;
    public static void ChangeCursorTex(Texture2D tex)
    {
        currentTex = tex;
        Cursor.SetCursor(tex, Vector2.zero, CursorMode.Auto);
    }
}
