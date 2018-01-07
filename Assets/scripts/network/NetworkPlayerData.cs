using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    [Header("Settings")]

    public Username username;

    private int m_Count = 0;

    void Start()
    {
        if (!isLocalPlayer) return;

        username.PlayerName = PlayerDataTransfer.instance.playerName;
        playerName = PlayerDataTransfer.instance.playerName;


        m_Count++;
    }

    /// <summary>
    /// Message sent by the RaceManager when the player data has been initialized.
    /// </summary>
    public void OnRaceInitializeData()
    {
        if(isLocalPlayer)
        {
            playerName = PlayerDataTransfer.instance.playerName;
            username.PlayerName = PlayerDataTransfer.instance.playerName;
            Debug.Log("Aquired Player Data: " + playerName);
        }
    }
}
