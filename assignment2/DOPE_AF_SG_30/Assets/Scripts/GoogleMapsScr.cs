using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMapsScr : MonoBehaviour {

    string url = "";
    public float lat= 0;
    public float lon=0;
    LocationInfo li;
    public int zoom = 14;
    public int mapWdh = 50;
    public int mapHgth = 50;
    public int scale;

    private bool loadingMap = false;

    private IEnumerator mapCoroutine;
    /*IEnumerator GetGoogleMapScr(float lat, float lon)
    {
        url = "http://maps.googleapis.com/maps/api/staticmap?centers" + lat + "." + lon + "&zoom=" + zoom + "&size" + mapWdh + "x" + mapHgth + "&scale=" + scale
            + "&mapType=roadmap&key";

        loadingMap = true;

    }*/
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
