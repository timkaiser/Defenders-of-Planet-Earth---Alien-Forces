using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCenter : MonoBehaviour {
    public GameObject origin;

    void Start()
    {
        if (origin == null)
            origin = GameObject.Find("originCube");
    }

	void Update () {
        transform.LookAt(origin.transform);
	}
}
