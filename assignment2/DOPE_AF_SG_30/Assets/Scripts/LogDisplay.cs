using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogDisplay : MonoBehaviour {

    static string displayText = "";
    GUIStyle guiStyle = new GUIStyle();

    public static void setText(string s)
    {
        displayText = s;
    }

    public static void addText(string s)
    {
        displayText += s;
    }

    void Start()
    {
        guiStyle.fontSize = Screen.height / 18;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, Screen.width / 2, Screen.height / 6), displayText, guiStyle);
    }
}
