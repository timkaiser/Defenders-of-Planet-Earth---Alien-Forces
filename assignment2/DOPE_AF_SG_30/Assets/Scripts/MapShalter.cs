using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapShalter : MonoBehaviour {

    public Button mapBtn;

    public void gotoMap()
    {
        Debug.Log("test");//SceneManager.LoadScene("GoogleMapsTest", LoadSceneMode.Single);
    }

    // Use this for initialization
    void Start()
    {
        mapBtn.onClick.AddListener(gotoMap);

    }
}
