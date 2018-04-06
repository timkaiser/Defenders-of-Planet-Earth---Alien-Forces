using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNetworkManager : NetworkBehaviour {
    [System.Serializable]
    public class AutoSetup
    {
        public GameObject mainCamera;
        public GameObject weaponCamera;
        public GameObject weapon;
    }

    public AutoSetup autoSetup;
    GameManager gm;

    void Start()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        gm.addPlayer(gameObject);
        if (!isLocalPlayer)
        {
            Destroy(autoSetup.mainCamera);
            Destroy(autoSetup.weaponCamera);
            autoSetup.weapon.layer = 0;
            foreach(Transform child in GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 0;
            }
        }
        else
        {
            gm.setLocalPlayer(gameObject);
        }
    }

    void OnDestroy()
    {
        gm.removePlayer(gameObject);
    }
}
