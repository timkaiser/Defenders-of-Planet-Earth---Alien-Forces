using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Enemy_Health : NetworkBehaviour 
{
	[SyncVar]
	private int health = 11;
    private GameObject lastDamageDealer = null;
    private float lastHitTime = 0.0f;

	[HideInInspector]
	public const int MAXHEALTH = 11;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void receiveDamage(int amount, GameObject byPlayer)
	{
		if (!isServer || health <= 0)
			return;

		health -= amount;
        lastDamageDealer = byPlayer;
        lastHitTime = Time.time;
        //Debug.Log("Enemy hit! Only " + health + " HP remaining!");

		if (health <= 0) 
		{
			//Set health to zero
			health = 0;

			//The enemy needs to disappear on all clients
			NetworkServer.Destroy(gameObject);
		}
	}

	public int getHealth()
	{
		return health;
	}

    public GameObject getLastDamageDealer()
    {
        return lastDamageDealer;
    }

    public float getLastHitTime()
    {
        return lastHitTime;
    }
}
