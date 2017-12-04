using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkPlayerData : NetworkBehaviour {

    /* Player specific */
    [SyncVar] public string playerName = "";
    [SyncVar] public int currentLap = 0;
    [SyncVar] public int currentWaypoint = 0;
    [SyncVar] public int uniqueID = 0;

    /* Race specific */
    [SyncVar] public bool raceActive = false;
    [SyncVar] public int numberOfLaps = 0;
    [SyncVar] public int numberOfWaypoints = 0;
    [SyncVar] public float raceTime = 0; /* TODO: Optimize */


    public void OnRaceInitializeData()
    {
        if(isLocalPlayer)
        {
            playerName = GameObject.FindGameObjectWithTag("PlayerDataTransfer").GetComponent<PlayerDataTransfer>().playerName;
        }
    }
}
