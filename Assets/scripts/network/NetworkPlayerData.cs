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
    [SyncVar] public string titleText = "";

    /* Race specific */
    [SyncVar] public bool raceActive = false;
    [SyncVar] public int numberOfLaps = 0;
    [SyncVar] public int numberOfWaypoints = 0;
    [SyncVar] public float raceTime = 0; /* TODO: Optimize */
    [SyncVar] public bool debugMode = false;


    public void OnRaceInitializeData()
    {
        if(isLocalPlayer)
        {
            PlayerDataTransfer data = GameObject.FindGameObjectWithTag("PlayerDataTransfer").GetComponent<PlayerDataTransfer>();

            playerName = data.playerName;
        }
    }
}
