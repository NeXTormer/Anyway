using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public PlayerData(GameObject ob)
    {
        player = ob;

        //-1 -> race hasn't begun yet, 0 -> first lap (directly after start), 1 -> first lap finished -> started second lap
        currentLap = -1;
        currentWaypoint = 0;
    }

    public GameObject player;
    public int currentLap;
    public int currentWaypoint;
}

public class RaceManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] players;

    public int numberOfLaps = 1;

    [Header("Data")]
    public PlayerData[] playerdataView;

    public float raceTime = 0;

    private bool raceActive = false;


    public Dictionary<string, PlayerData> playerData;

    [HideInInspector]
    public GameObject[] waypoints;

    void Start()
    {
        Waypoint[] tempwaypoints = GetComponentsInChildren<Waypoint>();
        
        for(int i = 0; i < tempwaypoints.Length; i++)
        {
            waypoints[i] = tempwaypoints[i].gameObject;
        }

        playerData = new Dictionary<string, PlayerData>();

        foreach(GameObject pl in players)
        {
            Debug.Log("addplayer: " + pl.name);
            AddPlayer(pl);
        }

        //add playerdata references to the playerdataview array in order to display them in the inspector
        playerdataView = new PlayerData[playerData.Values.Count];
        int count = 0;
        foreach(KeyValuePair<string, PlayerData> pair in playerData)
        {
            playerdataView[count] = pair.Value;
            count++;
        }


    }
    
    public void AddPlayer(GameObject player)
    {
        playerData.Add(player.name, new PlayerData(player));
    }

    public void OnWaypointHit(GameObject player, GameObject waypoint)
    {
        if(raceActive)
        {
            if (playerData.ContainsKey(player.name))
            {

                PlayerData data = playerData[player.name];
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
                    if ((data.currentWaypoint + 1) == (waypoints.Length - 1))
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
        }
    }
}
