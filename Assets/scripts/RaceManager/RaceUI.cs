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

    private GameObject player;
    private NetworkPlayerData data;

    private int m_OldLap = -1;
    private float m_LapStartTime = 0;

    void Awake()
    {
        player = this.transform.root.gameObject;
        data = player.GetComponent<NetworkPlayerData>();
    }


    void LateUpdate()
    {
        if(data != null)
        {
            UpdateText();

            if (data.currentLap != m_OldLap && data.raceActive)
            {
                m_OldLap = data.currentLap;

                if (m_LapStartTime != 0)
                {
                    float laptime = Time.time - m_LapStartTime;

                    WWW get = new WWW("http://faoiltiarna.ddns.net/addscore/filavandrel/" + PlayerDataTransfer.instance.playerName + "/" + laptime + "/anyway");
                    Debug.Log("WWW: " + PlayerDataTransfer.instance.playerName + ", " + laptime);
                }

                m_LapStartTime = Time.time;
            }

        }
    }

    public void UpdateText()
    {
        usernameText.text = PlayerDataTransfer.instance.playerName;
        timeText.text = data.raceTime.ToString("0.00");
        lapText.text = data.currentLap + " / " + data.numberOfLaps;
        wpText.text = data.currentWaypoint + " / " + data.numberOfWaypoints;

        titleText.text = data.titleText;
    }
}
