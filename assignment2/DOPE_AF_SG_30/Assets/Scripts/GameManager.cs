using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour {

    List<GameObject> players = new List<GameObject>();
    float startLat;
    float startLon;
    public bool centerCoordinatesSet = false;
    GameObject localPlayer;

    public void addPlayer(GameObject player)
    {
        if(!players.Contains(player))
            players.Add(player);
    }

    public void setLocalPlayer(GameObject player)
    {
        localPlayer = player;
    }

    public GameObject getLocalPlayer()
    {
        return localPlayer;
    }

    public void removePlayer(GameObject player)
    {
        players.Remove(player);
    }

    public List<GameObject> getPlayers()
    {
        return players;
    }

    void setStartLatLong(float lat, float lon)
    {
        if (!centerCoordinatesSet)
        {
            startLat = lat;
            startLon = lon;
        }
        centerCoordinatesSet = true;
    }

    public float[] getStartLatLon()
    {
        return new float[]{startLat, startLon};
    }
}