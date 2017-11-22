using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;
using UnityEngine.Networking;

[AddComponentMenu("RaceManager/RaceUI")]
public class RaceUI : NetworkBehaviour {

    public Text racetext;

    private StringBuilder sb;

    private int n_CurrentLap;
    private int n_CurrentWaypoint;
    private int n_MaxLap;
    private int n_MaxWaypoint;
    private string n_Playername;
    private bool n_RaceActive;
    private float n_RaceStartTime;

    private float m_RaceTime = 0f;

    void Awake()
    {
        sb = new StringBuilder(60);
    }

    [ClientRpc]
    public void RpcUpdateValues(int currentlap, int currentwp, int maxlap, int maxwp, string name, bool raceactive, float raceStarted)
    {
        n_CurrentLap = currentlap;
        n_CurrentWaypoint = currentwp;
        n_MaxLap = maxlap;
        n_MaxWaypoint = maxwp;
        n_Playername = name;
        n_RaceActive = raceactive;
        n_RaceStartTime = raceStarted;
    }

    

    void LateUpdate()
    {
        if(n_RaceActive)
        {
            UpdateText();
        }
    }

    public void UpdateText()
    {
        sb = new StringBuilder(70);
        sb.Append("Name: ");
        sb.AppendLine(n_Playername);
        sb.Append("Waypoint: ");
        sb.Append(n_CurrentWaypoint);
        sb.Append(" / ");
        sb.Append(n_MaxWaypoint);
        sb.Append("\nLap: ");
        sb.Append(n_CurrentLap);
        sb.Append(" / ");
        sb.Append(n_MaxLap);
        sb.Append("\n");
        sb.Append("Time: ");
        sb.Append(m_RaceTime.ToString("0.00"));

        racetext.text = sb.ToString();
    }
	
	
}
