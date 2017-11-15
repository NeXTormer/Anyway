using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

[AddComponentMenu("RaceManager/RaceUI")]
public class RaceUI : MonoBehaviour {

    public Text racetext;
    public RaceManager racemanager;
    public GameObject player;

    private StringBuilder sb;

    //Temp vars
    int t_CurrentLap;
    int t_CurrentWaypoint;

	void Start()
    {
        sb = new StringBuilder(60);
        UpdateText();
    }

    void LateUpdate()
    {
        UpdateText();   
    }

    public void UpdateText()
    {

        try
        {
            t_CurrentLap = racemanager.playerData[player.name].currentLap;
            t_CurrentWaypoint = racemanager.playerData[player.name].currentWaypoint;
        } 
        catch(KeyNotFoundException e)
        {
            Debug.LogWarning("Key not found (" + player.name + ") because the dictionary hasn't been fully built yet. Not a big problem.");
        }
        

        //Start and finish are also in the waypoints array, but they don't count as 'real' waypoints
        int maxwp = racemanager.waypoints.Length - 2;
        int nol = racemanager.numberOfLaps;

        float racetime = racemanager.raceTime;

        //TODO: performance?
        sb = new StringBuilder(70);
        sb.Append("Name: ");
        sb.AppendLine(player.name);
        sb.Append("Waypoint: ");
        sb.Append(t_CurrentWaypoint);
        sb.Append(" / ");
        sb.Append(maxwp);
        sb.Append("\nLap: ");
        sb.Append(t_CurrentLap);
        sb.Append(" / ");
        sb.Append(nol);
        sb.Append("\n");
        sb.Append("Time: ");
        sb.Append(racetime.ToString("0.00"));


        racetext.text = sb.ToString();
    }
	
	
}
