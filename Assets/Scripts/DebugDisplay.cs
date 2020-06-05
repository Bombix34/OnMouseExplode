using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplay : Singleton<DebugDisplay>
{
    float deltaTime = 0.0f;

    public bool IsShowingDebug { get; set; }

    private string m_StringToShow="";
    int debugLine = 0;

    private void Awake()
    {
        IsShowingDebug = false;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    public void Log(string text)
    {
        debugLine++;
        if (debugLine % 15 == 0)
        {
            m_StringToShow = "-" + text;
        }
        else if (debugLine % 3 == 0)
            m_StringToShow += "-" + text + "\r\n";
        else
            m_StringToShow += "-" + text;
    }

    private void OnGUI()
    {
        if (!IsShowingDebug)
        {
            return;
        }
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        GUIStyle objectsInScene = new GUIStyle();
        Rect rectObj = new Rect(0, ( 8*h) / 9, w, h * 2 / 100);
        objectsInScene.alignment = TextAnchor.UpperLeft;
        objectsInScene.fontSize = h * 2 / 100;
        objectsInScene.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        string objText = m_StringToShow;
        GUI.Label(rectObj, objText, objectsInScene);

        Rect rect = new Rect((2*w)/6, h/ 35, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 100;
        style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}