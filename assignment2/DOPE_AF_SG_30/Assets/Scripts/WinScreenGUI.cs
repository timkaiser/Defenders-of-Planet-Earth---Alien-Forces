using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreenGUI : MonoBehaviour {

    public Button back;
	// Use this for initialization
	void Start () {
		back.onClick.AddListener(backToMenu);
    }
    
    public void backToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
