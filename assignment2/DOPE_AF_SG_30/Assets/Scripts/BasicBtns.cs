using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BasicBtns : MonoBehaviour {

    public Button MainMenuBtn;
    public Button TwitterBtn;
   // public Button PlayBtn;

    public string APP_STORE_LINK_ANDROID = "http://www.DOPEAF.de";
    public string twitterNameParameter = "Hey! Let's play Defenders Of The Planet Earth!";
    public string twitterHashtag = "#DOPEAF_Game";
    private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
    private const string TWITTER_LANGUAGE = "en";
    public string LOCATION = "";

    public void PressedTwitterButton()
    {
        Application.OpenURL(TWITTER_ADDRESS + "?text=" + WWW.EscapeURL(twitterNameParameter + "\n"
            + Input.location.ToString()
            + twitterHashtag + "\n" + APP_STORE_LINK_ANDROID));
    }

    public void gotoMainScene()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void gotoPlaySceve()
    {
        SceneManager.LoadScene("Lobby",LoadSceneMode.Single);
    }

    // Use this for initialization
    void Start () {
        MainMenuBtn.onClick.AddListener(gotoMainScene);
       // PlayBtn.onClick.AddListener(gotoMainScene);
        TwitterBtn.onClick.AddListener(PressedTwitterButton);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
