using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerData
{
    public PlayerData(GameObject ob)
    {
        player = ob;

        //0 -> race hasn't begun yet, 1 -> first lap (directly after start)
        currentLap = 0;
        currentWaypoint = 0;
    }

    public GameObject player;
    public int currentLap;
    public int currentWaypoint;
}

public class RaceManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject[] waypoints;
    public GameObject[] players;

    public int numberOfLaps = 1;

    [Header("Data")]
    [ReadOnly]
    public PlayerData[] playerdataView;

    private Dictionary<string, PlayerData> playerData;

    void Start()
    {
        playerData = new Dictionary<string, PlayerData>();

        foreach(GameObject pl in players)
        {
            AddPlayer(pl);
        }
        playerdataView = new PlayerData[playerData.Values.Count];


        //add playerdata references to the playerdataview array in order to display them in the inspector
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
        if(playerData.ContainsKey(player.name))
        {
            PlayerData data = playerData[player.name];
            Waypoint wp = waypoint.GetComponent<Waypoint>();

            if(wp.type == WaypointType.Waypoint)
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
            else if(wp.type == WaypointType.Finish)
            {
                if ((data.currentWaypoint + 1) == waypoints.Length)
                {
                    //right waypoint
                    data.currentLap++;
                    if(data.currentLap == numberOfLaps)
                    {
                        //player has finished the race
                    }
                }
                else
                {
                    //wrong waypoint
                }
            }
            else if(wp.type == WaypointType.Start)
            {
                if(data.currentLap == 0)
                {
                    data.currentLap = 1;
                    //player started the first lap
                }
            }

        }
    }

    public void StartRace()
    {
        Debug.Log("Start Race");
        
    }

    


	
	
	void Update()
    {
		
	}
}
