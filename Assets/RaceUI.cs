using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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
        int maxwp = racemanager.waypoints.Length;
        int nol = racemanager.numberOfLaps;

        //TODO: performance?
        sb = new StringBuilder(60);
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

        racetext.text = sb.ToString();
    }
	
	
}
