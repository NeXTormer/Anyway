﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

[AddComponentMenu("RaceManager/RaceUI")]
[RequireComponent(typeof(NetworkPlayerData))]
public class RaceUI : MonoBehaviour
{
    public Text usernameText;
    public Text timeText;
    public Text lapText;
    public Text wpText;
    public Text titleText;

    public GameObject lapTime;

    private GameObject m_Player;
    private NetworkPlayerData m_Data;
    private Rigidbody m_Body;

    private int m_OldLap = -1;
    private float m_LapStartTime = 0;

    void Awake()
    {
        m_Player = this.transform.root.gameObject;
        m_Data = m_Player.GetComponent<NetworkPlayerData>();
    }

    void Start()
    {
        lapTime.SetActive(false);
        m_Body = this.transform.root.gameObject.GetComponent<Rigidbody>();
    }

    void LateUpdate()
    {
        if(m_Data != null)
        {
            UpdateText();

            if (m_Data.currentLap != m_OldLap && m_Data.raceActive)
            {
                m_OldLap = m_Data.currentLap;

                if (m_LapStartTime != 0)
                {
                    float f_LapTime = Time.time - m_LapStartTime;

                    WWW get = new WWW("http://games.htl-klu.at/anyway/addscore/9F412BDFA1D49B0D80/" + PlayerDataTransfer.instance.playerName + "/" + f_LapTime + "/anyway");
                    
                    Debug.Log("WWW: " + PlayerDataTransfer.instance.playerName + ", " + f_LapTime);
                    int minutes = (int) f_LapTime / 60;
                    int seconds = (int) f_LapTime % 60;
                    int ms = (int) ((f_LapTime % 1) * 100);
                    lapTime.GetComponent<Text>().text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + ms.ToString("00");
                    lapTime.SetActive(false);
                    lapTime.SetActive(true);
                }

                m_LapStartTime = Time.time;
            }

        }
    }

    public void UpdateText()
    {
        usernameText.text = PlayerDataTransfer.instance.playerName;

        bool started = m_Data.currentLap != -1;

        float f_racetime = Time.time - m_LapStartTime;

        int minutes = (int) f_racetime / 60;
        int seconds = (int) f_racetime % 60;
        int ms = (int) ((f_racetime % 1) * 100);

        timeText.text = started ? minutes + ":" + seconds.ToString("00") + ":" + ms.ToString("00") : "00:00:00";
        //lapText.text = m_Data.currentLap + " / " + m_Data.numberOfLaps;
        lapText.text = m_Body.velocity.magnitude.ToString("0");
        wpText.text = m_Data.currentWaypoint + " / " + m_Data.numberOfWaypoints;

        titleText.text = m_Data.titleText;
    }
}
