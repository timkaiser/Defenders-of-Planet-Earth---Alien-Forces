using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AutodestroyParticleEmitterNetworked : NetworkBehaviour {

    ParticleSystem ps;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        StartCoroutine(destroyPS());
    }

    IEnumerator destroyPS()
    {
        while (ps.IsAlive())
        {
            yield return new WaitForSeconds(1);
        }
        if(isServer)
            CmdDelete();
    }

    [Command]
    void CmdDelete()
    {
        NetworkServer.Destroy(gameObject);
    }
}
