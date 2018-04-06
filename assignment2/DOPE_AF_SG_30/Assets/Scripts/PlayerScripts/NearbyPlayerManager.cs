using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NearbyPlayerManager : NetworkBehaviour {

    GameManager gm;
    public float nearbyPlayerDistance = 6;
    public GameObject downedPlayerWarning;

    void Start()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (downedPlayerWarning == null)
            downedPlayerWarning = GameObject.Find("AllyDownWarning");
        downedPlayerWarning.GetComponent<Image>().color = Color.white;
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;

        List<GameObject> players = gm.getPlayers();
        List<GameObject> nearbyPlayers = new List<GameObject>();
        List<GameObject> otherPlayers = new List<GameObject>();
        foreach(GameObject g in players)
        {
            if (g != this.gameObject && (g.transform.position - transform.position).magnitude < nearbyPlayerDistance)
                nearbyPlayers.Add(g);
            if (g != this.gameObject)
                otherPlayers.Add(g);
        }

        bool downedPlayerExists = false;
        foreach(GameObject g in otherPlayers)
        {
            PlayerHealth gph = g.GetComponent<PlayerHealth>();
            if (gph.getHealthState() == PlayerHealth.HealthState.downed)
            {
                downedPlayerExists = true;
            }
        }

        /*foreach (GameObject g in nearbyPlayers)
        {
            PlayerHealth gph = g.GetComponent<PlayerHealth>();
            if (gph.getHealthState() == PlayerHealth.HealthState.downed)
            {
#if UNITY_EDITOR || UNITY_STANDALONE
                if (Input.GetKeyDown(KeyCode.E))
                {
                    CmdRevivePlayer(g);
                }
#endif
#if UNITY_ANDROID && !UNITY_EDITOR
                //gph.CmdRevive();
#endif
            }
        }*/
        downedPlayerWarning.SetActive(downedPlayerExists);
    }

    [Command]
    void CmdRevivePlayer(GameObject player)
    {
        player.GetComponent<PlayerHealth>().RpcRevive();
    }
}
