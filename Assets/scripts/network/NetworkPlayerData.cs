﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public struct WaypointData
{

}

public class NetworkPlayerData : NetworkBehaviour {

    /* Player specific */
    [SyncVar] public string playerName = "[E]";
    [SyncVar] public int currentLap = 0;
    [SyncVar] public int currentWaypoint = 0;
    [SyncVar] public int uniqueID = 0;
    [SyncVar] public string titleText = "";
    [SyncVar] public Color color = new Color32(0x69, 0x42, 0x00, 0xFF);

    /* Race specific */
    [SyncVar] public bool raceActive = false;
    [SyncVar] public int numberOfLaps = 0;
    [SyncVar] public int numberOfWaypoints = 0;
    [SyncVar] public float raceTime = 0; /* TODO: Optimize */
    [SyncVar] public bool debugMode = false;


    private int m_Count = 0;

    void FixedUpdate()
    {
        //if (m_Count > 2000) return;
        if (!isLocalPlayer) return;
        
        playerName = PlayerDataTransfer.instance.playerName;
        m_Count++;
    }

    public void OnRaceInitializeData()
    {
        if(isLocalPlayer)
        {
            playerName = PlayerDataTransfer.instance.playerName;
            Debug.Log("Aquired Player Data: " + playerName);
        }
    }
}
