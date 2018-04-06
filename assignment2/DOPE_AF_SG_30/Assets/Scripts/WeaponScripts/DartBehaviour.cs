using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DartBehaviour : NetworkBehaviour{

    Rigidbody rb;
    public float lifeTime = 4;
    public GameObject healingParticles;
    float spawnTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        spawnTime = Time.time;
    }

    [ServerCallback]
    void FixedUpdate()
    {
        if (Time.time - spawnTime > lifeTime && isServer)
            CmdExplode();
    }

    [Command]
    void CmdExplode()
    {
        NetworkServer.Spawn(Instantiate(healingParticles, transform.position, Quaternion.Euler(-90,0,0)));
        NetworkServer.Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isServer)
            return;

        if (other.gameObject.tag == "Player")
        {
            CmdRevivePlayer(other.gameObject);
            CmdExplode();
        }
    }

    [Command]
    void CmdRevivePlayer(GameObject player)
    {
        player.GetComponent<PlayerHealth>().RpcRevive();
    }
}
