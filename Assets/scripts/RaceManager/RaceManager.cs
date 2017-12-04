using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class PlayerData
{
    public PlayerData(GameObject ob)
    {
        player = ob;

        //-1 -> race hasn't begun yet, 0 -> first lap (directly after start), 1 -> first lap finished -> started second lap
        currentLap = -1;
        currentWaypoint = 0;
        name = player.name;
    }

    public string name;
    public GameObject player;
    public int currentLap;
    public int currentWaypoint;
}

[AddComponentMenu("RaceManager/RaceManager")]
public class RaceManager : NetworkBehaviour
{

    [Header("Settings")]
    public int numberOfLaps = 1;

    [Header("Data")]

    public PlayerData[] playerdataView;
    
    public bool raceActive = false;
    public float raceTime = 0;

    private GameObject[] m_Players;
    private GameObject[] m_Waypoints;
    private Dictionary<int, PlayerData> m_PlayerData;


    private void Awake()
    {
        m_PlayerData = new Dictionary<int, PlayerData>();
    }

    private void InitializeNetworkPlayerData()
    {
        foreach(KeyValuePair<int, PlayerData> pair in m_PlayerData)
        {
            PlayerData data = pair.Value;
            NetworkPlayerData networkData = data.player.GetComponent<NetworkPlayerData>();
            networkData.playerName = data.name;
            networkData.currentLap = data.currentLap;
            networkData.currentWaypoint = data.currentWaypoint;
            networkData.raceActive = raceActive;
            networkData.numberOfLaps = numberOfLaps;
            networkData.numberOfWaypoints = m_Waypoints.Length - 2;
            networkData.raceTime = raceTime;
            networkData.uniqueID = data.player.GetInstanceID();

            data.player.GetComponent<NetworkPlayerData>().SendMessage("OnRaceInitializeData");
        }
    }   

    private void UpdateNetworkPlayerData()
    {
        foreach (KeyValuePair<int, PlayerData> pair in m_PlayerData)
        {
            PlayerData data = pair.Value;
            NetworkPlayerData networkData = data.player.GetComponent<NetworkPlayerData>();
            networkData.currentLap = data.currentLap;
            networkData.currentWaypoint = data.currentWaypoint;
            networkData.raceActive = raceActive;
            networkData.raceTime = raceTime;
        }
    }

    //Should be called before the game start, but after adding all players
    public void InitializeRace()
    {
        Debug.Log("Initializing RaceManager");

        Waypoint[] tempwaypoints = GetComponentsInChildren<Waypoint>();

        m_Waypoints = new GameObject[tempwaypoints.Length];

        for (int i = 0; i < tempwaypoints.Length; i++)
        {
            m_Waypoints[i] = tempwaypoints[i].gameObject;
        }

        foreach (GameObject pl in m_Players)
        {
            Debug.Log("Add player to race: " + pl.name);
            AddPlayer(pl);
        }

        //add playerdata references to the playerdataview array in order to display them in the inspector
        playerdataView = new PlayerData[m_PlayerData.Values.Count];
        int count = 0;
        foreach (KeyValuePair<int, PlayerData> pair in m_PlayerData)
        {
            playerdataView[count] = pair.Value;
            count++;
        }

        /* Initialize NetworkPlayerData on all players in dictionary */
        InitializeNetworkPlayerData();
    }

    /* Adds players to array of potential players, but not the race */
    public void AddAllPlayers()
    {
        m_Players = GameObject.FindGameObjectsWithTag("Player");
    }

    /* Adds player to race */
    public void AddPlayer(GameObject player)
    {
        m_PlayerData.Add(player.GetInstanceID(), new PlayerData(player));
    }

    public void OnWaypointHit(GameObject player, GameObject waypoint)
    {

        if(raceActive)
        {

            if (m_PlayerData.ContainsKey(player.GetInstanceID()))
            {
                
                PlayerData data = m_PlayerData[player.GetInstanceID()];
                Waypoint wp = waypoint.GetComponent<Waypoint>();

                if (wp.type == WaypointType.Waypoint)
                {
                    if ((data.currentWaypoint + 1) == wp.waypointID)
                    {
                        //right waypoint
                        data.currentWaypoint++;
                    }
                    else
                    {
                        //wrong waypoint
                    }
                }
                else if (wp.type == WaypointType.Finish)
                {
                    if ((data.currentWaypoint + 1) == (m_Waypoints.Length - 1))
                    {
                        data.currentLap++;
                        if (data.currentLap == numberOfLaps)
                        {
                            //player has finished the race
                            StopRace();
                            return;
                        }
                        //right waypoint, player has finished one lap
                        data.currentWaypoint = 0;

                    }
                    else
                    {
                        //wrong waypoint
                    }
                }
                else if (wp.type == WaypointType.Start)
                {
                    if (data.currentLap == -1)
                    {
                        Debug.Log("Started Race (Lap -1 -> Lap 0)");
                        data.currentLap = 0;

                        //player started the first lap
                    }
                }
            }
        }    
    }

    public void StartRace()
    {
        raceActive = true;
        raceTime = 0;
    }

    public void StopRace()
    {
        raceActive = false;
    }
    
	
	void FixedUpdate()
    {
        if(raceActive)
        {
            raceTime += Time.deltaTime;

            UpdateNetworkPlayerData();
        }
    }


}
