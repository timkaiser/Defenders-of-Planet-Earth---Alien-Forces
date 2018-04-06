using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairColourChanger : MonoBehaviour {

    PlayerNetworkManager netMan;

    public Image img;
    public Color enemyColour;
    public Color friendColour;
    Color targetColour = Color.white;

    void Start()
    {
        netMan = transform.root.GetComponent<PlayerNetworkManager>();
        if (!netMan.isLocalPlayer)
        {
            Destroy(this);
            return;
        }
        if (img == null)
            img = GameObject.Find("Crosshair").GetComponent<Image>();
    }

	void Update () {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform.gameObject.tag == "Enemy")
                targetColour = enemyColour;
            else if (hit.transform.gameObject.tag == "Player")
                targetColour = friendColour;
            else
                targetColour = Color.white;
        }
        else
            targetColour = Color.white;
        img.color = Color.Lerp(img.color, targetColour, Time.deltaTime * 10);
    }
}
