using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutodestroyParticleEmitter : MonoBehaviour {

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
        Destroy(gameObject);
    }
}
