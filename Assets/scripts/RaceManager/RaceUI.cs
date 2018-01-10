using System.Collections;
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

                    WWW get = new WWW("http://faoiltiarna.ddns.net/addscore/filavandrel/" + PlayerDataTransfer.instance.playerName + "/" + f_LapTime + "/anyway");
                    Debug.Log("WWW: " + PlayerDataTransfer.instance.playerName + ", " + f_LapTime);
                    lapTime.GetComponent<Text>().text = "" + f_LapTime.ToString("0.00");
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
        timeText.text = m_Data.raceTime.ToString("0.00");
        lapText.text = m_Data.currentLap + " / " + m_Data.numberOfLaps;
        wpText.text = m_Data.currentWaypoint + " / " + m_Data.numberOfWaypoints;

        titleText.text = m_Data.titleText;
    }
}
