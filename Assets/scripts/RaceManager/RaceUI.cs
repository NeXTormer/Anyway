using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

[AddComponentMenu("RaceManager/RaceUI")]
[RequireComponent(typeof(NetworkPlayerData))]
public class RaceUI : MonoBehaviour {

    public Text racetext;

    private GameObject player;
    private StringBuilder sb;
    private NetworkPlayerData data;

    void Awake()
    {
        sb = new StringBuilder(60);
        player = this.transform.root.gameObject;
        data = player.GetComponent<NetworkPlayerData>();
    }


    void LateUpdate()
    {
        if(data != null && data.raceActive)
        {
            UpdateText();
        }
    }

    public void UpdateText()
    {
        /* TODO: performance? */
        sb = new StringBuilder(70);
        sb.Append("Name: ");
        sb.AppendLine(data.playerName);
        sb.Append("Waypoint: ");
        sb.Append(data.currentWaypoint);
        sb.Append(" / ");
        sb.Append(data.numberOfWaypoints);
        sb.Append("\nLap: ");
        sb.Append(data.currentLap);
        sb.Append(" / ");
        sb.Append(data.numberOfLaps);
        sb.Append("\n");
        sb.Append("Time: ");
        sb.Append(data.raceTime.ToString("0.00"));

        racetext.text = sb.ToString();
    }
}
