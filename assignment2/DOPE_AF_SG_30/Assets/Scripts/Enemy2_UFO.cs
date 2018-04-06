using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class Enemy2_UFO : NetworkBehaviour
{
	private float lastTimeOfFire = 0;
    private Enemy_Health healthScript;
	private Transform playerTransform = null;

	private Vector3 direction = Vector3.forward;
	private float TimeChangeDirection = 0.0f;

	public float reloadSpeed = 5.0f;
	private float lastCheck = 0.0f;

	public GameObject bomb_prefab;

	[SyncVar]
	private bool fightMode = false;

	// Use this for initialization
	void Start ()
	{
        healthScript = GetComponent<Enemy_Health>();
	}
	
	// Update is called once per frame
    [ServerCallback]
	void Update ()
    {
		if (gameObject != null && isServer) 
		{
			CheckWhichPLayerToAttack ();

			Move();

			CmdEnemyFire ();
		}
	}

	[Command]
	void CmdEnemyFire()
	{
		if (fightMode && Time.time - lastTimeOfFire > reloadSpeed)
		{
			//Bombe abwerfen
			try
			{
                NetworkServer.Spawn(Instantiate(bomb_prefab, transform.position, bomb_prefab.transform.rotation));
				lastTimeOfFire = Time.time;
			}
			catch(NullReferenceException) 
			{
				Debug.Log ("Error in Enemy2_UFO -> CmdEnemyFire: bomb is null!");
			}
		}
	}

	public void CheckWhichPLayerToAttack()
	{
		Enemy_Health eh = gameObject.GetComponent<Enemy_Health>();

		if(lastCheck == 0.0f || Time.time - lastCheck > 5.0f)
		{
			List<Collider> playerList = new List<Collider> ();
			Collider[] otherColliders = Physics.OverlapSphere (transform.position, 25.0f);
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

	public void Move()
	{
		if (fightMode) 
		{
			if (Time.time - TimeChangeDirection > 8.0f) 
			{
				if (playerTransform != null) 
				{
					Vector3 lookAtPosition = new Vector3 (playerTransform.position.x, transform.position.y, playerTransform.position.z);
					transform.LookAt (lookAtPosition); 
					TimeChangeDirection = Time.time;
				}
			}

			transform.Translate (direction * Time.deltaTime * 2);
		}
	}

	public bool EnemyInFightMode()
	{
		return fightMode;
	}
}
