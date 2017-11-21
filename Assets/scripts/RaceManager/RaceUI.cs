﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

[AddComponentMenu("RaceManager/RaceUI")]
public class RaceUI : MonoBehaviour {

    public Text racetext;

    private GameObject player;
    private StringBuilder sb;
    private RaceManager racemanager;

    private int t_CurrentLap;
    private int t_CurrentWaypoint;

    void Awake()
    {
        sb = new StringBuilder(60);
        player = this.transform.root.gameObject;
        racemanager = GameObject.FindGameObjectWithTag("RaceManager").GetComponent<RaceManager>();
    }


    void LateUpdate()
    {
        if(racemanager.raceActive)
        {
            UpdateText();
        }
    }

    public void UpdateText()
    {
        try
        {
            t_CurrentLap = racemanager.PlayerData[player.GetInstanceID()].currentLap;
            t_CurrentWaypoint = racemanager.PlayerData[player.GetInstanceID()].currentWaypoint;
        } 
        catch(KeyNotFoundException e)
        {
            try
            {
                Debug.LogWarning("Key not found (" + player.name + ") because the dictionary hasn't been fully built yet. Not a big problem if it only occurs once.");
            }
            catch (NullReferenceException ex)
            {
                Debug.LogWarning("Key not found ( NULL ) because the dictionary hasn't been fully built yet. Not a big problem if it only occurs once.");
            }
        }

        //Start and finish are also in the waypoints array, but they don't count as 'real' waypoints
        int maxwp = racemanager.Waypoints.Length - 2;
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
