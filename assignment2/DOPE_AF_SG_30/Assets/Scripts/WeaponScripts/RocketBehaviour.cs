using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RocketBehaviour : NetworkBehaviour 
{
    Rigidbody rb;
    public float rocketTopSpeed = 12;
    public float lifeTime = 3;
    public GameObject explosion;
    float spawnTime;
    GameObject particles;
    ParticleSystem pEffect;
    float emissionRate;
    bool emissionSet = false;
    [HideInInspector]
    public GameObject sender;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        particles = HelperFunctions.getChildGameObject(gameObject, "Particles");
        spawnTime = Time.time;
        pEffect = particles.GetComponent<ParticleSystem>();
        var em = pEffect.emission;
        emissionRate = em.rateOverTime.constant;
        em.rateOverTime = 5;
    }

    [ServerCallback]
    void FixedUpdate()
    {
        rb.AddForce(transform.forward * (rocketTopSpeed - rb.velocity.magnitude));
        if (Time.time - spawnTime > lifeTime && isServer)
            CmdExplode();
    }

    void Update()
    {
        if (!emissionSet && Time.time - spawnTime > 0.1f)
        {
            var em = pEffect.emission;
            em.rateOverTime = emissionRate;
            emissionSet = true;
        }
    }

    [Command]
    void CmdExplode()
    {
        particles.transform.parent = null;
        var em = pEffect.emission;
        em.enabled = false;
        var main = pEffect.main;
        main.loop = false;
        NetworkServer.Spawn(Instantiate(explosion, transform.position, Quaternion.identity));
        NetworkServer.Destroy(gameObject);
    }

    void OnCollisionEnter(Collision other)
    {
        if (!isServer)
            return;

		if (sender == null)
			return;

		if (other.gameObject.tag == sender.tag)
			return;

		CmdExplode();

        if (sender.tag == "Player" && other.gameObject.tag == "Enemy")
        {
            Enemy_Health eh = other.gameObject.GetComponent<Enemy_Health>();
            Bomb b = other.gameObject.GetComponent<Bomb>();
            if (eh != null)
            {
                eh.receiveDamage(3, sender);
            }
            else if(b != null)
            {
                b.m_dead = true;
            }
        }
        else if(sender.tag == "Enemy" && other.gameObject.tag == "Player")
        {
            PlayerHealth ph = other.gameObject.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.RpcReceiveDamage(4, sender);
            }
        }
    }
}
