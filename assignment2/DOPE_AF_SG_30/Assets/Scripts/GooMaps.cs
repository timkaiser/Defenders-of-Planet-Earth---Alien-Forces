using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GooMaps : MonoBehaviour {

    string url = "";
    
    public float latT = 48.262450f;
    public float lonT = 11.669223f;
    public float latM = 48.262460f;
    public float lonM = 11.669223f;
    public int zoom = 17;
    public int mapWdh;
    public int mapHgth;
    public int scale=2;

    string urlEXP = "";

    string key = "&key=AIzaSyCMohW1ZW7RE4IFT_741Yar-Xt4okDP3UM"; //put your own API key here.

    IEnumerator Start()

    {
        mapWdh = Screen.width;
        mapHgth = Screen.height;
        urlEXP = "http://maps.googleapis.com/maps/api/staticmap?center="+latT+","+lonT+"," +
                 "&zoom=" + zoom + "&size=" + mapWdh + "x" + mapHgth + "&maptype=roadmap&markers=color:blue%7Clabel:T%7C"
                 +latT+","+lonT+"&markers=color:green%7Clabel:M%7C" + latM + "," + lonM +
                 "&sensor=false";
        WWW www = new WWW(urlEXP + key);
        yield return www;
        gameObject.GetComponent<RawImage>().texture = www.texture;
    }



    int count = 0;
    private void Update()
    {
        if (count >= 50)
        {
            latT = LocationProvider.latitude;
            lonT = LocationProvider.longitude;
            StartCoroutine(Start());
        }
    }

}
