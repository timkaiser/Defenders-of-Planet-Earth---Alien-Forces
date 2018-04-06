using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlyaBtnSc : MonoBehaviour {

    public Button playBtn;

    public void gotoLobby()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

	// Use this for initialization
	void Start () {
        playBtn.onClick.AddListener(gotoLobby);

    }
	
}
