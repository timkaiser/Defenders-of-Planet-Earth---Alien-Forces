using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour {

    public static GlobalVariables instance = null;

    public static string username = "debug";

    //Source(Awake): https://unity3d.com/de/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;
        
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        
    }




	

}
