using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LaserBehaviour : NetworkBehaviour {

    public float duration = 0.5f;
    float startTime;
    LineRenderer lr;
    Material mat;
    public Vector3[] positions;

	void Start () {
        startTime = Time.time;
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
        mat = GetComponent<Renderer>().material;
        lr.SetPositions(positions);
    }

    void SetAlpha(Material material, float value)
    {
        Color color = material.color;
        color.a = value;
        material.color = color;
    }

    void Update()
    {
        SetAlpha(mat, (duration - (Time.time - startTime)) / duration);
    }

    [ServerCallback]
    void FixedUpdate()
    {
        if (Time.time - startTime > duration && isServer)
            CmdDestroy();
    }

    [Command]
    void CmdDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    [ClientRpc]
    public void RpcSetPositions(Vector3[] pos)
    {
        positions = pos;
    }
}
