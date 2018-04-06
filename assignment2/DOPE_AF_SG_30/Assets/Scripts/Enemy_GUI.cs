using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_GUI : NetworkBehaviour 
{
	public Texture2D greenHealth;
	public Texture2D redHealth;
	public Texture2D backgroundHealth;

	void OnGUI()
	{
		Enemy_UFO ufo = gameObject.GetComponent<Enemy_UFO>();

		if (ufo.EnemyInFightMode () == true) 
		{
			GUI.Label (new Rect (Screen.width/2-100, 13, 200, 50), backgroundHealth);

			Enemy_Health eh = gameObject.GetComponent<Enemy_Health> ();

			for (int i = 0; i < 11; i++) 
			{
				if (i < eh.getHealth ())
					GUI.Label (new Rect (Screen.width / 2 - 90 + i * 10, 20, 10, 25), greenHealth);
				else
					GUI.Label (new Rect (Screen.width / 2 - 90 + i * 10, 20, 10, 25), redHealth);
			}
		}
	}
}
