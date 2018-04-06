using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class EnemySpawner : NetworkBehaviour {

    public GameObject UFO1;
    public GameObject UFO2;
    public int numberofWaves = 4;

    void Start()
    {
        if (isServer)
        {
            StartCoroutine(spawnRoutine());
        }
    }

    void SpawnUFO1(Vector3 position)
    {
        NetworkServer.Spawn(Instantiate(UFO1, position, Quaternion.identity));
    }

    void SpawnUFO2()
    {
        NetworkServer.Spawn(Instantiate(UFO2, new Vector3(0, 5, 0), Quaternion.identity));
    }

    IEnumerator spawnRoutine()
    {
        int wavesRemaining = numberofWaves;
        while(wavesRemaining >= -1)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(enemies.Length == 0)
            {
                yield return new WaitForSeconds(3);
                switch (numberofWaves - wavesRemaining)
                {
                    case 1: SpawnUFO1(new Vector3(0, 5, -7.5f)); break;
                    case 2: SpawnUFO2(); break;
                    case 3: SpawnUFO1(new Vector3(0, 5, -7.5f)); SpawnUFO2(); break;
                    case 4: SpawnUFO1(new Vector3(4, 5, -7.5f)); SpawnUFO1(new Vector3(-4, 5, -7.5f)); SpawnUFO2(); break;
                }
                wavesRemaining--;
            }
            yield return new WaitForSeconds(1);
        }
        RpcWinGame();
        yield return null;
    }

    [ClientRpc]
    void RpcWinGame()
    {
        Destroy(GameObject.Find("GlobalVariables"));
        Destroy(GameObject.Find("LobbyManager"));
        SceneManager.LoadScene("WinScreen");
    }
}
