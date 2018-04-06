using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Enemy_UFO : NetworkBehaviour
{
	public GameObject enemy_rocket;
	public GameObject enemy_cannon;

	public float rocketSpeed = 10;
	public float reloadSpeed = 2;
	private float lastTimeOfFire = 0;
	[SyncVar]
	private bool fightMode = false;
    private Enemy_Health healthScript;
	private Transform playerTransform = null;
	private float lastCheck = 0.0f;

	// Use this for initialization
	void Start ()
	{
        healthScript = GetComponent<Enemy_Health>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (gameObject != null && isServer) 
		{
			CheckWhichPLayerToAttack ();

			CheckPlayerDeath ();

			CmdEnemyFire ();

			if (fightMode && healthScript.getHealth () < (int)(0.50f * Enemy_Health.MAXHEALTH)) 
			{
				transform.RotateAround (playerTransform.position, Vector3.up, 20.0f * Time.deltaTime);
				transform.LookAt (playerTransform);
			} 
			else if (fightMode) 
			{
				transform.LookAt (playerTransform);
			} 
			else 
			{
				transform.Rotate (Vector3.up, 30.0f * Time.deltaTime);
			}
		}
	}

	[Command]
	void CmdEnemyFire()
	{
		if (fightMode && Time.time - lastTimeOfFire > reloadSpeed)
		{
			try
			{
				GameObject r = Instantiate(enemy_rocket, enemy_cannon.transform.position, enemy_cannon.transform.rotation) as GameObject;
                RocketBehaviour rbehave = r.GetComponent<RocketBehaviour>();
                rbehave.sender = this.gameObject;
                Rigidbody rRb = r.GetComponent<Rigidbody>();
				rRb.velocity = r.transform.forward * rocketSpeed;
				lastTimeOfFire = Time.time;

				NetworkServer.Spawn(r);
			}
			catch(MissingReferenceException) 
			{
				Debug.Log("Error in UFOEnemy -> CmdFire: Rocket is null!");
			}
		}
	}

	public bool EnemyInFightMode()
	{
		return fightMode;
	}

	public void CheckWhichPLayerToAttack()
	{
		Enemy_Health eh = gameObject.GetComponent<Enemy_Health>();

		if (eh.getLastDamageDealer() != null && Time.time - eh.getLastHitTime () < 10.0f) 
		{
			playerTransform = eh.getLastDamageDealer().transform;
			fightMode = true;
		} 
		else if(lastCheck == 0.0f || Time.time - lastCheck > 5.0f)
		{
			List<Collider> playerList = new List<Collider> ();
			Collider[] otherColliders = Physics.OverlapSphere (transform.position, 10.0f);
			for (int i = 0; i < otherColliders.Length; i++) 
			{
				if (otherColliders[i].tag == "Player") 
				{
					playerList.Add(otherColliders[i]);
				}
			}

			if (playerList.Count != 0) 
			{
				playerTransform = playerList [UnityEngine.Random.Range (0, playerList.Count)].transform;
				fightMode = true;
			} 
			else 
			{
				playerTransform = null;
				fightMode = false;
			}

			lastCheck = Time.time;
		}
	}

	public void CheckPlayerDeath()
	{
		if (playerTransform != null) 
		{
			try {
				PlayerHealth ph = playerTransform.gameObject.GetComponent<PlayerHealth> ();

				if (fightMode && ph.getHealthState () != PlayerHealth.HealthState.alive) {
					fightMode = false;
				}

			} catch (NullReferenceException) {
				Debug.Log ("Error in Enemy_UFO -> CheckPlayerDeath: ph is null!");
			}
		} 
		else 
		{
			fightMode = false;
		}
	}
}
