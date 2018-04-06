using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gameplay_GUI : MonoBehaviour 
{
	public Texture2D Pfeil;
	public Texture2D Pfeil2;
	public Texture2D rocket_icon;
	public Texture2D laser_icon;
	public Texture2D heal_icon;
    private FireControl fireControl;
	public Texture2D mapIcon;
    private GameManager gm;

	public Texture2D greenHealth;
	public Texture2D redHealth;

    public RawImage map;

	private bool MenuActive = false;
    private bool inGame = true;

    void Start()
    {
        map.enabled = false;

        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        GameObject localPlayer = gm.getLocalPlayer();
        if (localPlayer == null)
        {
            StartCoroutine(GetLocalPlayer());
            inGame = false;
        }
        else
            fireControl = localPlayer.GetComponent<FireControl>();
    }

    IEnumerator GetLocalPlayer()
    {
        while(fireControl == null)
        {
            GameObject localPlayer = gm.getLocalPlayer();
            if(localPlayer != null)
                fireControl = localPlayer.GetComponent<FireControl>();
            yield return new WaitForEndOfFrame();
        }
        inGame = true;
    }

	void OnGUI()
	{
        if (!inGame)
            return;

        //Map
        if (GUI.Button(new Rect(Screen.width-(Screen.width/16), Screen.height - (Screen.width / 16), Screen.width / 16, (Screen.width / 16)), mapIcon))
        {
            map.enabled = !map.enabled;
        }


        if (!MenuActive) 
		{
			if (GUI.Button (new Rect (10, Screen.height / 2 - Screen.height/12, Screen.width/24, Screen.height/6), Pfeil))
			{
				MenuActive = true;
			}
		}
		else if (MenuActive) 
		{
			if (GUI.Button(new Rect(10, Screen.height / 2 - 3*Screen.height/12, Screen.width/12, Screen.width/12), rocket_icon)) 
			{
                fireControl.CmdSetWeaponType(FireControl.WeaponType.Rocket);
            }
			if (GUI.Button(new Rect (10,Screen.height/2 - Screen.height/12, Screen.width/12, Screen.width/12), laser_icon))
            {
                fireControl.CmdSetWeaponType(FireControl.WeaponType.Laser);
            }
			if (GUI.Button (new Rect (10,Screen.height/2 + Screen.height/12, Screen.width/12, Screen.width/12), heal_icon))
            {
                fireControl.CmdSetWeaponType(FireControl.WeaponType.Dart);
            }

			if (GUI.Button (new Rect (Screen.width/12+30, Screen.height / 2 - Screen.height/12, Screen.width/24, Screen.height/6), Pfeil2))
			{
				MenuActive = false;
			}
		}

		GameObject[] gob = GameObject.FindGameObjectsWithTag("Enemy");

		for (int i = 0,b = 0; i < gob.Length; i++) 
		{
			Enemy_Health eh = gob[i].gameObject.GetComponent<Enemy_Health>();

			if (eh != null) 
			{
				Debug.Log("getHealth(): "+eh.getHealth());

				for (int j = 0; j < Enemy_Health.MAXHEALTH; j++) 
				{
					if (j < eh.getHealth())
						GUI.Label (new Rect (Screen.width / 2 + Screen.width / 4 + j * 10, b * 25 + 50, 10, 25), greenHealth);
					else
						GUI.Label (new Rect (Screen.width / 2 + Screen.width / 4 + j * 10, b * 25 + 50, 10, 25), redHealth);
				}
				b++;
			}
		}
	}
}
