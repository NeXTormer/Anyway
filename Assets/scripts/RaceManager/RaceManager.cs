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
    public float lapStartTime = 0;

}

[AddComponentMenu("RaceManager/RaceManager")]
public class RaceManager : NetworkBehaviour
{
    [Header("Settings")]
    public int numberOfLaps = 1;

    [Header("Data")]

    /* TODO: replace with better solution */
    public PlayerData[] playerdataView;

    public bool debugMode = false;

    public bool autostartRace = true;
    public int minPlayersToAutostart = 2;

    public float defaultRaceCountdown = 5;

    public bool raceActive = false;
    public float raceTime = 0;

    private GameObject[] m_Players;
    private GameObject[] m_Waypoints;
    private Dictionary<int, PlayerData> m_PlayerData;
    private float m_RaceCountdown = 0;
    private bool m_RaceCountdownActive = false;
    private bool m_StartedInitialization = false;

    private List<Color> m_Colors;

    private void Awake()
    {
        m_PlayerData = new Dictionary<int, PlayerData>();
        InitializeColors();
    }

    private void InitializeNetworkPlayerData()
    {
        foreach(var pair in m_PlayerData)
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
            networkData.titleText = "findenig";
            networkData.debugMode = debugMode;
            networkData.color = SelectRandomColor();

            data.player.GetComponent<NetworkPlayerData>().SendMessage("OnRaceInitializeData");
        }
    }   

    private void UpdateNetworkPlayerData()
    {
        string title = "";
        if(m_RaceCountdownActive)
        {
            if(m_RaceCountdown < defaultRaceCountdown)
            {
                title = "Race starting in: " + (defaultRaceCountdown - m_RaceCountdown).ToString("0.00");
            }
            else if(m_RaceCountdown > defaultRaceCountdown && m_RaceCountdown < defaultRaceCountdown + 1)
            {
                title = "-===- GO! -===-";
            }   
        }

        foreach (KeyValuePair<int, PlayerData> pair in m_PlayerData)
        {
            PlayerData data = pair.Value;
            NetworkPlayerData networkData = data.player.GetComponent<NetworkPlayerData>();
            networkData.currentLap = data.currentLap;
            networkData.currentWaypoint = data.currentWaypoint;
            networkData.raceActive = raceActive;
            networkData.raceTime = raceTime;
            networkData.titleText = title;
        }
    }

    //Should be called before the game start, but after adding all players
    public void InitializeRace()
    {
        Debug.Log("Initializing RaceManager");

        m_Waypoints = GameObject.FindGameObjectsWithTag("Waypoint");

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

    private void InitializeColors()
    {
        m_Colors = new List<Color>
        {
            new Color32(0x5C, 0xA6, 0xFF, 0xFF),
            new Color32(0xD1, 0x5E, 0xFF, 0xFF),
            new Color32(0xFF, 0xE4, 0x5E, 0xFF),
            new Color32(0xFF, 0x5E, 0x5E, 0xFF),
            new Color32(0x1E, 0xFF, 0x61, 0xFF),
            new Color32(0x77, 0x77, 0x77, 0xFF),
            new Color32(0xFF, 0x8C, 0x35, 0xFF)
        };
    }

    /* Adds players to array of potential players, but not the race */
    public void AddAllPlayers()
    {
        m_Players = GameObject.FindGameObjectsWithTag("Player");
    }

    /* Adds player to race */
    public void AddPlayer(GameObject player)
    {
        if(!m_PlayerData.ContainsKey(player.GetInstanceID()))
        {
            m_PlayerData.Add(player.GetInstanceID(), new PlayerData(player));
        }
    }

    public void OnWaypointHit(GameObject player, GameObject waypoint)
    {

        if (raceActive)
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

        foreach (var pair in m_PlayerData)
        {
            pair.Value.lapStartTime = Time.time;
        }
    }

    /* starts race countdown | starts race after countdown ends */
    public void StartRaceCountdown()
    {
        Debug.Log("Started race countdown");
        m_RaceCountdown = 0;
        m_RaceCountdownActive = true;
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

        }
        if(m_RaceCountdownActive)
        {
            m_RaceCountdown += Time.deltaTime;

            if(m_RaceCountdown >= defaultRaceCountdown)
            {
                StartRace();

            }
            if (m_RaceCountdown > (defaultRaceCountdown + 1))
            {
                m_RaceCountdownActive = false;
            }
        }

        if(autostartRace)
        {
            if(!m_StartedInitialization)
            {
                if (NetworkManager.singleton.numPlayers >= minPlayersToAutostart)
                {
                    AddAllPlayers();
                    InitializeRace();
                    StartRaceCountdown();
                    m_StartedInitialization = true;
                }
            }
        }
        

        UpdateNetworkPlayerData();
    }

    private Color SelectRandomColor()
    {
        if (m_Colors.Count == 0)
        {
            InitializeColors();
        }

        var random = new System.Random();
        int i = random.Next(0, m_Colors.Count);
        Color c = m_Colors[i];
   
        m_Colors.Remove(c);

        return c;
    }
}
