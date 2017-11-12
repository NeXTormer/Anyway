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
        int currentlap = racemanager.playerData[player.name].currentLap;
        int currentwaypoint = racemanager.playerData[player.name].currentWaypoint;

        //Start and finish are also in the waypoints array, but they don't count as 'real' waypoints
        int maxwp = racemanager.waypoints.Length - 2;
        int nol = racemanager.numberOfLaps;

        float racetime = racemanager.raceTime;

        //TODO: performance?
        sb = new StringBuilder(70);
        sb.Append("Name: ");
        sb.AppendLine(player.name);
        sb.Append("Waypoint: ");
        sb.Append(currentwaypoint);
        sb.Append(" / ");
        sb.Append(maxwp);
        sb.Append("\nLap: ");
        sb.Append(currentlap);
        sb.Append(" / ");
        sb.Append(nol);
        sb.Append("\n");
        sb.Append("Time: ");
        sb.Append(racetime.ToString("0.00"));


        racetext.text = sb.ToString();
    }
	
	
}
