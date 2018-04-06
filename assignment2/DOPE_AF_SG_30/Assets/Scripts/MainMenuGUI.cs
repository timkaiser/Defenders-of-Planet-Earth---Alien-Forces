using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGUI : MonoBehaviour 
{

    void OnGUI()
    {

        //Style for buttons
        GUIStyle btnStyle = new GUIStyle(GUI.skin.button);
        //custom font
        Font sa = (Font)Resources.Load("Fonts/space_age/space_age", typeof(Font));
        GUI.skin.font = sa;
        btnStyle.fontSize = 45;
        btnStyle.normal.textColor = Color.green;

        GUI.contentColor = Color.cyan;
        Color newClr = new Color (0,0,1,1.0f);
        GUI.backgroundColor = newClr;      

        //Make sure the current scene is the main menu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            // "Start game" button
            if(GUI.Button(new Rect(Screen.width /2 - Screen.width/6, Screen.height/5, Screen.width/3, Screen.height/10), "Play", btnStyle))

            {
                SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
            }

            // "About" button
			if (GUI.Button(new Rect(Screen.width / 2 - Screen.width/6, Screen.height/5 + 2*Screen.height/10, Screen.width/3, Screen.height/10), "About", btnStyle))
            {
                SceneManager.LoadScene("About", LoadSceneMode.Single);
            }

			// "Friendlist" button
			if (GUI.Button(new Rect(Screen.width / 2 - Screen.width/6, Screen.height/5 + 4*Screen.height/10, Screen.width/3, Screen.height/10), "Friendlist",btnStyle))
			{
				SceneManager.LoadScene("Friends", LoadSceneMode.Single);
			}
            
            // "Quit game" button
			if (GUI.Button(new Rect(Screen.width / 2 - Screen.width/6, Screen.height/5 + 6*Screen.height/10, Screen.width/3, Screen.height/10), "Quit",btnStyle))
            {
                
                Application.Quit();
            }
            
        }
    }
}